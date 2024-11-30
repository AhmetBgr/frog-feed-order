using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cell : MonoBehaviour, IPoolableObject {
    public EntityController entity;

    private Node node;

    private void OnEnable() {
        if (!entity) return;

        GameManager.onGameOver += HandleOnExpire;

        entity.OnExpire += RemoveSelf;

    }

    private void OnDisable() {

        if (!entity) return;

        GameManager.onGameOver -= HandleOnExpire;


        entity.OnExpire -= RemoveSelf;

    }

    public virtual void HandleOnExpire() {
        if (!entity) return;

        // Make sure entites has parent as this cell
        entity.transform.SetParent(transform);

        StartCoroutine(entity.TriggerOnExpire(0.5f));

    }

    private void RemoveSelf() {
        node.RemoveTopCell();
        AnimateScale();
    }

    public void AnimateScale() {
        transform.DOScale(0f, 0.3f).OnComplete(() => {
            ReturnToPool();
        });
    }

    public void OnObjectSpawn() {
        node = GetComponentInParent<Node>();
        

        transform.localScale = Vector3.one;
        gameObject.SetActive(true);

        entity.transform.SetParent(transform);

        entity.transform.localScale = Vector3.one;
        entity.transform.localPosition = new Vector3(0f, entity.transform.localPosition.y, 0f);

        entity.gameObject.SetActive(true);

        entity.OnSpawn();
    }

    public void ReturnToPool() {
        ObjectPooler.instance.AddToPool(name, gameObject);
    }

}
