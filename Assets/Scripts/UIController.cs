using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public RectTransform levelCompletePanel;
    public RectTransform gameOverPanel;


    public TextMeshProUGUI movesCounterText;

    private void OnEnable() {
        GameManager.instance.OnMovesCountChanged += UpdateMoveCounterText;
        Game.onStateChanged += TryOpenCorrectPanel;

    }

    private void OnDisable() {
        GameManager.instance.OnMovesCountChanged -= UpdateMoveCounterText;
        Game.onStateChanged -= TryOpenCorrectPanel;


    }

    void UpdateMoveCounterText(int movesCount){
        //Debug.Log("move count in uicont: " + movesCount);
        movesCounterText.text = movesCount.ToString();
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