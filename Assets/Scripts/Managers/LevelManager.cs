using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;


public class LevelManager : MonoBehaviour{
    public GridManager gridManager;
    public GameManager gameManager;

    public SerializedLevel curSerializedLevel;


    public static int currentLevelIndex;
    static bool isLoading = false;
    [SerializeField] string levelToLoad;

    public static event Action<int> OnLeveload;

    public List<TextAsset> levels;


    public static LevelManager instance { get; private set; }

    /*public List<string> allLevels {
        get {
            if (allLevelsRef == null) {
                allLevelsRef = new List<string>();
                Object[] levels = Resources.LoadAll("Levels");
                foreach (Object t in levels) {
                    allLevelsRef.Add(t.name);
                }
            }
            return allLevelsRef;
        }
    }
    */


    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject); // Destroy duplicate
            return;
        }

        instance = this;
        //DontDestroyOnLoad(gameObject); // Make the instance persistent


    }

    // Start is called before the first frame update
    void Start()
    {
        currentLevelIndex = 0;
        LoadLevel(gridManager.transform, levels[currentLevelIndex]);
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextScene() {
        currentLevelIndex++;

        if (currentLevelIndex >= levels.Count) return;

        LoadLevel(gridManager.transform, levels[currentLevelIndex]);

        /*int index = SceneManager.GetActiveScene().buildIndex + 1;

        if (index >= SceneManager.sceneCountInBuildSettings) return;
            
        SceneManager.LoadScene(index);*/
    }

    public void LoadLevel(Transform levelParent, TextAsset textFile, bool clear = true) {

        /*if (isLoading || string.IsNullOrWhiteSpace(textFile.name)) {
            return;
        }
        */
        /*if (clear) {
            for (int i = transform.childCount - 1; i >= 0; i--) {
                Destroy(transform.GetChild(i).gameObject);
            }
        }*/

        gridManager.PopulateNodesGrid();
        //Debug.Log("level name trying to load: " + levelName);
        SerializedLevel serializedLevel = SerializeLevel(textFile);

        for (int i = 0; i < levelParent.childCount; i++) {

            Node node = levelParent.GetChild(i).GetComponent<Node>();
            InstantiateCells(serializedLevel.nodeObjects[i], gridManager.cellPrefabs, node.transform);

            node.UpdateTopCell();
        }
 
        //gameManager.movesCount = serializedLevel.movesCount;

        curSerializedLevel = serializedLevel;

        OnLeveload?.Invoke(currentLevelIndex);
    }


    static int levelIndex = 0;
    static List<string> allLevels => Utils.allLevels;



    public static SerializedLevel SerializeLevel(TextAsset textFile) {
        //SetLevelIndexByName(levelName);
        //TextAsset textFile = Resources.Load<TextAsset>("Levels/" + levelName);
        return JsonUtility.FromJson<SerializedLevel>(textFile.text);
    }

    public static TextAsset LoadLevelTextFile(string levelName) {
        SetLevelIndexByName(levelName);
        TextAsset textFile = Resources.Load<TextAsset>("Levels/" + levelName);
        return textFile;
    }

    public static void InstantiateCells(SerializedNodeObject serializedNodeObject, GameObject[] prefabs, Transform node) {
        ObjectPooler objectPooler = FindObjectOfType<ObjectPooler>();

        foreach (var serializedCellObject in serializedNodeObject.cellObjects) {

            GameObject go = null;
            if(Application.isPlaying)
                go = objectPooler.SpawnFromPool(serializedCellObject.prefab.Replace("(Clone)", ""));
            else {
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

            // Call the interface method if applicable
            IPoolableObject poolable = go.GetComponent<IPoolableObject>();
            if (poolable != null && Application.isPlaying) {
                poolable.OnObjectSpawn();
            }


            /*foreach (GameObject prefab in prefabs) {
                if (prefab.transform.name == serializedCellObject.prefab) {
                    var go = GameObject.Instantiate(prefab) as GameObject;

                    go.transform.parent = node;
                    go.transform.localPosition = serializedCellObject.pos;
                    go.transform.localEulerAngles = serializedCellObject.angles;

                }
            }*/
        }
    }

    /*public static SerializedLevel LoadNextLevel() {
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
    }*/

    static void SetLevelIndexByName(string levelName) {
        for (int i = 0; i < allLevels.Count; i++) {
            if (allLevels[i] == levelName) {
                levelIndex = i;
            }
        }
    }
}
