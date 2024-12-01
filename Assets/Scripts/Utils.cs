using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    private static List<string> allLevelsRef;
    public static List<string> allLevels {
        get {
            if (allLevelsRef == null) {
                allLevelsRef = new List<string>();
                Object[] levels = Resources.LoadAll("Levels");
                foreach (Object t in levels) {
                    allLevelsRef.Add(t.name);
                }
            }
            return allLevelsRef;
        }
    }

    public static Vector3Int Vec3ToInt(Vector3 v) {
		return Vector3Int.RoundToInt(v);
	}

    public static Vector2Int PosToCoord(Vector3 pos) {
        return new Vector2Int(Mathf.Abs((int)pos.x), Mathf.Abs((int)pos.z));
    }

    public static Vector3 CoordToPos(Vector2Int coord) {
        return new Vector3(coord.x, 0f, -coord.y);
    }

    public static Texture2D MakeTex(int width, int height, Color col) {
        Color[] pix = new Color[width * height];

        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}
