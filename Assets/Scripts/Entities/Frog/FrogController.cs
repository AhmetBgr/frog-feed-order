using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FrogController : EntityController{
    public FrogModal modal;
    public FrogView view;

    private GridManager gridManager;
    private List<EntityModal> visitedEntities = new List<EntityModal>();
    private bool isBusy = false;

    // Events
    public static event Action OnInteracted;
    public static event Action<List<Vector2Int>, EntityColor> OnTongueMove;
    public static event Action<List<Vector2Int>, List<Vector3>> OnSuccessfullEat;

    void Start(){
        modal.view = view;
        gridManager = GridManager.instance;

    }

    private void OnMouseDown() {
        if (isBusy || Game.state != State.Playing) return;

        isBusy = true;

        //Play interaction sound effect
        AudioManager.instance.PlaySound(view.interactSFX);

        // Get path for the tongue
        List<Vector3> tonguePath = GetTonguePath();

        OnInteracted?.Invoke();

        // Play tongue move animation       
        view.PlayTongueAnimation(tonguePath, Game.tongueMoveDur, () => { isBusy = false; });

    }

    private List<Vector3> GetTonguePath() {
        visitedEntities.Clear();


        List<Vector3> tonguePath = new List<Vector3>();
        tonguePath.Add(transform.position);

        List<Vector2Int> tonguePathCoord = new List<Vector2Int>();
        tonguePathCoord.Add(Utils.PosToCoord(transform.position));

        Vector2Int tongueDir = modal.dir;

        EntityModal nextEntity = gridManager.GetEntity(gridManager.GetNextCoord(modal.coord, tongueDir));

        if (nextEntity == null) {
            Debug.LogWarning($"Next cell has no entity: {gridManager.GetNextCoord(modal.coord, tongueDir)}");
            return tonguePath;
        }

        float delay = 0f;

        while (nextEntity && !visitedEntities.Contains(nextEntity)) {
            //nextEntity.isVisited = true;
            visitedEntities.Add(nextEntity);

            // Add entity position to path
            tonguePath.Add(nextEntity.transform.position);
            tonguePathCoord.Add(Utils.PosToCoord(nextEntity.transform.position));

            // Trigger animation 
            //nextEntity.view.AnimatePunchScale(Vector3.one * 0.5f, 0.2f, delay);
            if (!IsValidEntity(nextEntity)) {
                nextEntity.view.AnimatePunchPos(new Vector3(tongueDir.x, 0f, tongueDir.y)*0.3f, Game.tongueMoveDur, Game.tongueMoveDur * (tonguePath.Count-1), view.grapeDenySFX);
                break;
            }

            // change dir if next enitity is arrow iwth same color
            if(nextEntity.type == EntityType.Arrow && nextEntity.color == modal.color) {
                tongueDir = nextEntity.dir;
            }

            // Update delay and get the next entity
            delay += Game.tongueMoveDur;
            nextEntity = gridManager.GetEntity(gridManager.GetNextCoord(nextEntity.coord, tongueDir));
        }

        // Check if eating is succesfull
        if (!nextEntity || nextEntity.type == EntityType.Frog || (nextEntity.type == EntityType.Grape && nextEntity.color == modal.color)) {
            modal.isExpired = true;

            // Trigger event
            OnSuccessfullEat?.Invoke(tonguePathCoord, tonguePath);

            StartCoroutine(modal.TriggerOnExpire((tonguePath.Count - 0.5f) * Game.tongueMoveDur * 2 + Game.tongueMoveDur));

            //view.AnimateScale(Vector3.zero, 0.5f, (tonguePath.Count-1) * Game.tongueMoveDur * 2 + Game.tongueMoveDur);
        }

        OnTongueMove?.Invoke(tonguePathCoord, modal.color);

        return tonguePath;
    }

    private bool IsValidEntity(EntityModal entity) {
        return (entity.type == EntityType.Grape || entity.type == EntityType.Arrow) && entity.color == modal.color;

    }


}
