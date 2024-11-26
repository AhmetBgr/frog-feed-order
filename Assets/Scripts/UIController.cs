using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI movesCounterText;

    private void OnEnable() {
        GameManager.instance.OnMovesCountChanged += UpdateMoveCounterText;   
    }

    private void OnDisable() {
        GameManager.instance.OnMovesCountChanged -= UpdateMoveCounterText;

    }

    void UpdateMoveCounterText(int movesCount){
        //Debug.Log("move count in uicont: " + movesCount);
        movesCounterText.text = movesCount.ToString();
    }
}
