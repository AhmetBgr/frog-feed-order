using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour{
    public GridManager gridManager;

    public SerializedLevel curSerializedLevel;

    public List<TextAsset> levels;

    private ObjectPooler objectPooler;

    public int currentLevelIndex;

    public event Action<int> OnLeveload;

    public static LevelManager instance { get; private set; }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject); // Destroy duplicate
            return;
        }

        instance = this;
        //DontDestroyOnLoad(gameObject); // Make the instance persistent
    }

    void Start(){
        gridManager.DestroyAllNodes();

        currentLevelIndex = 0;
        LoadLevel(gridManager.transform, levels[currentLevelIndex]);
    }

    public void ReloadCurLevel() {
        LoadLevel(gridManager.transform, levels[currentLevelIndex]);
    }

    public void LoadNextLevel() {
        currentLevelIndex++;

        if (currentLevelIndex >= levels.Count) return;

        LoadLevel(gridManager.transform, levels[currentLevelIndex]);
    }

    public void LoadLevel(Transform levelParent, TextAsset textFile) {
        gridManager.PopulateNodesGrid();

        SerializedLevel serializedLevel = SerializeLevel(textFile);

        // Spawns cells for each node in the grid if node has any.
        for (int i = 0; i < levelParent.childCount; i++) {

            Node node = levelParent.GetChild(i).GetComponent<Node>();
            GenerateCells(serializedLevel.nodeObjects[i], gridManager.cellPrefabs, node.transform);

            node.UpdateTopCell();
        }

        curSerializedLevel = serializedLevel;

        OnLeveload?.Invoke(currentLevelIndex);
    }

    private SerializedLevel SerializeLevel(TextAsset textFile) {
        return JsonUtility.FromJson<SerializedLevel>(textFile.text);
    }

    public static TextAsset LoadLevelTextFile(string levelName) {
        TextAsset textFile = Resources.Load<TextAsset>("Levels/" + levelName);
        return textFile;
    }

    private void GenerateCells(SerializedNodeObject serializedNodeObject, GameObject[] prefabs, Transform node) {
        if(objectPooler == null)
            objectPooler = FindObjectOfType<ObjectPooler>();

        // Loops through all cells in serialized node data
        foreach (var serializedCellObject in serializedNodeObject.cellObjects) {
            GameObject go = null;
            if(Application.isPlaying) // Gets cell object from the pool when playing thegame
                go = objectPooler.SpawnFromPool(serializedCellObject.prefab.Replace("(Clone)", ""));
            else { 
                // Instantiates new cell object. This is for when using level editor in scene view
                foreach (GameObject prefab in prefabs) {
                     if (prefab.transform.name == serializedCellObject.prefab) {
                        go = GameObject.Instantiate(prefab) as GameObject;
                        break;
                     }
                }
            }

            if (go == null)
                Debug.LogError("couldn't instantiate an object.");

            go.transform.parent = node;
            go.transform.localPosition = serializedCellObject.pos;
            go.transform.localEulerAngles = serializedCellObject.angles;

            // Calls the OnObjectSpawn method if applicable
            IPoolableObject poolable = go.GetComponent<IPoolableObject>();
            if (poolable != null && Application.isPlaying) {
                poolable.OnObjectSpawn();
            }
        }
    }
}
