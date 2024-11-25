using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EntityView : MonoBehaviour
{
    public EntityModal modal;

    protected virtual void OnEnable() {

        FrogController.OnTongueMove += HandleAnimateEntity;
    }

    protected virtual void OnDisable() {
        FrogController.OnTongueMove -= HandleAnimateEntity;

    }


    // Handle the event when it's triggered
    private void HandleAnimateEntity(List<Vector2Int> tonguePathCoord) {
        if (tonguePathCoord.Contains(modal.coord)) {
            AnimatePunchScale(Vector3.one * 0.5f, Game.tongueMoveDur, Game.tongueMoveDur * tonguePathCoord.IndexOf(modal.coord));
        }
    }

    // Default animation (can be overridden by subclasses)
    public virtual void AnimatePunchScale(Vector3 scale, float duration, float delay = 0f) {
        transform.DOPunchScale(scale, duration, vibrato:1).SetDelay(delay);
    }
}
