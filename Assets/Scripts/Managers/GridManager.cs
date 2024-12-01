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

    public GameObject GetCellPrefab(EntityType type, EntityColor color) {
        var firstMatch = Array.Find(cellPrefabPools, elem => elem.entity.modal.type == type && elem.entity.modal.color == color);

        return firstMatch.gameObject; // Return null if entity not found
    }

    public Node GetNode(Vector2Int coord) {
        if (IsOutOfGrid(coord)) return null;

        return nodesGrid[coord.x, coord.y];
    }

    // Returns top entity at given coord.
    // Returns null if the coord is out of grid or node at the coord is empty.
    public EntityController GetEntity(Vector2Int coord) {
        if (IsOutOfGrid(coord)) return null;

        return nodesGrid[coord.x, coord.y].topCell.entity;
    }

    // Returns adjacent coordinate in given direction
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
    public void PopulateNodesGrid() {
        for (int i = 0; i < gridSize*gridSize; i++) {
            int x = (i / gridSize);
            int y = i % gridSize;
            Node node;

            if (i >= transform.childCount) {
                if (Application.isPlaying) {
                    // Gets node object from the pool when playing the game

                    node = ObjectPooler.instance.SpawnFromPool(nodePrefab.name).GetComponent<Node>();

                    node.transform.SetParent(transform);
                }
                else {
                    // Instantiates new node object. This is for when using level editor in scene view
                    node = Instantiate(nodePrefab, new Vector3(x, 0f, -y), Quaternion.identity).GetComponent<Node>();
                    node.transform.SetParent(transform);
                }
            }
            else {
                node = transform.GetChild(i).GetComponent<Node>();
            }

            nodesGrid[x, y] = node;
            node.transform.position = new Vector3(x, 0f, -y);
        }
    }

    public void DestroyAllNodes() {
        // Find all nodes
        List<GameObject> objs = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++) {
            objs.Add(transform.GetChild(i).gameObject);
        }

        // Destroy all found nodes
        foreach (var item in objs) {
            DestroyImmediate(item);
        }
    }
}
