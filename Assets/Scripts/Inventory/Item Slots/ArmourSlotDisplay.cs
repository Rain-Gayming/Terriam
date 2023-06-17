using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class ArmourSlotDisplay : ItemSlotDisplay
{
    [BoxGroup("Items")]
    public ArmourInfo armourItem;

    [BoxGroup("Display")]
    public TMP_Text itemArmourText;
    [BoxGroup("Display")]
    public TMP_Text armourTypeText;

    private void Start() {

    }

    private void Update() {
        if(item != armourItem){
            item = armourItem;
            UpdateInfo();
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
        itemArmourText.text = armourItem.armourValue.ToString();
        armourTypeText.text = armourItem.armourType.ToString();
    }

    public override void UseItem()
    {
        Debug.Log("Equipping " + armourItem.itemName);
    }
}
