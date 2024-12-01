using UnityEngine;
using UnityEngine.SceneManagement;

public class Debugger : MonoBehaviour{

    void Update(){   

        // Reload scene to start first level
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
