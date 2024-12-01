using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour{

    public List<Cell> cells = new List<Cell>();
    public Cell topCell;

    private static GridManager gridManager;

    void Start(){
        gridManager = GridManager.instance;
    }

    // Updates top cell and disables other cell's entities
    public void UpdateTopCell() {
        cells.Clear();
        for (int i = 0; i < transform.childCount; i++) {
            Cell cell = transform.GetChild(i).GetComponent<Cell>();


            if (i < transform.childCount - 1) {
                cell.entity?.gameObject.SetActive(false);
            }

            cells.Add(cell);
        }

        topCell = cells[cells.Count - 1];

    }
    public Cell RemoveTopCell() {
        if (topCell.entity == null) {
            // Prevent removing the base cell
            Debug.LogWarning("tried to remove base cell");
            return null;
        }
        Cell cellToRemove = cells[cells.Count - 1]; 

        cells.Remove(cellToRemove);

        topCell = cells[cells.Count - 1];

        if (!topCell.entity) return cellToRemove;

        topCell.entity.gameObject.SetActive(true);

        return cellToRemove;
    }

    // Used by Level Editor
    public void DestroyTopCell() {
        UpdateTopCell();

        if (!topCell.entity) return;

        DestroyImmediate(RemoveTopCell().gameObject);
    }

    // Instanciate new cell to the top used by LevelEditor
    public void AddCell(EntityType type, EntityColor color) {
        if (gridManager == null && transform.parent != null)
            transform.parent.TryGetComponent(out gridManager);

        if (gridManager == null) return;

        GameObject prefab = gridManager.GetCellPrefab(type, color);

        GameObject cell = Instantiate(prefab, position: transform.GetChild(transform.childCount-1).position + (Vector3.up * 0.1f), Quaternion.identity);
        cell.transform.SetParent(transform);

        cells.Add(topCell);

        UpdateTopCell();
    }

}
