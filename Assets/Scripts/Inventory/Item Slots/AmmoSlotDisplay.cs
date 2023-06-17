using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class AmmoSlotDisplay : ItemSlotDisplay
{
    [BoxGroup("Items")]
    public AmmoItem ammoInfo;

    [BoxGroup("Display")]
    public TMP_Text itemDamageText;

    private void Start() {

    }

    private void Update() {
        if(item != ammoInfo.ammoInfo){
            item = ammoInfo.ammoInfo;
            UpdateInfo();
        }
        if(ammoInfo.amount > 1){
            itemNameText.text = item.itemName + "(" + ammoInfo.amount.ToString() + ")";
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
        itemDamageText.text = ammoInfo.ammoInfo.damage.ToString();
    }
}
