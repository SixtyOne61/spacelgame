using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

public class EntSpawnerTest : SpacelEntity {

    public GameObject Prefab;
    private float _timer = 2.5f;

    public override void Update()
    {
        base.Update();

        _timer -= Time.deltaTime;

        if (_timer < 0.0f)
        {
            _timer = 2.5f;
            GameObject.Instantiate(Prefab, transform.position, transform.rotation, transform.parent);
        }
    }
}
