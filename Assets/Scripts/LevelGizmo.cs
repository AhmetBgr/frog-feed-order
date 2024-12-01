using UnityEngine;

public class LevelGizmo : MonoBehaviour
{
#if UNITY_EDITOR
    public static Vector3 pos { get; private set; }
    static Color color = new Color(2f, 2f, 2f);
    public static bool drawEnabled { get; private set; }

    public static void UpdateGizmo(Vector3 v, Color c) {
        pos = v;
        color = c;
    }

    public static void Enable(bool b) {
        drawEnabled = b;
    }

    // Used for drawing wire cube and line to help see where mouse input is aiming in level editor
    void OnDrawGizmos() {
        if (drawEnabled) {
            

            Gizmos.color = color;
            Gizmos.DrawWireCube(pos, new Vector3(1f, 0.5f, 1f));

            // Additional draws to make it look thicker, unneceserray otherwise.
            Gizmos.DrawWireCube(pos + (new Vector3(1f, 0.05f, 1f) * 0.01f), new Vector3(1f, 0.5f, 1f));
            Gizmos.DrawWireCube(pos - (new Vector3(1f, 0.05f, 1f) * 0.01f), new Vector3(1f, 0.5f, 1f));

            Gizmos.DrawLine(pos + Vector3.up*1.3f, pos + Vector3.down);

            // Additional draws to make it look thicker, unneceserray otherwise.
            Gizmos.DrawLine((pos + Vector3.up * 1.3f) + (new Vector3(1f, 0f, -1f) * 0.01f), pos + Vector3.down + (new Vector3(1f, 0f, -1f) * 0.01f));
            Gizmos.DrawLine((pos + Vector3.up * 1.3f) - (new Vector3(1f, 0f, -1f) * 0.01f), pos + Vector3.down - (new Vector3(1f, 0f, -1f) * 0.01f));
        }
    }
#endif
}
