using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogModal : EntityModal
{
    //public Vector2Int dir;
    public static event Action onFrogExpire;

    protected override void Start() {
        base.Start();

        // Update direction
        switch (transform.rotation.eulerAngles.y) {
            case 0f:
            dir = Vector2Int.down;
            break;
            case 90f:
            dir = Vector2Int.left;
            break;
            case 180f:
            dir = Vector2Int.up;
            break;
            case 270f:
            dir = Vector2Int.right;
            break;
        }

        GameManager.instance.AddToFrogsPool(this);
    }

    public override IEnumerator TriggerOnExpire(float delay = 0) {
        yield return base.TriggerOnExpire(delay);

        onFrogExpire?.Invoke();
    }

}
