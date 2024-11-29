using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrapeView : EntityView{

    [SerializeField] private SoundEffect eatenSFX;

    protected override void OnEnable() {
        base.OnEnable();
    }

    protected override void OnDisable() {
        base.OnDisable();
    }

    public void PlayRetractAnim(List<Vector3> path, float dur, float delay = 0f, Action onCompleteCallBack = null) {
        if (path.Count == 0) return;

        StartCoroutine(RetractAnim(path, dur, delay, onCompleteCallBack));
    }


    private IEnumerator RetractAnim(List<Vector3> path, float segmentDuration, float delay = 0f, Action onCompleteCallBack = null) {
        yield return new WaitForSecondsRealtime(delay);
        
        path.Add(transform.position); // add origin position

        int pointsCount = path.Count;

        // Retract tongue anim

        for (int i = pointsCount - 1; i > 0; i--) {
            // Check if grape is about to enter frog's mouth
            if(i == 1) {
                AnimateScale(Vector3.zero, Game.tongueMoveDur, Game.tongueMoveDur/3, () => gameObject.SetActive(true), eatenSFX);
            }

            float startTime = Time.time;

            Vector3 startPosition = path[i];
            Vector3 endPosition = path[i-1];
            Vector3 pos = startPosition;

            while (pos != endPosition) {
                float t = (Time.time - startTime) / segmentDuration;
                pos = Vector3.Lerp(startPosition, endPosition, t);

                transform.position = pos;
                yield return null;
            }
        }
        // Trigger on complete event
        if (onCompleteCallBack != null)
            onCompleteCallBack();
    }
}
