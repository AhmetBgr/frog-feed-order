using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Debugger : MonoBehaviour
{
    private bool resetTargetFps = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {

            if (resetTargetFps) {
                resetTargetFps = false;
                Application.targetFrameRate = 60;
            }
            else {
                resetTargetFps = true;
                Application.targetFrameRate = 75;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Backspace)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
