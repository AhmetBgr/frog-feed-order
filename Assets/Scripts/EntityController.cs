using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    public EntityView view;
    public EntityModal modal;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        modal.view = view;   
    }
    
}
