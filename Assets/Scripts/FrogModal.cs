using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogModal : EntityModal
{
    public Vector2Int dir;

    protected override void Start() {
        base.Start();

        // Find direction
        dir = Vector2Int.down;
    }

}
