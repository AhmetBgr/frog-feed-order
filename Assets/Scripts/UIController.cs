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

    private void OnEnable() {
        GameManager.instance.OnMovesCountChanged += UpdateMoveCounterText;
        Game.onStateChanged += TryOpenCorrectPanel;

    }

    private void OnDisable() {
        GameManager.instance.OnMovesCountChanged -= UpdateMoveCounterText;
        Game.onStateChanged -= TryOpenCorrectPanel;


    }

    private void Start() {
        restartButton.onClick.AddListener(LevelManager.instance.ReloadCurScene);
        nextButton.onClick.AddListener(LevelManager.instance.LoadNextScene);

    }

    void UpdateMoveCounterText(int movesCount){
        //Debug.Log("move count in uicont: " + movesCount);
        movesCounterText.text = movesCount.ToString();

        movesCounterText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
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
