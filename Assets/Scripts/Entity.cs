using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType {
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

public class Entity : MonoBehaviour
{
    public GameObject prefab;
    public Vector2Int coord;
    public EntityType type;
    public EntityColor color;

}
