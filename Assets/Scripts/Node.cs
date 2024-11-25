using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    public List<Cell> cells = new List<Cell>();
    public Cell topCell;

    private static GridController gridController;

    private void OnEnable() {
        cells.Clear();
        for (int i = 0; i < transform.childCount; i++) {
            cells.Add(transform.GetChild(i).GetComponent<Cell>());
        }

        topCell = cells[cells.Count - 1];

        if (!topCell.entity) return;

        topCell.entity.OnExpire += RemoveTopCell;
    }

    private void OnDisable() {
        if (!topCell.entity) return;

        topCell.entity.OnExpire -= RemoveTopCell;
    }


    void Start()
    {
        if(gridController == null && transform.parent != null)
            transform.parent.TryGetComponent(out gridController);


    }

    public void RemoveTopCell() {
        if (!topCell.entity) return; // dont remove the base cell

        topCell.AnimateScale();


        cells.RemoveAt(cells.Count - 1);

        topCell = cells[cells.Count - 1];

        if (!topCell.entity) return;

        topCell.entity.gameObject.SetActive(true);

    }

    public void DeleteTopCell() {
        if (transform.childCount <= 1) return;

        if (cells.Count > 1) {
            cells[cells.Count - 2].entity.gameObject.SetActive(true);
        }

        cells.RemoveAt(cells.Count - 1);
        
        DestroyImmediate(transform.GetChild(transform.childCount - 1).gameObject);


    }

    public void AddCell(EntityType type, EntityColor color) {
        if (gridController == null && transform.parent != null)
            transform.parent.TryGetComponent(out gridController);

        if (gridController == null) return;

        GameObject prefab = gridController.GetCellPrefab(type, color);

        GameObject cell = Instantiate(prefab, position: transform.GetChild(transform.childCount-1).position + (Vector3.up * 0.1f), Quaternion.identity);
        cell.transform.SetParent(transform);

        if (topCell.entity != null)
            topCell.entity.gameObject.SetActive(false);

        topCell = cell.GetComponent<Cell>();

        cells.Add(topCell);
        /*cells.Add(cell.GetComponent<Cell>());

        if(cells.Count > 1) {
            cells[cells.Count - 2].entity.gameObject.SetActive(false);
        }*/

    }

}
