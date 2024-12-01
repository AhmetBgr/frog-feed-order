using System;
using UnityEngine;
using DG.Tweening;

public class EntityView : MonoBehaviour
{
    public SoundEffect punchScaleSFX;
    public SoundEffect entityDenySFX;

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
