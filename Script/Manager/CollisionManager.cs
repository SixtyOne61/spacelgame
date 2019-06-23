using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionManager : Singleton<CollisionManager>
{
	// To do : order world collision separate in zone for reduce number of call
    //private CollisionGroup<CompCollisionPlayer> _playerGroup = new CollisionGroup<CompCollisionPlayer>();
    //private CollisionGroup<CompCollisionWorld> _worldGroup = new CollisionGroup<CompCollisionWorld>();
    //private CollisionGroup<CompCollisionBullet> _bulletGroup = new CollisionGroup<CompCollisionBullet>();
    //private CollisionGroup<CompShield> _shieldGroup = new CollisionGroup<CompShield>();

    [Tooltip("True for disable manager, use for editor scene")]
    public bool IsDisable;

    public void Start()
    {
        // TO DO : maybe add mask for collision between team etc
    }

    #region Register

    private bool CanRegister()
    {
        if (IsDisable)
        {
            return false;
        }

        // disable on editor
        if (SceneManager.GetActiveScene().name.GetHashCode() == "Editor".GetHashCode())
        {
            return false;
        }

        return true;
    }

    public void Register(CompCollision component)
    {
        // TO DO
       // _worldGroup.Add(component);
    }

    #endregion

    #region UnRegister

    public void UnRegister(CompCollision component)
    {
        // TO DO
    }
    
    #endregion

    public void FixedUpdate()
    {
    }

}
