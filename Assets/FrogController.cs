using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : MonoBehaviour
{
    //public FrogView view;

    public FrogView view;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnMouseDown() {
        // Get path for the tongue

        // Test path
        List<Vector3> tonguePath = new List<Vector3>();
        tonguePath.Add(Vector3.zero);
        tonguePath.Add(Vector3.forward*-1);
        tonguePath.Add(new Vector3(1f,0f,-1f));

        // Play anim        
        view.PlayTongueAnimation(tonguePath, 0.5f);
    }
}
