using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.IO;


//[CustomEditor(typeof(GridController))]
public class LevelEditor : EditorWindow {
	public GridController gridController;

	int selGridInt = 0;
	int selGridInt2 = 0;

	string[] selectStrings;
	string[] selectStrings2;


	Event e;
	bool in2DMode;
	string currentLevel;
	static string textFilePath => Application.dataPath + "/leveleditorprefabs.txt";

	public GameObject[] prefabs;
	Vector2 scrollPos;
	bool refreshPrefabs = true;
	GUIStyle wrapperRef;
	GUIStyle wrapper {
		get {
			if (wrapperRef == null) {
				wrapperRef = new GUIStyle();
				wrapperRef.padding = new RectOffset(20, 20, 20, 20);
				float n = 0.16f;
				wrapperRef.normal.background = Utils.MakeTex(1, 1, new Color(n, n, n, 1f));
			}
			return wrapperRef;
		}
	}

    private void Awake() {

	}

    void OnEnable() {
		SceneView.duringSceneGui += SceneGUI;
		//Selection.selectionChanged += ApplyAction;

		gridController = FindAnyObjectByType<GridController>();

		List<string> selectStringsTmp = new List<string>();
		selectStringsTmp.Add("None");
		selectStringsTmp.Add("Erase");
		selectStringsTmp.Add("Rotate");
		selectStringsTmp.Add("Add Grape");
		selectStringsTmp.Add("Add Frog");
		selectStringsTmp.Add("Add Arrow");

		selectStrings = selectStringsTmp.ToArray();


		List<string> selectStringsTmp2 = new List<string>();
		selectStringsTmp2.Add("Blue");
		selectStringsTmp2.Add("Purple");
		selectStringsTmp2.Add("Green");
		selectStringsTmp2.Add("Red");
		selectStringsTmp2.Add("Yellow");

		selectStrings2 = selectStringsTmp2.ToArray();

	}

	void OnDisable() {
		SceneView.duringSceneGui -= SceneGUI;
		//Selection.selectionChanged -= ApplyAction;
	}



	void OnValidate() {
		refreshPrefabs = true;
	}


	[MenuItem("Window/Level Editor")]
	public static void ShowWindow() {
		EditorWindow.GetWindow(typeof(LevelEditor));
	}

	public void SceneGUI(SceneView sceneView) {

		e = Event.current;
		var controlID = GUIUtility.GetControlID(FocusType.Passive);
		var eventType = e.GetTypeForControl(controlID);
		if (eventType == EventType.MouseDown) {
			ApplyAction();
        }

		Vector3 currentPos = GetPosition(e.mousePosition);

		currentPos = new Vector3(currentPos.x, currentPos.y - 0.9f, currentPos.z);

		Color color = Color.white;

		if(selGridInt2 == 0) {
			color = Color.blue;
        }
		else if (selGridInt2 == 1) {
			color = Color.magenta;

		}
		else if (selGridInt2 == 2) {
			color = Color.green;

		}
		else if (selGridInt2 == 3) {
			color = Color.red;

		}
		else if (selGridInt2 == 4) {
			color = Color.yellow;

		}

		LevelGizmo.UpdateGizmo(currentPos, color);
		LevelGizmo.Enable(true); //selGridInt != 0
		sceneView.Repaint();
		Repaint();
	}



	void OnGUI() {

		string previousLevel = currentLevel;
		GUILayout.BeginVertical(wrapper);

		scrollPos = GUILayout.BeginScrollView(scrollPos);

		DrawingWindow();

		BigSpace();
		BigSpace();
		BigSpace();
		BigSpace();


		if (GUILayout.Button("Clear Grid")) {
			gridController.PopulateNodesGrid();
		}
		EditorGUILayout.EndScrollView();
		GUILayout.EndVertical();

	}

	void ApplyAction() {
		//if (Selection.activeGameObject == null) return;

		Node node;
		node = gridController.GetNode(Utils.PosToCoord(GetPosition(e.mousePosition)));

		if (node == null) return;

		if (selGridInt == 1) {

			node.RemoveLastCell();
		}
		else if (selGridInt == 2) {
			EntityModal entity = node.activeCell?.entity;
			Debug.Log("here1");


			if (!entity || entity.type == EntityType.Grape ) return;

			Debug.Log("here2");
			Vector3 angles = entity.transform.rotation.eulerAngles;

			entity.transform.eulerAngles = new Vector3(angles.x, (angles.y + 90)%360, angles.z);
		}
		else if (selGridInt == 3) {
			node.AddCell(EntityType.Grape, GetEntityColor(selGridInt2));

		}
		else if (selGridInt == 4) {
			node.AddCell(EntityType.Frog, GetEntityColor(selGridInt2));
		}
		else if (selGridInt == 5) {
			node.AddCell(EntityType.Arrow, GetEntityColor(selGridInt2));
		}
	}

	void DrawingWindow() {

		HorizontalLine();
	
		EditorGUILayout.Space();

		GUILayout.Label("Select Action: ", EditorStyles.boldLabel);

		selGridInt = GUILayout.SelectionGrid(selGridInt, selectStrings, selectStrings.Length, GUILayout.Width(470));


		BigSpace();

        if (refreshPrefabs) {

			refreshPrefabs = false;

		}

		GUILayout.Label("Choose Color:", EditorStyles.boldLabel);

		selGridInt2 = GUILayout.SelectionGrid(selGridInt2, selectStrings2, selectStrings2.Length, GUILayout.Width(370));

	}

	Vector3 GetPosition(Vector3 mousePos) {
		Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);

		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit, 100.0f)) {
			Vector3 pos = hit.point + (hit.normal * 1f);
			/*if (selGridInt == 1) {
				pos = hit.transform.position;
			}*/
			return Utils.Vec3ToInt(pos);
		}

		Plane hPlane = new Plane(Vector3.forward, Vector3.zero);
		float distance = 0;
		if (hPlane.Raycast(ray, out distance)) {
			return Utils.Vec3ToInt(ray.GetPoint(distance));
		}
	
		return Vector3.zero;
	}

	/*public void CreateGUI() {
		// Each editor window contains a root VisualElement object
		VisualElement root = rootVisualElement;

		// VisualElements objects can contain other VisualElement following a tree hierarchy
		Label label = new Label("Hello World!");
		root.Add(label);

		// Create button
		Button button = new Button();
		button.name = "button";
		button.text = "Button";
		root.Add(button);

		// Create toggle
		Toggle toggle = new Toggle();
		toggle.name = "toggle";
		toggle.label = "Toggle";
		root.Add(toggle);
	}*/

	EntityColor GetEntityColor(int index) {
		EntityColor entityColor = EntityColor.Blue;
        switch (index) {
			case 0:
				entityColor = EntityColor.Blue;
				break;
			case 1:
				entityColor = EntityColor.Purple;

				break;
			case 2:
				entityColor = EntityColor.Green;

				break;
			case 3:
				entityColor = EntityColor.Red;

				break;
			case 4:
				entityColor = EntityColor.Yellow;

				break;

		}
		return entityColor;
    }

	void HorizontalLine() => EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

	void BigSpace() {
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
	}
}
