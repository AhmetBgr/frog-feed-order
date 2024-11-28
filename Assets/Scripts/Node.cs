using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class Node : MonoBehaviour
{

    public List<Cell> cells = new List<Cell>();
    public Cell topCell;

    private static GridController gridController;


    private void OnEnable() {

        //LevelManager.OnLeveload += UpdateTopCell;

        Invoke("UpdateTopCell", 0.1f);
        Invoke("SubscribeOnExpire", 0.1f);

        //UpdateTopCell();

        //SubscribeOnExpire();
    }

    private void OnDisable() {
        UnsubscribeOnExpire();

        //LevelManager.OnLeveload -= UpdateTopCell;

    }


    void Start()
    {

        if(gridController == null && transform.parent != null)
            transform.parent.TryGetComponent(out gridController);



    }

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

    private void SubscribeOnExpire() {
        if (topCell.entity == null) {

            Debug.LogWarning("tried to subscribe base cell entity");
            return; // dont remove the base cell

        }
        topCell.entity.OnExpire += RemoveTopCell;
    }

    private void UnsubscribeOnExpire() {
        if (!topCell.entity) return;

        topCell.entity.OnExpire -= RemoveTopCell;
    }
    public void RemoveTopCell() {
        UnsubscribeOnExpire();


        if (topCell.entity == null) {

            Debug.LogWarning("tried to remove base cell");
            return; // dont remove the base cell
        }


        topCell.AnimateScale();


        cells.RemoveAt(cells.Count - 1);

        topCell = cells[cells.Count - 1];

        if (!topCell.entity) return;

        topCell.entity.gameObject.SetActive(true);

        SubscribeOnExpire();

    }

    public void DeleteTopCell() {
        UpdateTopCell();


        if (transform.childCount <= 1) return;

        if (cells.Count >= 3) {
            cells[cells.Count - 2].entity.gameObject.SetActive(true);
        }

        Cell cell = cells[cells.Count - 1];

        if (!Application.isPlaying) {
            EditorUtility.SetDirty(cell.gameObject);

            EditorSceneManager.MarkSceneDirty(gameObject.scene);
            EditorSceneManager.SaveOpenScenes();
        }

        cells.Remove(cell);

        DestroyImmediate(transform.GetChild(transform.childCount - 1).gameObject);


    }

    public void AddCell(EntityType type, EntityColor color) {
        UpdateTopCell();


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


        if (!Application.isPlaying) {
            EditorUtility.SetDirty(topCell.gameObject);

            EditorSceneManager.MarkSceneDirty(gameObject.scene);
            EditorSceneManager.SaveOpenScenes();
        }
    }

}
