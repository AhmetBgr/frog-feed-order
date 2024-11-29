using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogModal : DirectionalEntityModal { 

    // Entity modal has similar event but,
    // we need seperate event for frog because 
    // game manager needs to track it specificly
    public static event Action onFrogExpire;

    public override IEnumerator TriggerOnExpire(float delay = 0) {
        yield return base.TriggerOnExpire(delay);

        onFrogExpire?.Invoke();
    }
}