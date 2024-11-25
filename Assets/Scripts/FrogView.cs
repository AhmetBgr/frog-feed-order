using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogView : EntityView
{
    public LineRenderer lr;
    //public List<Vector3> tonguePath = new List<Vector3>();
    private float t = 0;

    // Trigger tongue animation
    public void PlayTongueAnimation(List<Vector3> tonguePath, float dur, Action onCompleteCallBack = null) {
        StartCoroutine(TongueAnim(tonguePath, dur, onCompleteCallBack));
    }

    private IEnumerator TongueAnim(List<Vector3> tonguePath, float segmentDuration, Action onCompleteCallBack = null) {
        // Extent tongue anim
        lr.enabled = true;
        int pointsCount = tonguePath.Count;
        lr.positionCount = pointsCount;

        for (int i = 0; i < pointsCount; i++) {
            lr.SetPosition(i, tonguePath[0]);
        }

        int piece = (1 * pointsCount - 1);
        //float segmentDuration = dur / piece;

        for (int i = 0; i < pointsCount - 1; i++) {
            float startTime = Time.time;

            Vector3 startPosition = tonguePath[i];
            Vector3 endPosition = tonguePath[i + 1];
            Vector3 pos = startPosition;
            while (pos != endPosition) {

                t = segmentDuration == 0 ? 1 : ((Time.time - startTime) / segmentDuration);
                pos = Vector3.Lerp(startPosition, endPosition, t);

                // animate all other points except point at index i
                for (int j = i + 1; j < pointsCount; j++) {
                    lr.SetPosition(j, pos);
                }
                yield return null;
            }
        }


        yield return new WaitForSeconds(segmentDuration * Time.deltaTime * 20f);

        // Retract tongue anim

        float totalLenght = 0f;
        for (int i = pointsCount - 1; i > 0; i--) {
            totalLenght += (tonguePath[i] - tonguePath[i - 1]).magnitude;
        }

        for (int i = pointsCount - 1; i >= 1; i--) {
            float startTime = Time.time;

            Vector3 startPosition = tonguePath[i - 1];
            Vector3 endPosition = tonguePath[i];
            Vector3 pos = endPosition;

            float pieceLength = (endPosition - startPosition).magnitude;
            //segmentDuration = dur * (pieceLength / totalLenght);

            while (pos != startPosition) {
                float t = (Time.time - startTime) / segmentDuration;
                pos = Vector3.Lerp(endPosition, startPosition, t);

                // animate all other points except point at index i
                for (int j = i; j < pointsCount; j++) {
                    lr.SetPosition(j, pos);
                }
                yield return null;
            }
        }

        if (onCompleteCallBack != null)
            onCompleteCallBack();
    }

    public void DeactivateFrog() {
        // Hide the frog or play deactivation animation

    }
}
