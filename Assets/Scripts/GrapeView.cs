using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrapeView : EntityView
{
    protected override void OnEnable() {
        base.OnEnable();
        FrogController.OnSuccessfullEat += HandleRecractAnim;
    }

    protected override void OnDisable() {
        base.OnDisable();
        FrogController.OnSuccessfullEat -= HandleRecractAnim;
    }

    public void PlayScaleAnim(Vector3 endValue, float dur, float delay = 0f) {
        transform.DOScale(endValue, dur).SetDelay(delay);//.SetEase(Ease.InCubic);
    }

    private void HandleRecractAnim(List<Vector2Int> tonguePathCoord, List<Vector3> tonguePath) {
        if (tonguePathCoord.Contains(modal.coord)) {
            int index = tonguePathCoord.IndexOf(modal.coord);
            List<Vector3> path = new List<Vector3>();
            path.AddRange(tonguePath.GetRange(0, index ));
            PlayRetractAnim(path, Game.tongueMoveDur * path.Count, CalculateRecractDelay(tonguePath.Count, index));
            PlayScaleAnim(Vector3.zero, Game.tongueMoveDur,  (tonguePath.Count - 1)* Game.tongueMoveDur * 1.8f  + (index * 0.2f * Game.tongueMoveDur)); //(tonguePath.Count - index + tonguePath.Count) * Game.tongueMoveDur
        }
    }

    private float CalculateRecractDelay(int tonguePathCount, int index) {
        return Game.tongueMoveDur * (tonguePathCount - 1) + ((tonguePathCount - index) * Game.tongueMoveDur * 0.75f);
    }

    public void PlayRetractAnim(List<Vector3> path, float dur, float delay = 0f) {
        if (path.Count == 0) return;

        path.Reverse();

        //transform.DOScale(0f, 0.2f).SetDelay(delay);
        transform.DOPath(path.ToArray(), dur).SetDelay(delay).SetEase(Ease.Linear);
    }

}
