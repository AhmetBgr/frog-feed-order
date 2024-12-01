using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LevelEditor : EditorWindow {
    [SerializeField] private GridManager gridManager;
    [SerializeField] private LevelManager levelManager;

    private string LevelPath => $"{Application.dataPath}/Resources/Levels/";

    private string[] actionOptions;
    private string[] colorOptions;

    // List of all saved levels loaded from the project resources.
    private List<string> savedLevels => Utils.allLevels;

    private int selectedActionIndex = 0;
    private int selectedColorIndex = 0;
    private int savedLevelIndex = 0;

    private string newLevelName;
    private int movesCount;
    private string currentLevel;

    private Vector2 scrollPosition;

    // Custom GUI style for consistent wrapping and styling of the editor UI.
    private GUIStyle wrapperStyle;
    private GUIStyle WrapperStyle => wrapperStyle ??= CreateWrapperStyle();

    private Event currentEvent;

    [MenuItem("Window/Level Editor")]
    public static void ShowWindow() => GetWindow<LevelEditor>();

    private void OnEnable() {
        SceneView.duringSceneGui += OnSceneGUI;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        InitializeReferences();
        InitializeUIOptions();
    }

    private void OnDisable() {
        SceneView.duringSceneGui -= OnSceneGUI;
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }

    private void InitializeReferences() {
        gridManager = FindObjectOfType<GridManager>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void InitializeUIOptions() {
        actionOptions = new[]
        {
            "None", "Erase", "Rotate", "Add Grape", "Add Frog", "Add Arrow"
        };

        colorOptions = new[]
        {
            "Blue", "Purple", "Green", "Red", "Yellow"
        };
    }

    private void OnGUI() {
        GUILayout.BeginVertical(WrapperStyle);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        DrawActionSelection();
        BigSpace();
        BigSpace();

        DrawHorizontalLine();
        DrawMovesCount();
        BigSpace();
        BigSpace();
        
        DrawHorizontalLine();
        DrawGridActions();
        BigSpace();
        BigSpace();
        
        DrawHorizontalLine();
        DrawLevelSaveLoad();

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    // Handles real-time interactions in the Scene View (e.g., mouse clicks).
    private void OnSceneGUI(SceneView sceneView) {
        currentEvent = Event.current;

        HandleSceneMouseInput();    // Process mouse input for placing or modifying cells.

        Vector3 currentPosition = GetGridPosition(currentEvent.mousePosition);
        UpdateGizmo(currentPosition);   // Update the visual indicator in the scene.

        sceneView.Repaint();    // Repaint the scene to reflect changes.
    }

    private void HandleSceneMouseInput() {
        if (currentEvent.type == EventType.MouseDown) {
            ApplyAction();
        }
    }

    private void DrawActionSelection() {
        DrawHorizontalLine();
        EditorGUILayout.LabelField("Select Action:", EditorStyles.boldLabel);
        selectedActionIndex = GUILayout.SelectionGrid(selectedActionIndex, actionOptions, actionOptions.Length / 2, GUILayout.Height(120));

        EditorGUILayout.LabelField("Choose Color:", EditorStyles.boldLabel);
        selectedColorIndex = GUILayout.SelectionGrid(selectedColorIndex, colorOptions, colorOptions.Length, GUILayout.Height(30));
    }

    private void DrawMovesCount() {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Moves Count:");
        movesCount = EditorGUILayout.IntField(movesCount);

        EditorGUILayout.EndHorizontal();
    }

    private void DrawGridActions() {
        if (GUILayout.Button("Clear Grid")) {
            gridManager.DestroyAllNodes();
            gridManager.PopulateNodesGrid();
        }
    }

    private void DrawLevelSaveLoad() {
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("LEVEL NAME:");
        newLevelName = EditorGUILayout.TextField(newLevelName);

        if (GUILayout.Button("Save Level")) {
            SaveLevel(newLevelName);
        }
        EditorGUILayout.EndHorizontal();

        BigSpace();

        GUILayout.Label("Load Saved Levels:");

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Load Level", GUILayout.Width(150))) {
            currentLevel = savedLevels[savedLevelIndex];
            LoadLevel(currentLevel);
        }
        savedLevelIndex = EditorGUILayout.Popup(savedLevelIndex, savedLevels.ToArray());

        EditorGUILayout.EndHorizontal();

    }

    private void ApplyAction() {
        // Find node in mouse position
        gridManager.PopulateNodesGrid();
        Node targetNode = gridManager.GetNode(Utils.PosToCoord(GetGridPosition(currentEvent.mousePosition)));

        if (targetNode == null) {
            return;
        }

        switch (selectedActionIndex) {
            case 1:
            targetNode.DestroyTopCell();
            break;
            case 2:
            RotateEntity(targetNode);
            break;
            default:
            AddEntityToNode(targetNode);
            break;
        }
    }

    // Rotates an entity on a specific node by 90 degrees.
    private void RotateEntity(Node node) {
        EntityModal entity = node.topCell?.entity.modal;

       
        if (entity == null || entity.type == EntityType.Grape) {
            return;
        }

        Vector3 rotation = node.topCell.transform.rotation.eulerAngles;
        node.topCell.transform.eulerAngles = new Vector3(rotation.x, (rotation.y + 90) % 360, rotation.z);
    }

    private void AddEntityToNode(Node node) {
        EntityType entityType = GetSelectedEntityType();
        EntityColor color = GetSelectedEntityColor();

        if (entityType != EntityType.None) {
            node.AddCell(entityType, color);
        }
    }

    private EntityType GetSelectedEntityType() {
        return selectedActionIndex switch {
            3 => EntityType.Grape,
            4 => EntityType.Frog,
            5 => EntityType.Arrow,
            _ => EntityType.None
        };
    }

    private EntityColor GetSelectedEntityColor() {
        return selectedColorIndex switch {
            0 => EntityColor.Blue,
            1 => EntityColor.Purple,
            2 => EntityColor.Green,
            3 => EntityColor.Red,
            4 => EntityColor.Yellow,
            _ => EntityColor.Blue
        };
    }

    private void SaveLevel(string levelName) {
        if (string.IsNullOrWhiteSpace(levelName)) {
            Debug.LogWarning("Level name cannot be empty.");
            return;
        }

        string path = $"{LevelPath}{levelName}.json";

        if (!Directory.Exists(LevelPath)) {
            Directory.CreateDirectory(LevelPath);
        }

        File.WriteAllText(path, JsonUtility.ToJson(new SerializedLevel(gridManager.gameObject, movesCount)));
        AssetDatabase.Refresh();
    }

    private void LoadLevel(string levelName) {
        if (string.IsNullOrWhiteSpace(levelName)) {
            Debug.LogWarning("Invalid level name.");
            return;
        }
        gridManager.DestroyAllNodes();
        levelManager.LoadLevel(gridManager.transform, LevelManager.LoadLevelTextFile(levelName));
        movesCount = levelManager.curSerializedLevel.movesCount;

    }

    // Converts a screen mouse position to a position on the grid.
    private Vector3 GetGridPosition(Vector3 mousePosition) {
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f)) {
            return Utils.Vec3ToInt(hit.transform.position) + (hit.normal * 0.2f); // + hit.normal.normalized
        }

        return Vector3.one * 999; // Return an out of grid position
    }

    private void UpdateGizmo(Vector3 position) {
        Vector2Int coord = Utils.PosToCoord(position);
        bool isOutOfGrid = gridManager.IsOutOfGrid(coord);
        
        LevelGizmo.Enable(!isOutOfGrid); // Disable if out of grid

        if (isOutOfGrid) return;

        Color color = GetSelectedColor();
        LevelGizmo.UpdateGizmo(position, color);
    }

    private Color GetSelectedColor() {
        return selectedColorIndex switch {
            0 => Color.blue,
            1 => Color.magenta,
            2 => Color.green,
            3 => Color.red,
            4 => Color.yellow,
            _ => Color.white
        };
    }

    private void OnPlayModeStateChanged(PlayModeStateChange state) {
        if (state == PlayModeStateChange.EnteredEditMode) {
            InitializeReferences();
        }
    }

    private GUIStyle CreateWrapperStyle() {
        return new GUIStyle {
            padding = new RectOffset(20, 20, 20, 20),
            normal = { background = Utils.MakeTex(1, 1, new Color(0.16f, 0.16f, 0.16f, 1f)) }
        };
    }

    private void DrawHorizontalLine() => EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

    void BigSpace() {
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }
}
