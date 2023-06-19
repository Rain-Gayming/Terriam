using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class InteractionCamera : MonoBehaviour
{
    public PlayerInputs inputs;
    [BoxGroup("References")]
    public InputManager inputManager;
    [BoxGroup("References")]
    public GameObject interactObject;
    [BoxGroup("References")]
    public TMP_Text textObject;
    [BoxGroup("References")]
    public TMP_Text interactKey;

    [BoxGroup("Interaction Info")]
    public Transform interactPoint;
    [BoxGroup("Interaction Info")]
    public float interactRange;
    private void Start() {
        inputs = new PlayerInputs();
    }
    private void Update() {
        RaycastHit hit;
        if(Physics.Raycast(interactPoint.position, interactPoint.forward, out hit, interactRange)){
            if(hit.transform.GetComponent<Interactable>()){
                interactObject.SetActive(true);
                 string pth;
                if(inputs.Keyboard.Interact.bindings[1].overridePath != null)
                    pth = inputs.Keyboard.Interact.bindings[1].overridePath;
                else                    
                    pth = inputs.Keyboard.Interact.bindings[0].path;
                int length = pth.Length;
                int lengthResult = length -= 11;
                string result = pth.Substring(11, lengthResult);
                pth = result;
                interactKey.text = pth + ") Interact";
#region  Ground Item Text
                if(hit.transform.GetComponent<GroundItem>().ammoItem.ammoInfo)
                {
                    textObject.text = hit.transform.GetComponent<GroundItem>().ammoItem.ammoInfo.itemName +
                         "("+ hit.transform.GetComponent<GroundItem>().ammoItem.amount +")";
                }
                if(hit.transform.GetComponent<GroundItem>().miscItem.miscInfo)
                {
                    textObject.text = hit.transform.GetComponent<GroundItem>().miscItem.miscInfo.itemName +
                         "("+ hit.transform.GetComponent<GroundItem>().miscItem.amount +")";
                }
                if(hit.transform.GetComponent<GroundItem>().consumableItem.consumableInfo)
                {
                    textObject.text = hit.transform.GetComponent<GroundItem>().consumableItem.consumableInfo.itemName +
                         "("+ hit.transform.GetComponent<GroundItem>().consumableItem.amount +")";
                }
                if(hit.transform.GetComponent<GroundItem>().junkItem.junkInfo)
                {
                    textObject.text = hit.transform.GetComponent<GroundItem>().junkItem.junkInfo.itemName +
                         "("+ hit.transform.GetComponent<GroundItem>().junkItem.amount +")";
                }
                if(hit.transform.GetComponent<GroundItem>().weaponInfo)
                {
                    textObject.text = hit.transform.GetComponent<GroundItem>().weaponInfo.itemName;
                }
                if(hit.transform.GetComponent<GroundItem>().armourInfo)
                {
                    textObject.text = hit.transform.GetComponent<GroundItem>().armourInfo.itemName;
                }
#endregion
                
                if(inputManager.interact)
                    hit.transform.GetComponent<Interactable>().Interact(gameObject);
                inputManager.interact = false;
            }else{
                interactObject.SetActive(false);
            }
        }else{
                interactObject.SetActive(false);
        }
    }
}
