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

    public static GridManager instance { get; private set; }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject); // Destroy duplicate
            return;
        }

        instance = this;
        //DontDestroyOnLoad(gameObject); // Make the instance persistent
    }

    void Start()
    {
        UpdateNodesGrid();

    }

    public void PopulateNodesGrid() {
        List<GameObject> objs = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++) {
            objs.Add(transform.GetChild(i).gameObject);
        }

        foreach (var item in objs) {
            DestroyImmediate(item);
        }
        
        for (int i = 0; i < 25; i++) {
            int x = (i / 5);
            int y = i % 5;

            Node node = Instantiate(nodePrefab, new Vector3(x, 0f, -y), Quaternion.identity).GetComponent<Node>();
            node.transform.SetParent(transform);
            //Node node = transform.GetChild(i).GetComponent<Node>();
            nodesGrid[x, y] = node;

        }
    }

    public GameObject GetCellPrefab(EntityType type, EntityColor color) {
        UpdateNodesGrid();

        var firstMatch = Array.Find(cellPrefabPools, elem => elem.entity.type == type && elem.entity.color == color);

        return firstMatch.gameObject; // Return null if entity not found
    }

    public Node GetNode(Vector2Int coord) {
        UpdateNodesGrid();

        if (coord.x >= 5 | coord.y >= 5) return null;

        if (coord.x < 0 | coord.y < 0) return null;


        return nodesGrid[coord.x, coord.y];
    }

    public EntityModal GetEntity(Vector2Int coord) {
        Debug.Log("coord: " + coord);

        if (coord.x >= 5 | coord.y >= 5) return null;

        if (coord.x < 0 | coord.y < 0) return null;



        return nodesGrid[coord.x, coord.y].topCell.entity;
    }

    public EntityModal GetUnvisitedEntity(Vector2Int coord) {

        Debug.Log("coord: " + coord);

        if (coord.x >= 5 | coord.y >= 5) return null;

        if (coord.x < 0 | coord.y < 0) return null;


        


        return nodesGrid[coord.x, coord.y].topCell.entity;
    }


    public Vector2Int GetNextCoord(Vector2Int fromCoord, Vector2Int dir) {
        Vector2Int nextCoord = new Vector2Int(fromCoord.x + dir.x, fromCoord.y - dir.y);

        return nextCoord;
    }

    public bool IsOutOfGrid(Vector2Int coord) {
        if (coord.x >= 5 | coord.y >= 5) return true;

        if (coord.x < 0 | coord.y < 0) return true;

        return false;
    }

    /*public Node GetNextNode(Vector2Int fromCoord, Vector2Int dir) {

        Vector2Int nextCoord = new Vector2Int(fromCoord.x + dir.x, fromCoord.y - dir.y);
        if (nextCoord.x >= 5 | nextCoord.y >= 5) return null;

        Node node = nodesGrid[nextCoord.x, nextCoord.y];

        if(node == null) {
            Debug.LogWarning("null next node is returned from: " + nextCoord);
        }
        else {
            Debug.Log("next node is returned from: " + nextCoord);
        }

        return node;
    }*/

    private void UpdateNodesGrid() {
        for (int i = 0; i < 25; i++) {
            int x = (i / 5);
            int y = i % 5;

            Node node = transform.GetChild(i).GetComponent<Node>();
            nodesGrid[x, y] = node;
        }
    }
}