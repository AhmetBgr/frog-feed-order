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
    public Vector2Int coord;
    public EntityType type;
    public EntityColor color;
    public EntityView view;
    public Vector2Int dir;
    public Vector3 initPos;
    public bool isExpired = false;


    public void SetInitPos(Vector3 newPos) {
        initPos = newPos;
    }

    public void SetDirection(Vector2Int newDir) {
        dir = newDir;
    }

    public void SetIsExpired(bool value) {
        isExpired = value;
    }

    public void SetCoord(Vector2Int newCoord) {
        coord = newCoord;
    }

}
