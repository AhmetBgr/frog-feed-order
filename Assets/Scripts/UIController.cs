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

    private string movesCounterBaseText = "{0} Moves";
    private string levelNameBaseText = "Level {0}";


    private void OnEnable() {
        GameManager.instance.OnMovesCountChanged += UpdateMoveCounterText;
        GameManager.onLevelComplete += OpenLevelCompletePanel;
        GameManager.onGameOver += OpenGameOverPanel;

        LevelManager.OnLeveload += UpdateLevelNameText;
    }

    private void OnDisable() {
        GameManager.instance.OnMovesCountChanged -= UpdateMoveCounterText;
        GameManager.onLevelComplete -= OpenLevelCompletePanel;
        GameManager.onGameOver -= OpenGameOverPanel;


        LevelManager.OnLeveload -= UpdateLevelNameText;
    }

    private void Start() {
        restartButton.onClick.AddListener(LevelManager.instance.ReloadCurLevel);
        nextButton.onClick.AddListener(LevelManager.instance.LoadNextLevel);
    }

    void UpdateMoveCounterText(int movesCount){
        movesCounterText.text = String.Format(movesCounterBaseText, movesCount);// movesCount.ToString();

        movesCounterText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
    }

    void UpdateLevelNameText(int curLevelIndex) {
        Debug.Log("cur level index in uicont: " + curLevelIndex);
        levelNameText.text = String.Format(levelNameBaseText, curLevelIndex + 1); //(curLevelIndex+1).ToString();

        //levelNameText.text = String.Format(levelNameText.text, curLevelIndex + 1);

        levelNameText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
    }

    private void OpenLevelCompletePanel() {

        levelCompletePanel.gameObject.SetActive(true);
        Vector3 initScale = levelCompletePanel.localScale;
        levelCompletePanel.localScale = Vector3.zero;
        levelCompletePanel.DOScale(initScale, 0.5f);


        // Disable next level button when completed the final level
        if (LevelManager.currentLevelIndex >= LevelManager.instance.levels.Count - 1)
            nextButton.gameObject.SetActive(false);

        /*if (state == State.LevelComplete) {

        }
        else if(state == State.GameOver) {
            gameOverPanel.gameObject.SetActive(true);
            Vector3 initScale = gameOverPanel.localScale;
            gameOverPanel.localScale = Vector3.zero;
            gameOverPanel.DOScale(initScale, 2f).SetDelay(0.5f);
        }*/

    }

    private void OpenGameOverPanel() {

        gameOverPanel.gameObject.SetActive(true);
        Vector3 initScale = gameOverPanel.localScale;
        gameOverPanel.localScale = Vector3.zero;
        gameOverPanel.DOScale(initScale, 2f).SetDelay(0.5f);
    }
}
