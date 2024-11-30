using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogView : EntityView{

    [SerializeField] private LineRenderer lr;
    public SoundEffect interactSFX;

    private float t = 0;

    // Trigger tongue animation
    public void PlayTongueAnimation(List<Vector3> tonguePath, float dur, Action onCompleteCallBack = null) {
        StartCoroutine(TongueAnim(tonguePath, dur, onCompleteCallBack));
    }

    private IEnumerator TongueAnim(List<Vector3> tonguePath, float segmentDuration, Action onCompleteCallBack = null) {
        // Extent tongue anim

        int pointsCount = tonguePath.Count;

        // Setup line renderer points
        lr.gameObject.SetActive(true);
        lr.positionCount = pointsCount;
        for (int i = 0; i < pointsCount; i++) {
            lr.SetPosition(i, tonguePath[0]);
        }

        // for each loop, it's lerping all points that didn't reach desired tongue path point
        for (int i = 0; i < pointsCount - 1; i++) {
            float startTime = Time.time;


            Vector3 startPosition = tonguePath[i];
            Vector3 endPosition = tonguePath[i + 1];
            Vector3 pos = startPosition;



            while (pos != endPosition) {

                t = segmentDuration == 0 ? 1 : ((Time.time - startTime) / segmentDuration);
                pos = Vector3.Lerp(startPosition, endPosition, t);

                // Animate all other points except points before index i
                // Points before index i are reached the desried point
                for (int j = i + 1; j < pointsCount; j++) {
                    lr.SetPosition(j, pos);
                }

                yield return null;
            }

        }



        yield return new WaitForSecondsRealtime(segmentDuration / 2);


        // Retract tongue anim

        // for each loop, it's lerping all points that didn't reach desired tongue path point
        for (int i = pointsCount - 1; i >= 1; i--) {
            float startTime = Time.time;

            Vector3 startPosition = tonguePath[i - 1];
            Vector3 endPosition = tonguePath[i];
            Vector3 pos = endPosition;



            while (pos != startPosition) {
                float t = (Time.time - startTime) / segmentDuration;
                pos = Vector3.Lerp(endPosition, startPosition, t);

                // Animate all other points except points before index i
                // Points before index i are reached the desried point
                for (int j = i; j < pointsCount; j++) {
                    lr.SetPosition(j, pos);
                }
                yield return null;
            }

        }

        // Trigger on complete event 
        if (onCompleteCallBack != null)
            onCompleteCallBack();

        lr.gameObject.SetActive(false);
    }
}
