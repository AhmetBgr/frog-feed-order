using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] private List<FrogModal> frogs = new List<FrogModal>();

    [SerializeField] private SoundEffect levelCompleteSFX;
    [SerializeField] private SoundEffect gameOverSFX;

    [SerializeField] private int _movesCount;

    public static event Action onLevelComplete;
    public static event Action onGameOver;


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

    private void OnEnable() {
        FrogController.OnInteracted += DecreaseMoveCount;
        FrogController.onFrogExpire += CheckforLevelComplete;
        LevelManager.OnLeveload += Reset;
    }

    private void OnDisable() {
        FrogController.OnInteracted -= DecreaseMoveCount;
        FrogController.onFrogExpire -= CheckforLevelComplete;
        LevelManager.OnLeveload -= Reset;
    }

    private void Reset(int levelIndex) {
        Game.state = State.Playing;

        movesCount = LevelManager.instance.curSerializedLevel.movesCount;
    }

    private void TriggerOnMoveCountChanged() {
        OnMovesCountChanged?.Invoke(movesCount);
    }

    private void DecreaseMoveCount() {
        movesCount--;

        CheckForGameOver();

        //CheckforLevelComplete();
    }

    private void CheckForGameOver() {
        if (movesCount <= 0 && Game.state == State.Playing) {
            // Lose Condition
            Game.SetState(State.GameOver);
            AudioManager.instance.PlaySound(gameOverSFX, delay: 0.5f);
            onGameOver?.Invoke();
            frogs.Clear();

            return;
        }
    }

    private void CheckforLevelComplete(float delay) {
        if (Game.state != State.Playing) return;

        // Check if all frogs are expired
        foreach (var item in frogs) {
            if (!item.isExpired) {
                CheckForGameOver();
                return;
            }
        }

        frogs.Clear();

        // Trigger Level complete event
        Game.SetState(State.LevelComplete);

        StartCoroutine(TriggerOnLevelComplete(delay));
    }

    private IEnumerator TriggerOnLevelComplete(float delay) {
        yield return new WaitForSecondsRealtime(delay);

        AudioManager.instance.PlaySound(levelCompleteSFX);
        onLevelComplete?.Invoke();
    }

    public void AddToFrogsPool(FrogModal frog) {
        if (frogs.Contains(frog)) return;

        frogs.Add(frog);
    }

    public void RemoveFromFrogsPool(FrogModal frog) {
        if (!frogs.Contains(frog)) return;

        frogs.Remove(frog);
    }
}
