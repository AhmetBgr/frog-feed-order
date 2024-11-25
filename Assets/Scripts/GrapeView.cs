using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrapeView : EntityView
{
    protected override void OnEnable() {
        base.OnEnable();
    }

    protected override void OnDisable() {
        base.OnDisable();
    }

    public void PlayRetractAnim(List<Vector3> path, float dur, float delay = 0f) {
        if (path.Count == 0) return;

        path.Reverse();

        //transform.DOScale(0f, 0.2f).SetDelay(delay);
        transform.DOPath(path.ToArray(), dur).SetDelay(delay).SetEase(Ease.Linear);
    }

}
