using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeModal : EntityModal{
    public override void OnSpawn() {
        base.OnSpawn();


        // We need to seperate grapes from cell parent beacuse,
        // Cell has scale animation which ruins retract animation
        transform.SetParent(null);
    }
}
