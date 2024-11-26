using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<FrogController> frogs = new List<FrogController>();

    [SerializeField] private int _movesCount;

    public int movesCount {
        get => _movesCount;
        set {
            _movesCount = value;
            TriggerOnMoveCountChanged();
        }
    }

    public event Action<int> OnMovesCountChanged;

    public static GameManager instance { get; private set; }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject); // Destroy duplicate
            return;
        }

        instance = this;
        //DontDestroyOnLoad(gameObject); // Make the instance persistent

    }

    private void Start() {
        movesCount = _movesCount; // makes sure value given in inspector is triggered for event
    }

    private void OnEnable() {
        FrogController.OnInteracted += DecreaseMoveCount; 
    }

    private void OnDisable() {
        FrogController.OnInteracted -= DecreaseMoveCount;

    }

    private void TriggerOnMoveCountChanged() {
        OnMovesCountChanged?.Invoke(movesCount);

        //Debug.Log("move count:" + movesCount);

        // check for level complete
        foreach (var item in frogs) {
            if (!item.modal.isExpired)
                return;
        }

        // Trigger Level complete event
        Game.state = State.LevelComplete;



        if (movesCount <= 0 && Game.state != State.LevelComplete) {
            // Lose Condition
            Game.state = State.GameOver;
            return;
        }


    }

    private void DecreaseMoveCount() {
        movesCount--;
    }

    public void AddToFrogsPool(FrogController frog) {
        frogs.Add(frog);
    }
}
