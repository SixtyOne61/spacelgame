
public class CompCollisionBullet : CompCollision
{
    public override void Start()
    {
        base.Start();
        CollisionManager.Instance.Register(this);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        CollisionManager.Instance.UnRegister(this);
    }
}
