using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionManager : Singleton<CollisionManager>
{
    private CollisionGroup _playerGroup = new CollisionGroup();
    private CollisionGroup _worldGroup = new CollisionGroup();

    [Tooltip("True for disable manager, use for editor scene")]
    public bool IsDisable;

    public void Start()
    {
        // init group
        _playerGroup = new CollisionGroup(Tool.BuilderGameplay.Tag.GetHashCode());
        _worldGroup = new CollisionGroup(Tool.BuilderWorld.Tag.GetHashCode().GetHashCode());
    }

    public void Register(CompCollision component, int tagHash)
    {
        if(IsDisable)
        {
            return;
        }

        // disable on editor
        if(SceneManager.GetActiveScene().name.GetHashCode() == "Editor".GetHashCode())
        {
            return;
        }

        if(tagHash == _playerGroup.Id)
        {
            _playerGroup.Add(component);
        }
        else if(tagHash == _worldGroup.Id)
        {
            _worldGroup.Add(component);
        }
    }

    public void UnRegister(CompCollision component, int tagHash)
    {
        if (IsDisable)
        {
            return;
        }

        if (tagHash == _playerGroup.Id)
        {
            _playerGroup.Remove(component);
        }
        else if (tagHash == _worldGroup.Id)
        {
            _worldGroup.Remove(component);
        }
    }

    public void FixedUpdate()
    {
        foreach(CompCollision playerCollision in _playerGroup.Components)
        {
            if(!playerCollision.Owner.activeSelf)
            {
                continue;
            }

            foreach(CompCollision worldCollision in _worldGroup.Components)
            {
                if(!worldCollision.Owner.activeSelf)
                {
                    continue;
                }

                // manage hit if we have
                playerCollision.Hit(worldCollision);
            }
        }
    }
}
