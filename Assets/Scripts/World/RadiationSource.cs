using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiationSource : MonoBehaviour
{
    public float radiation;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.GetComponent<PlayerController>())
        {
            other.gameObject.GetComponent<PlayerController>().radiation = radiation;
            other.gameObject.GetComponent<PlayerController>().activeRadiation = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {        
        if(other.gameObject.GetComponent<PlayerController>()){
            other.gameObject.GetComponent<PlayerController>().radiation = 0;
            other.gameObject.GetComponent<PlayerController>().activeRadiation = false;
        }
    }
}
