using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionManager : Singleton<CollisionManager>
{
    private CollisionGroup<CompCollisionPlayer> _playerGroup = new CollisionGroup<CompCollisionPlayer>();
    private CollisionGroup<CompCollisionWorld> _worldGroup = new CollisionGroup<CompCollisionWorld>();
    private CollisionGroup<CompCollisionBullet> _bulletGroup = new CollisionGroup<CompCollisionBullet>();
    private CollisionGroup<CompShield> _shieldGroup = new CollisionGroup<CompShield>();

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

    public void Register(CompCollisionPlayer component)
    {
        if(CanRegister())
        {
            _playerGroup.Add(component);
        }
    }

    public void Register(CompCollisionWorld component)
    {
        if (CanRegister())
        {
            _worldGroup.Add(component);
        }
    }

    public void Register(CompCollisionBullet component)
    {
        if (CanRegister())
        {
            _bulletGroup.Add(component);
        }
    }

    public void Register(CompShield component)
    {
        if (CanRegister())
        {
            _shieldGroup.Add(component);
        }
    }

    #endregion

    #region UnRegister

    public void UnRegister(CompCollisionPlayer component)
    {
        if(CanRegister())
        {
            _playerGroup.Remove(component);
        }
    }

    public void UnRegister(CompCollisionWorld component)
    {
        if (CanRegister())
        {
            _worldGroup.Remove(component);
        }
    }

    public void UnRegister(CompCollisionBullet component)
    {
        if (CanRegister())
        {
            _bulletGroup.Remove(component);
        }
    }

    public void UnRegister(CompShield component)
    {
        if (CanRegister())
        {
            _shieldGroup.Remove(component);
        }
    }

    #endregion

    public void FixedUpdate()
    {
        PlayerToWorld();
        ShieldToBullet();
        BulletToOther();
    }

    private void PlayerToWorld()
    {
        foreach (CompCollisionPlayer playerCollision in _playerGroup.Components)
        {
            if (!playerCollision.Owner.activeSelf)
            {
                continue;
            }

            foreach (CompCollisionWorld worldCollision in _worldGroup.Components)
            {
                if (!worldCollision.Owner.activeSelf)
                {
                    continue;
                }

                // manage hit if we have
                playerCollision.Hit(worldCollision);
            }
        }
    }

    private void ShieldToBullet()
    {
        foreach (CompShield shield in _shieldGroup.Components)
        {
            if (!shield.Owner.activeSelf)
            {
                continue;
            }

            foreach (CompCollisionBullet bulletCollision in _bulletGroup.Components)
            {
                if (!bulletCollision.Owner.activeSelf)
                {
                    continue;
                }

                // manage hit if we have
                shield.Hit(bulletCollision);
            }
        }
    }

    private void BulletToOther()
    {
        foreach (CompCollisionBullet bulletCollision in _bulletGroup.Components)
        {
            if (!bulletCollision.Owner.activeSelf)
            {
                continue;
            }

            foreach (CompCollisionWorld worldCollision in _worldGroup.Components)
            {
                if (!worldCollision.Owner.activeSelf)
                {
                    continue;
                }

                // manage hit if we have
                bulletCollision.Hit(worldCollision);
            }

            foreach (CompCollisionPlayer playerCollision in _playerGroup.Components)
            {
                if (!playerCollision.Owner.activeSelf)
                {
                    continue;
                }

                // manage hit if we have
                bulletCollision.Hit(playerCollision);
            }
        }
    }
}
