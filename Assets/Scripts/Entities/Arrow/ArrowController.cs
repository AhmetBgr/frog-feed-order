using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : EntityController{
    protected override void OnEnable() {
        base.OnEnable();
        FrogController.OnSuccessfullEat += HandleExpiration;
    }

    protected override void OnDisable() {
        base.OnDisable();
        FrogController.OnSuccessfullEat -= HandleExpiration;
    }

    public override void OnSpawn() {
        base.OnSpawn();

        UpdateDirection();
    }

    private void HandleExpiration(List<Vector2Int> tonguePathCoord, List<Vector3> tonguePath) {
        if (tonguePathCoord.Contains(modal.coord)) {
            int index = tonguePathCoord.IndexOf(modal.coord);

            float expireDelay = CalculateExpireDelay(tonguePath.Count, index);

            StartCoroutine(TriggerOnExpire(expireDelay));
        }
    }

    public float CalculateExpireDelay(int tonguePathCount, int index) {
        float baseDelay = Game.unitMoveDur * (tonguePathCount - 1);
        float retractDelay = Game.unitMoveDur * (tonguePathCount - index);
        return baseDelay + retractDelay + (Game.unitMoveDur / 2);
    }
}
