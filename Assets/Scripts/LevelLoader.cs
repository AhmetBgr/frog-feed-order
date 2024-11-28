using System.Collections.Generic;
using UnityEngine;

public class LevelLoader {
    static int levelIndex = 0;
    static List<string> allLevels => Utils.allLevels;

    public static SerializedLevel LoadLevel(string levelName) {
        SetLevelIndexByName(levelName);
        TextAsset textFile = Resources.Load<TextAsset>("Levels/" + levelName);
        return JsonUtility.FromJson<SerializedLevel>(textFile.text);
    }

    public static void InstantiateCells(SerializedNodeObject serializedNodeObject, GameObject[] prefabs, Transform node) {

        foreach (var serializedCellObject in serializedNodeObject.cellObjects) {
            foreach (GameObject prefab in prefabs) {
                if (prefab.transform.name == serializedCellObject.prefab) {
                    var go = GameObject.Instantiate(prefab) as GameObject;
                    go.transform.parent = node;
                    go.transform.localPosition = serializedCellObject.pos;
                    go.transform.localEulerAngles = serializedCellObject.angles;
                }
            }
        }
    }

    public static SerializedLevel LoadNextLevel() {
        levelIndex++;
        if (levelIndex >= allLevels.Count) {
            levelIndex = 0;
        }
        if (allLevels[levelIndex].Contains("test")) {
            return LoadNextLevel();
        }
        else {
            return LoadLevel(allLevels[levelIndex]);
        }
    }

    static void SetLevelIndexByName(string levelName) {
        for (int i = 0; i < allLevels.Count; i++) {
            if (allLevels[i] == levelName) {
                levelIndex = i;
            }
        }
    }
}