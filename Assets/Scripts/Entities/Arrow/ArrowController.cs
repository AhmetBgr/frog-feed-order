using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : EntityController
{
    public ArrowModal modal;
    public EntityView view;

    private const float retractDelayFactor = 0.3f;

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
            List<Vector3> path = new List<Vector3>();
            path.AddRange(tonguePath.GetRange(0, index));

            float retractDelay = CalculateRecractDelay(tonguePath.Count, index);
            float scaleDelay = (tonguePath.Count - 1) * Game.tongueMoveDur * 1.8f + (index * 0.2f * Game.tongueMoveDur);
            ///float scaleDelay = retractDelay;

            //view.AnimateScale(Vector3.zero, Game.tongueMoveDur, scaleDelay, () => { gameObject.SetActive(false); }); 
            ///(tonguePath.Count - index + tonguePath.Count) * Game.tongueMoveDur

            StartCoroutine(TriggerOnExpire(retractDelay + Game.tongueMoveDur));
            /// retractDelay + (Game.tongueMoveDur * (tonguePath.Count - index - 1))
            /// retractDelay + Game.tongueMoveDur / 2)
            ///(Game.tongueMoveDur * (tonguePath.Count - index - 1))

        }
    }

    /*public float CalculateRecractDelay(int tonguePathCount, int index) {
        return Game.tongueMoveDur * (tonguePathCount - 1) + ((tonguePathCount - index) * Game.tongueMoveDur * 0.75f);
    }*/

    public float CalculateRecractDelay(int tonguePathCount, int index) {
        float baseDelay = Game.tongueMoveDur * (tonguePathCount - 1);
        float retractDelay = Game.tongueMoveDur * (tonguePathCount - index - 1);
        //float reducedDelay = Game.tongueMoveDur * (tonguePathCount - index - 1);
        return baseDelay + retractDelay + (Game.tongueMoveDur /2);
    }
}
