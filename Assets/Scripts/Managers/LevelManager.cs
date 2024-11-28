using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour{
    public GridController gridManager;
    
    public static LevelManager instance { get; private set; }

    public static string currentLevelName = "";
    static bool isLoading = false;
    [SerializeField] string levelToLoad;

    public static event Action OnLeveload;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject); // Destroy duplicate
            return;
        }

        instance = this;
        //DontDestroyOnLoad(gameObject); // Make the instance persistent
        LoadLevel(gridManager.transform, levelToLoad);

    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextScene() {
        int index = SceneManager.GetActiveScene().buildIndex + 1;

        if (index >= SceneManager.sceneCountInBuildSettings) return;
            
        SceneManager.LoadScene(index);
    }

    public void LoadLevel(Transform levelParent, string levelName, bool clear = true) {

        if (isLoading || string.IsNullOrWhiteSpace(levelName)) {
            return;
        }

        /*if (clear) {
            for (int i = transform.childCount - 1; i >= 0; i--) {
                Destroy(transform.GetChild(i).gameObject);
            }
        }*/

        gridManager.PopulateNodesGrid();
        Debug.Log("level name trying to load: " + levelName);
        SerializedLevel serializedLevel = LevelLoader.LoadLevel(levelName);

        for (int i = 0; i < levelParent.childCount; i++) {
            currentLevelName = levelName;

            Node node = levelParent.GetChild(i).GetComponent<Node>();
            LevelLoader.InstantiateCells(serializedLevel.nodeObjects[i], gridManager.cellPrefabs, node.transform);

            node.UpdateTopCell();
        }

        foreach (Transform node in levelParent) {

        }

        OnLeveload?.Invoke();

        /*currentLevelName = levelName;
        SerializedLevel serializedLevel = LevelLoader.LoadLevel(currentLevelName);
        GameObject newLevelObj = new GameObject();
        newLevelObj.transform.name = currentLevelName;
        newLevelObj.transform.parent = transform;
        LevelLoader.InstantiateLevel(serializedLevel, prefabs, newLevelObj.transform);
        */
        //EventManager.onLevelStarted?.Invoke(currentLevelName);
    }
}
