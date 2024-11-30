using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FrogController : EntityController{
    [SerializeField] private FrogModal modal;
    [SerializeField] private FrogView view;

    private GridManager gridManager;
    private List<EntityModal> visitedEntities = new List<EntityModal>();
    private bool isBusy = false;

    public static event Action OnInteracted;
    public static event Action<List<Vector2Int>, EntityColor> OnTongueMove;
    public static event Action<List<Vector2Int>, List<Vector3>> OnSuccessfullEat;

    // we need seperate event from the EntityModal Class as well because 
    // GameManager needs to track it specificly for frogs
    public static event Action<float> onFrogExpire;


    protected  void Start(){
        //base.Start();

        modal.view = view;
        gridManager = GridManager.instance;
    }


    public override void OnSpawn() {
        base.OnSpawn();
        UpdateDirection();
        GameManager.instance.AddToFrogsPool(modal);

    }

    public override IEnumerator TriggerOnExpire(float delay = 0) {
        onFrogExpire?.Invoke(delay);

        yield return base.TriggerOnExpire(delay);
    }

    private void OnMouseDown() {
        // Check if can be interacted
        if (isBusy || Game.state != State.Playing) return;

        isBusy = true;

        AudioManager.instance.PlaySound(view.interactSFX);

        List<Vector3> tonguePath = GetTonguePath();

        OnInteracted?.Invoke();

        if (Game.state == State.GameOver) return;

        view.PlayTongueAnimation(tonguePath, Game.tongueMoveDur, onCompleteCallBack: () => { isBusy = false; });

    }

    private List<Vector3> GetTonguePath() {
        visitedEntities.Clear();

        List<Vector3> tonguePath = new List<Vector3>();
        tonguePath.Add(transform.position);

        List<Vector2Int> tonguePathCoord = new List<Vector2Int>();
        tonguePathCoord.Add(Utils.PosToCoord(transform.position));

        Vector2Int tongueDir = modal.dir;

        EntityModal nextEntity = gridManager.GetEntity(gridManager.GetNextCoord(modal.coord, tongueDir))?.entityModal;

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

            if (!IsValidEntity(nextEntity)) {
                // Play deny feedback animation 
                Vector3 dir = new Vector3(tongueDir.x, 0f, tongueDir.y) * 0.3f;
                nextEntity.view.AnimatePunchPos(dir, Game.tongueMoveDur, Game.tongueMoveDur * (tonguePath.Count-1), nextEntity.view.entityDenySFX);
                break;
            }

            // change dir if next enitity is arrow with the same color
            if(nextEntity.type == EntityType.Arrow && nextEntity.color == modal.color) {
                tongueDir = nextEntity.dir;
            }

            // Update delay and get the next entity
            delay += Game.tongueMoveDur;
            nextEntity = gridManager.GetEntity(gridManager.GetNextCoord(nextEntity.coord, tongueDir))?.entityModal;
        }

        if (CanEat(nextEntity)) {
            //modal.isExpired = true;

            // Trigger event
            OnSuccessfullEat?.Invoke(tonguePathCoord, tonguePath);

            // Trigger expiration event with delay,
            // delay is calculated based on tongue path long and unit move dur
            modal.isExpired = true;
            StartCoroutine(TriggerOnExpire((tonguePath.Count - 0.5f) * Game.tongueMoveDur * 2 + Game.tongueMoveDur));
        }

        // Trigger event
        OnTongueMove?.Invoke(tonguePathCoord, modal.color);

        return tonguePath;
    }

    // Checks if entity is valid for tongue path
    private bool IsValidEntity(EntityModal entity) {
        return (entity.type == EntityType.Grape || entity.type == EntityType.Arrow) && entity.color == modal.color;

    }

    // Checks if frog can succesfully eat
    private bool CanEat(EntityModal entity) {
        bool hasGrape = false;

        foreach (var item in visitedEntities) {
            if (item.type == EntityType.Grape) {
                hasGrape = true;
                break;
            }
        }

        if (!hasGrape) return false;

        return !entity || entity.type == EntityType.Frog || (entity.type == EntityType.Grape && entity.color == modal.color);
    }
}
