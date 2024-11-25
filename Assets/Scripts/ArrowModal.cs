using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowModal : EntityModal
{
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
    }

}
