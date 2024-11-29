using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cell : MonoBehaviour, IPoolableObject {
    public EntityModal entity;

    private Node node;

    private void Start() {
        node = GetComponentInParent<Node>();
    }

    private void OnEnable() {
        if (!entity) return;

        entity.OnExpire += RemoveSelf;

    }

    private void OnDisable() {

        if (!entity) return;

        entity.OnExpire -= RemoveSelf;

    }

    private void RemoveSelf() {
        node.RemoveTopCell();
        AnimateScale();
    }

    public void AnimateScale() {
        transform.DOScale(0f, 0.5f).OnComplete(() => {
            ReturnToPool();
            //Invoke("ReturnToPool", 3f);
            //gameObject.SetActive(false);
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

        /*transform.localScale = Vector3.one;
        transform.localPosition = new Vector3(0f, 0f, 0f);
        transform.localEulerAngles = Vector3.zero;
        gameObject.SetActive(true);

        entity.transform.SetParent(transform);

        entity.transform.localScale = Vector3.one;
        entity.transform.localPosition = new Vector3(0f, entity.transform.localPosition.y, 0f);

        entity.gameObject.SetActive(true);*/

        ObjectPooler.instance.AddToPool(name, gameObject);
    }

}
