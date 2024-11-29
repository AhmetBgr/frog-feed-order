using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogModal : DirectionalEntityModal { 

    // we need seperate event from the EntityModal Class as well because 
    // GameManager needs to track it specificly for frogs
    public static event Action onFrogExpire;

    public override IEnumerator TriggerOnExpire(float delay = 0) {
        yield return base.TriggerOnExpire(delay);

        onFrogExpire?.Invoke();
    }

    public override void OnSpawn() {
        base.OnSpawn();

        Start();

        GameManager.instance.AddToFrogsPool(this);
    }
}