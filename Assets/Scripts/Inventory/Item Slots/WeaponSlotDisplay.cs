using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class WeaponSlotDisplay : ItemSlotDisplay
{
    [BoxGroup("Items")]
    public WeaponInfo weaponItem;

    [BoxGroup("Display")]
    public TMP_Text itemDamageText;

    private void Start() {

    }

    private void Update() {
        if(item != weaponItem){
            item = weaponItem;
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
        itemDamageText.text = weaponItem.damage.ToString();
    }

    public override void UseItem()
    {
        GetComponentInParent<PlayerController>().EquipWeapon(weaponItem);
        Debug.Log("Equipping " + weaponItem.itemName);
    }
}
