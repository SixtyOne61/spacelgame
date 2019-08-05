using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Engine;
using Tool;

[System.Serializable]
public class CompShooter : ComponentTrigger
{
    public Tool.SCROneValue ParamBetweenBullet;
    public Tool.SCROneValue ParamBetweenShoot;

    // list of bullet spawner, assume we have only 2
    private List<Transform> _bulletSpawners = new List<Transform>();
    // list of timer manager attach on bullet spawner
    private List<TriggerValue> _timerBulletTrigger = new List<TriggerValue>();
    // timer between shoot
    private TriggerValue _timerShoot;

    public override void Start()
    {
        foreach (Transform child in Owner.transform)
        {
            if (child.CompareTag("BulletSpawner"))
            {
                _bulletSpawners.Add(child);
                _timerBulletTrigger.Add(new TriggerValue(ParamBetweenBullet.Value));
                AddTrigger(_timerBulletTrigger.Last());
            }
        }

        // init timer between shoot
        _timerShoot = new TriggerValue(ParamBetweenShoot.Value);

        AddTrigger(_timerShoot);
        base.Start();
    }

    public override void Update()
    {
        float primaryShootTrigger = Input.GetAxis("PrimaryShoot");

        if (primaryShootTrigger >= 1.0f && _timerShoot.IsAvailable)
        {
            // looking for an available bullet spawner
            if (Fire())
            {
                _timerShoot.Start();
            }
        }

        // update all trigger
        foreach (TriggerValue trigger in _timerBulletTrigger)
        {
            trigger.Update();
        }

        _timerShoot.Update();

        base.Update();
    }

    private bool Fire()
    {
        for (int i = 0; i < _timerBulletTrigger.Count; ++i)
        {
            if (_timerBulletTrigger[i].IsAvailable)
            {
                _timerBulletTrigger[i].Start();
                GameObject go = Builder.Instance.Build(Builder.FactoryType.Gameplay, (int)Tool.BuilderGameplay.Type.Bullet, _bulletSpawners[i].position, _bulletSpawners[i].rotation, Owner.transform.parent);
                //Physics.IgnoreCollision(go.GetComponent<Collider>(), Owner.GetComponent<Collider>());
                return true;
            }
        }

        return false;
    }
}
