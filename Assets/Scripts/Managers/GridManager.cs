using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Cell[] cellPrefabPools;

    public GameObject[] cellPrefabs;


    public static Node[,] nodesGrid = new Node[5,5];

    public GameObject nodePrefab;

    private const int gridSize = 5;

    public static GridManager instance { get; private set; }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject); // Destroy duplicate
            return;
        }

        instance = this;
        //DontDestroyOnLoad(gameObject); // Make the instance persistent
    }

    void Start(){
        UpdateNodesGrid();

    }

    // Deletes and then regenrates all nodes
    public void PopulateNodesGrid() {
        // Find all nodes
        List<GameObject> objs = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++) {
            objs.Add(transform.GetChild(i).gameObject);
        }

        // Destroy all found nodes
        foreach (var item in objs) {
            DestroyImmediate(item);
        }
        
        // Regenerate all nodes
        for (int i = 0; i < gridSize* gridSize; i++) {
            int x = (i / gridSize);
            int y = i % gridSize;

            Node node = Instantiate(nodePrefab, new Vector3(x, 0f, -y), Quaternion.identity).GetComponent<Node>();
            node.transform.SetParent(transform);
            nodesGrid[x, y] = node;

        }
    }

    public GameObject GetCellPrefab(EntityType type, EntityColor color) {
        UpdateNodesGrid();

        var firstMatch = Array.Find(cellPrefabPools, elem => elem.entity.entityModal.type == type && elem.entity.entityModal.color == color);

        return firstMatch.gameObject; // Return null if entity not found
    }

    public Node GetNode(Vector2Int coord) {
        UpdateNodesGrid();

        if (IsOutOfGrid(coord)) return null;

        return nodesGrid[coord.x, coord.y];
    }

    public EntityController GetEntity(Vector2Int coord) {
        if (IsOutOfGrid(coord)) return null;

        return nodesGrid[coord.x, coord.y].topCell.entity;
    }

    public Vector2Int GetNextCoord(Vector2Int fromCoord, Vector2Int dir) {
        Vector2Int nextCoord = new Vector2Int(fromCoord.x + dir.x, fromCoord.y - dir.y);

        return nextCoord;
    }

    // Checks if given coordinates is out of the grid
    public bool IsOutOfGrid(Vector2Int coord) {
        if (coord.x >= gridSize | coord.y >= gridSize) return true;

        if (coord.x < 0 | coord.y < 0) return true;

        return false;
    }

    // Updates grid nodes matrix,
    // Grid nodes matrix is used to find nodes with given coord
    private void UpdateNodesGrid() {
        for (int i = 0; i < gridSize*gridSize; i++) {
            int x = (i / gridSize);
            int y = i % gridSize;

            Node node = transform.GetChild(i).GetComponent<Node>();
            nodesGrid[x, y] = node;
        }
    }
}
