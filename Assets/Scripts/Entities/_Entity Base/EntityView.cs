using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EntityView : MonoBehaviour
{
    public EntityModal modal;
    public SoundEffect punchScaleSFX;

    protected virtual void OnEnable() {

        FrogController.OnTongueMove += HandleAnimateEntity;
        transform.localScale = Vector3.zero;

        AnimateScale(Vector3.one, 0.5f);
    }

    protected virtual void OnDisable() {
        FrogController.OnTongueMove -= HandleAnimateEntity;
    }

    // Handle the event when it's triggered
    private void HandleAnimateEntity(List<Vector2Int> tonguePathCoord, EntityColor targetColor) {
        if (targetColor != modal.color) return;

        if (tonguePathCoord.Contains(modal.coord)) {
            AnimatePunchScale(Vector3.one * 0.5f, Game.tongueMoveDur, Game.tongueMoveDur * tonguePathCoord.IndexOf(modal.coord), punchScaleSFX);
        }
    }

    // Default animation (can be overridden by subclasses)
    public virtual void AnimatePunchScale(Vector3 scale, float duration, float delay = 0f, SoundEffect sound = null) {
        if(sound)
            AudioManager.instance.PlaySound(sound, delay);

        transform.DOPunchScale(scale, duration, vibrato:1).SetDelay(delay);
    }

    public virtual void AnimatePunchPos(Vector3 dir, float duration, float delay = 0f, SoundEffect sound = null) {
        if (sound)
            AudioManager.instance.PlaySound(sound, delay);

        transform.DOPunchPosition(dir, duration, vibrato: 10, elasticity: 10).SetDelay(delay);
    }

    public virtual void AnimateScale(Vector3 endValue, float duration, float delay = 0f, Action onComplete = null, SoundEffect sound = null) {
        if(sound)
            AudioManager.instance.PlaySound(sound, delay);

        transform.DOScale(endValue, duration).SetDelay(delay).OnComplete(() => onComplete?.Invoke());
    }
}
