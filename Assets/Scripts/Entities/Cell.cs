using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cell : MonoBehaviour {
    public EntityModal entity;

    public void AnimateScale() {
        transform.DOScale(0f, 0.5f); //.OnComplete(() => entity.transform.SetParent(transform));
    }
}
