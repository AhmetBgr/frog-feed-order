using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    public EntityModal entityModal;

    public Cell cell;

    public event Action OnExpire;

    protected virtual void OnEnable() {
        //GameManager.instance.onGameOver += TriggerOnExpireImmidieatly;
    }

    protected virtual void OnDisable() {
        //GameManager.instance.onGameOver -= TriggerOnExpireImmidieatly;

    }



    public virtual IEnumerator TriggerOnExpire(float delay = 0) {

        yield return new WaitForSecondsRealtime(delay);

        OnExpire?.Invoke();
    }

    public virtual void OnSpawn() {
        entityModal.isExpired = false;
        transform.localPosition = entityModal.initPos;
        entityModal.coord = Utils.PosToCoord(transform.position);
    }

    protected virtual void UpdateDirection() {
        Transform transformToGetRot = transform.parent ? transform.parent : transform;

        // Update direction
        switch (transformToGetRot.rotation.eulerAngles.y) {
            case 0f:
            entityModal.dir = Vector2Int.down;
            break;
            case 90f:
            entityModal.dir = Vector2Int.left;
            break;
            case 180f:
            entityModal.dir = Vector2Int.up;
            break;
            case 270f:
            entityModal.dir = Vector2Int.right;
            break;
        }
    }

}
