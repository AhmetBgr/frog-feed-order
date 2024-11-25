using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : MonoBehaviour
{
    public FrogModal modal;

    public FrogView view;

    private GridController gridController;
    // Start is called before the first frame update
    void Start()
    {
        gridController = FindAnyObjectByType<GridController>();
    }

    private void OnMouseDown() {
        // Get path for the tongue
        List<Vector3> tonguePath = new List<Vector3>();
        tonguePath.Add(transform.position);

        EntityModal nextEntity = gridController.GetEntity(gridController.GetNextCoord(modal.coord, modal.dir));

        if (nextEntity == null) {

            Debug.LogWarning("next cell has no entitiy: "+ gridController.GetNextCoord(modal.coord, modal.dir));
            return;

        } 

        while ((nextEntity.type == EntityType.Grape | nextEntity.type == EntityType.Arrow) && nextEntity.color == modal.color) {

            tonguePath.Add(nextEntity.transform.position);
            nextEntity = gridController.GetEntity(gridController.GetNextCoord(nextEntity.coord, modal.dir));

            if(nextEntity == null) {

                Debug.Log("next cell has no entity: " ); //+ gridController.GetNextCoord(nextEntity.coord, modal.dir)
                break;
            }

        }

        /*for (int i = 0; i < tonguePath.Count; i++) {
            Debug.Log("tongue path " + i + ": " + tonguePath[i]);
        }*/

        if (tonguePath.Count <= 1) return;

        // Play anim        
        view.PlayTongueAnimation(tonguePath, 0.2f);
    }
}
