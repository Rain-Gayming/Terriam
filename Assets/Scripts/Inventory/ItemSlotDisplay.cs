using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlotDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [BoxGroup("Display")]
    public Image icon;
    [BoxGroup("Display")]
    public TMP_Text itemNameText;
    [BoxGroup("Display")]
    public TMP_Text itemValueText;
    [BoxGroup("Display")]    
    public TMP_Text itemWeightText;

    [BoxGroup("Hovered")]    
    public bool hovered;
    [BoxGroup("Hovered")]    
    public bool shouldDisplay;

    [BoxGroup("Items")]
    public ItemInfo item;


    private void Update() {
        if(item == null){
            Destroy(gameObject);
        }

        if(hovered && shouldDisplay){
            GetComponentInParent<InventoryInfoManager>().DisplayInfo(item);
            shouldDisplay = false;
        }else{
            shouldDisplay = true;
        }

        if(Input.GetKeyDown(KeyCode.Mouse0)){
            if(hovered){
                UseItem();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
        shouldDisplay = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
        shouldDisplay = false;
    }

    public virtual void UpdateInfo()
    {
        icon.sprite = item.icon;
        itemNameText.text = item.itemName;
        itemValueText.text = item.value.ToString();
        itemWeightText.text = item.weight.ToString();
    }

    public virtual void UseItem()
    {
        
    }
}
