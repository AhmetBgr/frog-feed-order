using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoundEffect))]
public class SoundEditor : Editor {

    public override void OnInspectorGUI() {

        DrawDefaultInspector();

        SoundEffect sound = (SoundEffect)target;

        // Volume slider
        sound.volume = EditorGUILayout.Slider(sound.volume, leftValue: 0, rightValue: 1);

        // Toggle for random pitch usage
        sound.useRandomPitch = GUILayout.Toggle(sound.useRandomPitch, "Use Random Pitch");

        if (sound.useRandomPitch)
            sound.pitchRandom = EditorGUILayout.Vector2Field("Random Pitch", sound.pitchRandom);
        else
            sound.pitch = EditorGUILayout.Slider(sound.pitch, leftValue: -3, rightValue: 3);

        if (GUILayout.Button("Play")) {
            sound.PlayPreview();
        }

        if (sound.source && sound.source.isPlaying && GUILayout.Button("Stop")) {
            sound.StopPreview();
        }

        // Mark the sound object as dirty to ensure changes are saved
        EditorUtility.SetDirty(sound);
    }
}