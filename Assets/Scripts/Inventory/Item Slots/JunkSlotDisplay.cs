using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class JunkSlotDisplay : ItemSlotDisplay
{
    [BoxGroup("Items")]
    public JunkItem junkInfo;


    private void Start() {

    }

    private void Update() {
        if(item != junkInfo.junkInfo){
            item = junkInfo.junkInfo;
            UpdateInfo();
        }
        if(junkInfo.amount > 1){
            itemNameText.text = item.itemName + "(" + junkInfo.amount.ToString() + ")";
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
