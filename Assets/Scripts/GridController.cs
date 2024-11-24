using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class GridController : MonoBehaviour
{
    public Cell[] cellPrefabPools;

    public static Node[,] nodesGrid = new Node[5,5];

    public GameObject nodePrefab;

    // Start is called before the first frame update
    void Start()
    {
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

    public Node GetNode(Vector3 pos) {
        UpdateNodesGrid();

        int x = Mathf.Abs((int)pos.x);
        int y = Mathf.Abs((int)pos.z);

        if (x >= 5 | y >= 5) return null;

        return nodesGrid[x, y];
    }

    private void UpdateNodesGrid() {
        for (int i = 0; i < 25; i++) {
            int x = (i / 5);
            int y = i % 5;

            Node node = transform.GetChild(i).GetComponent<Node>();
            nodesGrid[x, y] = node;
        }
    }
}
