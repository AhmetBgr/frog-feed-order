using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType {
    None,
    Frog,
    Grape,
    Arrow
}
public enum EntityColor {
    Blue,
    Purple,
    Green,
    Red,
    Yellow
}

public class EntityModal : MonoBehaviour{
    public GameObject prefab;
    public Vector2Int coord;
    public EntityType type;
    public EntityColor color;
    public EntityView view;
    public Vector2Int dir;
    public bool isExpired = false;

    public event Action OnExpire;

    protected virtual void Start() {
        // Find coord
        coord = Utils.PosToCoord(transform.position);
    }

    public virtual IEnumerator TriggerOnExpire(float delay = 0) {

        yield return new WaitForSeconds(delay);

        OnExpire?.Invoke();
    }

}
