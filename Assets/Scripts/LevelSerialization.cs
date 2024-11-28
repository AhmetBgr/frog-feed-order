using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SerializedLevel {
    //public List<SerializedLevelObject> LevelObjects;

    public List<SerializedNodeObject> nodeObjects;
    public int movesCount;

    public SerializedLevel(GameObject level, int movesCount) {
        nodeObjects = new List<SerializedNodeObject>();

        foreach (Transform node in level.transform) {
            SerializedNodeObject serializedNodeObject = new SerializedNodeObject(node);

            foreach (Transform cell in node) {

                if (cell.name == "Base Cell") continue;

                cell.name = cell.name.Replace("(Clone)","");

                serializedNodeObject.cellObjects.Add(new SerializedCellObject(cell));
            }

            nodeObjects.Add(serializedNodeObject);
        }
        this.movesCount = movesCount;

        /*foreach (Transform child in level.transform) {
            LevelObjects.Add(new SerializedLevelObject(child));
        }*/
    }
}

[System.Serializable]
public class SerializedNodeObject {
    public List<SerializedCellObject> cellObjects;

    public Vector3 pos;

    public SerializedNodeObject(Transform t) {
        cellObjects = new List<SerializedCellObject>();
        pos = t.localPosition;
    }
}

[System.Serializable]
public class SerializedCellObject {
    public string prefab;
    public Vector3 pos;
    public Vector3 angles;
    public SerializedCellObject(Transform t) {
        prefab = t.name;
        pos = t.localPosition;
        angles = t.localEulerAngles;
    }
}