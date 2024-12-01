using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour{
    public EntityModal modal;
    public EntityView view;

    public Cell cell;

    public event Action OnExpire;

    protected virtual void OnEnable() {
        FrogController.OnTongueMove += PlayPunchScaleAnim;

        PlaySpawnAnim();
    }

    protected virtual void OnDisable() {
        FrogController.OnTongueMove -= PlayPunchScaleAnim;
    }

    protected virtual void Awake() {
        if (view == null)
            view = GetComponent<EntityView>();

        if (modal == null)
            modal = GetComponent<EntityModal>();

        modal.SetInitPos(transform.localPosition);
    }

    // Reset state when the entity is spawned
    public virtual void OnSpawn() {
        transform.SetParent(transform);

        transform.localScale = Vector3.one;
        transform.localPosition = new Vector3(0f, transform.localPosition.y, 0f);

        gameObject.SetActive(true);

        modal.SetIsExpired(false);

        transform.localPosition = modal.initPos;

        modal.SetCoord(Utils.PosToCoord(transform.position));
    }

    public virtual IEnumerator TriggerOnExpire(float delay = 0) {

        yield return new WaitForSecondsRealtime(delay);

        OnExpire?.Invoke();
    }

    private void PlaySpawnAnim() {
        // Setup scale anim
        transform.localScale = Vector3.zero;
        view.AnimateScale(Vector3.one, 0.5f);
    }

    private void PlayPunchScaleAnim(List<Vector2Int> tonguePathCoord, EntityColor targetColor) {
        if (targetColor != modal.color) return; // Exit if the colors don't match

        // Check if the entity's coordination is in the tongue path
        if (tonguePathCoord.Contains(modal.coord)) {
            float delay = Game.unitMoveDur * tonguePathCoord.IndexOf(modal.coord);
            view.AnimatePunchScale(Vector3.one * 0.5f, Game.unitMoveDur, delay, view.punchScaleSFX);
        }
    }

    protected virtual void UpdateDirection() {
        // Get the correct transform to use for direction calculation
        Transform transformToGetRot = transform.parent ? transform.parent : transform;

        // Update direction
        switch (transformToGetRot.rotation.eulerAngles.y) {
            case 0f:
            modal.SetDirection(Vector2Int.down);
            break;
            case 90f:
            modal.SetDirection(Vector2Int.left);
            break;
            case 180f:
            modal.SetDirection(Vector2Int.up);
            break;
            case 270f:
            modal.SetDirection(Vector2Int.right);
            break;
        }
    }

}
