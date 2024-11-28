using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrapeView : EntityView
{
    public SoundEffect eatenSFX;

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
        //transform.DOPath(path.ToArray(), dur).SetDelay(delay).SetEase(Ease.Linear);
        StartCoroutine(RetractAnim(path, dur, delay));
    }


    private IEnumerator RetractAnim(List<Vector3> path, float segmentDuration, float delay = 0f) {
        yield return new WaitForSecondsRealtime(delay);
        path.Reverse();
        path.Add(transform.position);

        int pointsCount = path.Count;

        // Retract tongue anim

        float totalLenght = 0f;
        for (int i = pointsCount - 1; i > 0; i--) {
            totalLenght += (path[i] - path[i - 1]).magnitude;
        }
        //Vector3 startPosition = transform.position;

        for (int i = pointsCount - 1; i > 0; i--) {
            if(i == 1) {
                AnimateScale(Vector3.zero, Game.tongueMoveDur, Game.tongueMoveDur/3, () => gameObject.SetActive(false), eatenSFX);
            }

            float startTime = Time.time;

            Vector3 startPosition = path[i];
            Vector3 endPosition = path[i-1];
            Vector3 pos = startPosition;

            float pieceLength = (endPosition - startPosition).magnitude;
            //segmentDuration = dur * (pieceLength / totalLenght);

            while (pos != endPosition) {
                float t = (Time.time - startTime) / segmentDuration;
                pos = Vector3.Lerp(startPosition, endPosition, t);

                transform.position = pos;


                // animate all other points except point at index i
                for (int j = i; j < pointsCount; j++) {
                
                }
                yield return null;
            }
            //startPosition = endPosition;
        }
        //AnimateScale(Vector3.zero, Game.tongueMoveDur, 0f, () => gameObject.SetActive(false), eatenSFX);

        /*if (onCompleteCallBack != null)
            onCompleteCallBack();

        lr.gameObject.SetActive(false);*/
    }
}
