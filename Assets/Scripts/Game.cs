using System;

// Currently Paused state not used but it's there if needed
public enum State {
    Playing, Paused, GameOver, LevelComplete
}

public class Game{

    // Duration of a unit to move 1 Unit. eg: frog's tongue.
    // Almost every entity depends on this variable so, 
    // changing this basicaly means changing the speed of the game
    public static float unitMoveDur = 0.2f; 

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
