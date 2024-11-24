using System.Collections;
using System.Collections.Generic;
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

    void OnDrawGizmos() {
        if (drawEnabled) {
            Gizmos.color = color;
            Gizmos.DrawWireCube(pos, new Vector3(1f, 0.1f, 1f));

            // Additional draws to make it look thicker, unneceserray otherwise.
            Gizmos.DrawWireCube(pos + (new Vector3(1f, 0.05f, 1f) * 0.01f), new Vector3(1f, 0.05f, 1f));
            Gizmos.DrawWireCube(pos - (new Vector3(1f, 0.05f, 1f) * 0.01f), new Vector3(1f, 0.05f, 1f));
        }
    }
#endif
}
