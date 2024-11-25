using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FrogController : MonoBehaviour
{
    public FrogModal modal;

    public FrogView view;

    private GridController gridController;

    //public delegate void OnTongueMoveDelegate(List<Vector2Int> tonguePathCoord);
    public static event Action<List<Vector2Int>, EntityColor> OnTongueMove;
    public static event Action<List<Vector2Int>, List<Vector3>> OnSuccessfullEat;


    // Start is called before the first frame update
    void Start()
    {
        //base.Start();
        modal.view = view;
        gridController = FindAnyObjectByType<GridController>();
    }

    private void OnMouseDown() {
        // Get path for the tongue
        List<Vector3> tonguePath = GetTonguePath();

        if (tonguePath.Count <= 1) return;

        // Play anim        
        view.PlayTongueAnimation(tonguePath, Game.tongueMoveDur);

    }

    private List<Vector3> GetTonguePath() {
        List<Vector3> tonguePath = new List<Vector3>();
        tonguePath.Add(transform.position);

        List<Vector2Int> tonguePathCoord = new List<Vector2Int>();
        tonguePathCoord.Add(Utils.PosToCoord(transform.position));

        Vector2Int tongueDir = modal.dir;

        EntityModal nextEntity = gridController.GetEntity(gridController.GetNextCoord(modal.coord, tongueDir));

        if (nextEntity == null) {
            Debug.LogWarning($"Next cell has no entity: {gridController.GetNextCoord(modal.coord, tongueDir)}");
            return tonguePath;
        }

        float delay = 0f;



        while (IsValidEntity(nextEntity)) {
            // Add entity position to path
            tonguePath.Add(nextEntity.transform.position);
            tonguePathCoord.Add(Utils.PosToCoord(nextEntity.transform.position));

            // Trigger animation 
            //nextEntity.view.AnimatePunchScale(Vector3.one * 0.5f, 0.2f, delay);

            // change dir if next enitity is arrow iwth same color
            if(nextEntity.type == EntityType.Arrow && nextEntity.color == modal.color) {
                tongueDir = nextEntity.dir;
            }

            // Update delay and get the next entity
            delay += Game.tongueMoveDur;
            nextEntity = gridController.GetEntity(gridController.GetNextCoord(nextEntity.coord, tongueDir));

            if (nextEntity == null) {
                Debug.Log("Next cell has no entity.");
                break;
            }
        }

        // Check if eating is succesfull
        if (!nextEntity || (nextEntity.type != EntityType.Frog && nextEntity.color == modal.color)) {
            // Trigger event
            OnSuccessfullEat?.Invoke(tonguePathCoord, tonguePath);

            StartCoroutine(modal.TriggerOnExpire((tonguePath.Count - 1) * Game.tongueMoveDur * 2 + Game.tongueMoveDur));

            //view.AnimateScale(Vector3.zero, 0.5f, (tonguePath.Count-1) * Game.tongueMoveDur * 2 + Game.tongueMoveDur);
        }

        OnTongueMove?.Invoke(tonguePathCoord, modal.color);

        return tonguePath;
    }

    private bool IsValidEntity(EntityModal entity) {
        // Check if the entity matches the desired type and color
        return (entity.type == EntityType.Grape || entity.type == EntityType.Arrow) && entity.color == modal.color;
    }


}
