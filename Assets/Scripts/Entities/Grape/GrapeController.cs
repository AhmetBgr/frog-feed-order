using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeController : EntityController
{
    [SerializeField] private GrapeModal modal;
    [SerializeField] private GrapeView view;

    private const float retractDelayFactor = 0.3f;


    protected override void OnEnable() {
        base.OnEnable();
        FrogController.OnSuccessfullEat += HandleRecractAnim;
    }

    protected override void OnDisable() {
        base.OnDisable();

        FrogController.OnSuccessfullEat -= HandleRecractAnim;
    }

    public override void OnSpawn() {
        base.OnSpawn();


        // We need to seperate grapes from cell parent beacuse,
        // Cell has scale animation which ruins retract animation
        transform.SetParent(null);
    }

    public override IEnumerator TriggerOnExpire(float delay = 0) {
        yield return base.TriggerOnExpire(delay);

        //transform.SetParent(cell.transform);
    }

    private void HandleRecractAnim(List<Vector2Int> tonguePathCoord, List<Vector3> tonguePath) {
        if (tonguePathCoord.Contains(modal.coord)) {
            int index = tonguePathCoord.IndexOf(modal.coord);
            List<Vector3> path = new List<Vector3>();
            path.AddRange(tonguePath.GetRange(0, index )); 

            float retractDelay = CalculateRecractDelay(tonguePath.Count, index);
            view.PlayRetractAnim(path, Game.tongueMoveDur, retractDelay, () => transform.SetParent(cell.transform));  //, () => transform.SetParent(cell.transform)

            StartCoroutine(TriggerOnExpire(retractDelay + (Game.tongueMoveDur * (tonguePath.Count - index - 1))));
        }
    }

    public float CalculateRecractDelay(int tonguePathCount, int index) {
        float baseDelay = Game.tongueMoveDur * (tonguePathCount - 1 + tonguePathCount - index);
        float reducedDelay = Game.tongueMoveDur * retractDelayFactor * (tonguePathCount - index - 1);
        return baseDelay - reducedDelay + (Game.tongueMoveDur / 3);
    }
}
