using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ConsumableSlotDisplay : ItemSlotDisplay
{
    [BoxGroup("Display")]
    public TMP_Text useText;
    [BoxGroup("Display")]
    public TMP_Text typeText;

    [BoxGroup("Items")]
    public ConsumableItem consumableItem;


    private void Start() {
    }

    private void Update() {
        if(consumableItem.consumableInfo != item)
        {
            item = consumableItem.consumableInfo;
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
        if(consumableItem.amount <= 0){
            GetComponentInParent<PlayerInventory>().consumableSlots.Remove(this.gameObject);
            GetComponentInParent<PlayerInventory>().consumableItems.Remove(consumableItem);
            Destroy(gameObject);
        }
        for (int i = 0; i < GetComponentInParent<PlayerInventory>().consumableItems.Count; i++)
        {
            if(GetComponentInParent<PlayerInventory>().consumableItems[i].consumableInfo == consumableItem.consumableInfo){
                GetComponentInParent<PlayerInventory>().consumableItems[i].amount = consumableItem.amount;
            }
        }
        if(consumableItem.amount > 1){
            itemNameText.text = item.itemName + "(" + consumableItem.amount.ToString() + ")";
        }
        if(consumableItem.consumableInfo.restoresLimb)
            typeText.text = consumableItem.consumableInfo.healType.ToString();
        else
            typeText.gameObject.SetActive(false);
        switch (consumableItem.consumableInfo.healType)
        {
            case HealType.health:
                useText.text = consumableItem.consumableInfo.health.ToString();
            break;
            case HealType.thirst:
                useText.text = consumableItem.consumableInfo.thirst.ToString();
            break;
            case HealType.hunger:
                useText.text = consumableItem.consumableInfo.hunger.ToString();
            break;
            case HealType.radiation:
                useText.text = consumableItem.consumableInfo.radiation.ToString();
            break;
        }
        
    }

    public override void UseItem()
    {
        consumableItem.amount--;   
        GetComponentInParent<PlayerController>().currentHealth += consumableItem.consumableInfo.health;
        GetComponentInParent<PlayerController>().TakePassiveRadiation(consumableItem.consumableInfo.radiation);
        UpdateInfo();
    }
}
