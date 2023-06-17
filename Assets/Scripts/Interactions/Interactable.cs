using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool destroy;
    public virtual void Interact(GameObject interactedWith)
    {
        if(destroy){
            Destroy(gameObject);
        }
    }
}
