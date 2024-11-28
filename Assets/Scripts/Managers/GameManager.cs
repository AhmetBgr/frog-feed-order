using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] private List<FrogModal> frogs = new List<FrogModal>();

    //public FrogModal[] frogs;

    [SerializeField] private int _movesCount;

    public SoundEffect levelCompleteSFX;
    public SoundEffect gameOverSFX;


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

        //Reset(0);
    }

    private void OnEnable() {
        FrogController.OnInteracted += DecreaseMoveCount;
        FrogModal.onFrogExpire += CheckforLevelComplete;
        LevelManager.OnLeveload += Reset;
    }

    private void OnDisable() {
        FrogController.OnInteracted -= DecreaseMoveCount;
        FrogModal.onFrogExpire -= CheckforLevelComplete;
        LevelManager.OnLeveload -= Reset;

    }

    private void Reset(int levelIndex) {
        Game.state = State.Playing;


        frogs.Clear();

        FrogModal[] frogsArray = FindObjectsOfType<FrogModal>(true);

        foreach (var item in frogsArray) {
            frogs.Add(item);
        }


        movesCount = LevelManager.instance.curSerializedLevel.movesCount;
    }

    private void TriggerOnMoveCountChanged() {


        OnMovesCountChanged?.Invoke(movesCount);

        Debug.Log("shoud trigger on move count chenged:" + movesCount);



    }

    private void DecreaseMoveCount() {
        movesCount--;

        CheckForGameOver();
    }

    private void CheckForGameOver() {
        if (movesCount <= 0) {
            // Lose Condition
            Game.SetState(State.GameOver);
            AudioManager.instance.PlaySound(gameOverSFX);
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
        AudioManager.instance.PlaySound(levelCompleteSFX);

    }

    public void AddToFrogsPool(FrogModal frog) {
        if (frogs.Contains(frog)) return;

        frogs.Add(frog);

        #if UNITY_EDITOR
        
        EditorUtility.SetDirty(this);
        EditorSceneManager.MarkSceneDirty(gameObject.scene);
        EditorSceneManager.SaveOpenScenes();
        
        #endif
    }
    public void RemoveFromFrogsPool(FrogModal frog) {
        if (!frogs.Contains(frog)) return;


        frogs.Remove(frog);

#if UNITY_EDITOR

        EditorUtility.SetDirty(this);
        EditorSceneManager.MarkSceneDirty(gameObject.scene);
        EditorSceneManager.SaveOpenScenes();

#endif
    }
}
