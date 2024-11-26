using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour{
    public static LevelManager instance { get; private set; }

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
}
