using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State {
    Playing, Paused, GameOver, LevelComplete
}

public class Game : MonoBehaviour
{
    public static float tongueMoveDur = 0.3f; // duration of frog's tongue to move 1 Unit
    public static State state;


    private void Start() {
        state = State.Playing; 
    }

}
