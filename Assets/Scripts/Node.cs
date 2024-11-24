using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    public List<Cell> cells = new List<Cell>();

    private static GridController gridController;

    void Start()
    {
        if(gridController == null && transform.parent != null)
            transform.parent.TryGetComponent(out gridController);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveLastCell() {
        if (transform.childCount <= 1) return;

        /*if (cells.Count > 1) {
            cells[cells.Count - 2].entity.gameObject.SetActive(true);
        }
        cells.RemoveAt(cells.Count - 1);*/
        DestroyImmediate(transform.GetChild(transform.childCount - 1).gameObject);


    }

    public void AddCell(EntityType type, EntityColor color) {
        if (gridController == null && transform.parent != null)
            transform.parent.TryGetComponent(out gridController);

        if (gridController == null) return;

        GameObject prefab = gridController.GetCellPrefab(type, color);

        GameObject cell = Instantiate(prefab, position: transform.GetChild(transform.childCount-1).position + (Vector3.up * 0.1f), Quaternion.identity);
        cell.transform.SetParent(transform);
        
        /*cells.Add(cell.GetComponent<Cell>());

        if(cells.Count > 1) {
            cells[cells.Count - 2].entity.gameObject.SetActive(false);
        }*/

    }

}
