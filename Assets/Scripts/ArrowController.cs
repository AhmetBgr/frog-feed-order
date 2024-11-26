using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public ArrowModal modal;
    public EntityView view;


    protected void OnEnable() {
        FrogController.OnSuccessfullEat += HandleExpiration;
    }

    protected void OnDisable() {
        FrogController.OnSuccessfullEat -= HandleExpiration;
    }

    private void HandleExpiration(List<Vector2Int> tonguePathCoord, List<Vector3> tonguePath) {
        if (tonguePathCoord.Contains(modal.coord)) {
            int index = tonguePathCoord.IndexOf(modal.coord);
            List<Vector3> path = new List<Vector3>();
            path.AddRange(tonguePath.GetRange(0, index));

            float retractDelay = CalculateRecractDelay(tonguePath.Count, index);
            view.AnimateScale(Vector3.zero, Game.tongueMoveDur, (tonguePath.Count - 1) * Game.tongueMoveDur * 1.8f + (index * 0.2f * Game.tongueMoveDur), () => gameObject.SetActive(false)); //(tonguePath.Count - index + tonguePath.Count) * Game.tongueMoveDur

            StartCoroutine(modal.TriggerOnExpire(retractDelay + Game.tongueMoveDur / 2));
        }
    }

    public float CalculateRecractDelay(int tonguePathCount, int index) {
        return Game.tongueMoveDur * (tonguePathCount - 1) + ((tonguePathCount - index) * Game.tongueMoveDur * 0.75f);
    }
}
