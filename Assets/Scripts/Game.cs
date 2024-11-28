using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State {
    Playing, Paused, GameOver, LevelComplete
}

public class Game : MonoBehaviour
{
    public static float tongueMoveDur = 0.2f; // duration of frog's tongue to move 1 Unit
    public static State state;

    public static event Action<State> onStateChanged;

    private void Start() {
        state = State.Playing; 
    }

    public static void SetState(State state) {
        Game.state = state;

        onStateChanged?.Invoke(state);
    }

}
