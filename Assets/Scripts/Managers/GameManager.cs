using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<FrogModal> frogs = new List<FrogModal>();

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
        FrogModal.onFrogExpire += CheckforLevelComplete;
    }

    private void OnDisable() {
        FrogController.OnInteracted -= DecreaseMoveCount;
        FrogModal.onFrogExpire -= CheckforLevelComplete;

    }

    private void TriggerOnMoveCountChanged() {
        OnMovesCountChanged?.Invoke(movesCount);

        //Debug.Log("move count:" + movesCount);
    }

    private void DecreaseMoveCount() {
        movesCount--;

        CheckForGameOver();
    }

    private void CheckForGameOver() {
        if (movesCount <= 0 && Game.state != State.LevelComplete) {
            // Lose Condition
            Game.SetState(State.GameOver);
            return;
        }
    }

    private void CheckforLevelComplete() {
        // check for level complete

        foreach (var item in frogs) {
            if (!item.isExpired) {
                return;
            }
        }

        // Trigger Level complete event
        Game.SetState(State.LevelComplete);
    }

    public void AddToFrogsPool(FrogModal frog) {
        frogs.Add(frog);
    }
}
