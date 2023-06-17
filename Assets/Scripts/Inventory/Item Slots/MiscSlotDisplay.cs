using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class MiscSlotDisplay : ItemSlotDisplay
{
    [BoxGroup("Items")]
    public MiscItem miscInfo;


    private void Start() {

    }

    private void Update() {
        if(item != miscInfo.miscInfo){
            item = miscInfo.miscInfo;
            UpdateInfo();
        }
        if(miscInfo.amount > 1){
            itemNameText.text = item.itemName + "(" + miscInfo.amount.ToString() + ")";
        }
        
        if(hovered && shouldDisplay){
            GetComponentInParent<InventoryInfoManager>().DisplayInfo(item);
            shouldDisplay = false;
        }else if(!hovered){
            shouldDisplay = true;
        }
    }

    public override void UpdateInfo()
    {
        base.UpdateInfo();
    }
}
