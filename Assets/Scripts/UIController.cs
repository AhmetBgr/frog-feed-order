using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    public RectTransform levelCompletePanel;
    public RectTransform gameOverPanel;

    public Button restartButton;
    public Button nextButton;

    public TextMeshProUGUI movesCounterText;
    public TextMeshProUGUI levelNameText;


    private void OnEnable() {
        GameManager.instance.OnMovesCountChanged += UpdateMoveCounterText;
        Game.onStateChanged += TryOpenCorrectPanel;
        LevelManager.OnLeveload += UpdateLevelNameText;
    }

    private void OnDisable() {
        GameManager.instance.OnMovesCountChanged -= UpdateMoveCounterText;
        Game.onStateChanged -= TryOpenCorrectPanel;
        LevelManager.OnLeveload -= UpdateLevelNameText;


    }

    private void Start() {
        restartButton.onClick.AddListener(LevelManager.instance.ReloadCurScene);
        nextButton.onClick.AddListener(LevelManager.instance.LoadNextScene);
    }

    void UpdateMoveCounterText(int movesCount){
        movesCounterText.text = movesCount.ToString();

        movesCounterText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
    }

    void UpdateLevelNameText(int curLevelIndex) {
        Debug.Log("cur level index in uicont: " + curLevelIndex);
        levelNameText.text = (curLevelIndex+1).ToString();

        //levelNameText.text = String.Format(levelNameText.text, curLevelIndex + 1);

        levelNameText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
    }

    private void TryOpenCorrectPanel(State state) {

        if(state == State.LevelComplete) {
            levelCompletePanel.gameObject.SetActive(true);
        }
        else if(state == State.GameOver) {
            gameOverPanel.gameObject.SetActive(true);
        }

    }
}
