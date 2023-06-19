using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Photon.Pun;

public class PlayerInventory : MonoBehaviour
{
    [BoxGroup("Components")]
    public InputManager inputManager;
    [BoxGroup("Components")]
    public GameObject inventoryObject;
    [BoxGroup("Components")]
    public PhotonView photonView;
    [BoxGroup("Components")]
    public PlayerController playerController;

#region  Item Lists
    [BoxGroup("Item Lists")]
    [BoxGroup("Item Lists/Weapons")]
    public List<WeaponInfo> weaponItems;
    [BoxGroup("Item Lists/Weapons")]
    public List<GameObject> weaponSlots;
    [BoxGroup("Item Lists/Armour")]
    public List<ArmourInfo> armourItems;
    [BoxGroup("Item Lists/Armour")]
    public List<GameObject> armourSlots;
    [BoxGroup("Item Lists/Consumable")]
    public List<ConsumableItem> consumableItems;
    [BoxGroup("Item Lists/Consumable")]
    public List<GameObject> consumableSlots;
    [BoxGroup("Item Lists/Ammo")]
    public List<AmmoItem> ammoItems;
    [BoxGroup("Item Lists/Ammo")]
    public List<GameObject> ammoSlots;
    [BoxGroup("Item Lists/Junk")]
    public List<JunkItem> junkItems;
    [BoxGroup("Item Lists/Junk")]
    public List<GameObject> junkSlots;
    [BoxGroup("Item Lists/Misc")]
    public List<MiscItem> miscItems;
    [BoxGroup("Item Lists/Misc")]
    public List<GameObject> miscSlots;
#endregion
#region  slots
    [BoxGroup("Item Slots")]
    [BoxGroup("Item Slots/Weapons")]
    public GameObject weaponSlot;
    [BoxGroup("Item Slots/Weapons")]
    public Transform weaponGrid;

    [BoxGroup("Item Slots/Armour")]
    public GameObject armourSlot;
    [BoxGroup("Item Slots/Armour")]
    public Transform armourGrid;

    [BoxGroup("Item Slots/Food")]
    public Transform foodGrid;

    [BoxGroup("Item Slots/Drinks")]
    public Transform drinksGrid;

    [BoxGroup("Item Slots/Medical")]
    public GameObject consumableSlot;
    [BoxGroup("Item Slots/Medical")]
    public Transform medicalGrid;
    
    [BoxGroup("Item Slots/Ammo")]
    public GameObject ammoSlot;
    [BoxGroup("Item Slots/Ammo")]
    public Transform ammoGrid;

    [BoxGroup("Item Slots/Junk")]
    public GameObject junkSlot;
    [BoxGroup("Item Slots/Junk")]
    public Transform junkGrid;

    [BoxGroup("Item Slots/Misc")]
    public GameObject miscSlot;
    [BoxGroup("Item Slots/Misc")]
    public Transform miscGrid;
#endregion

    [BoxGroup("Testing")]
    public WeaponInfo testWeaponItem;
    


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(inputManager.inventory){
            if(inventoryObject.activeInHierarchy){
                inventoryObject.SetActive(false);
                playerController.paused =  false;
            }else{
                inventoryObject.SetActive(true);
                playerController.paused =  true;
            }
            playerController.pauseUI.SetActive(false);
            inputManager.inventory = false;
        }

        if(Input.GetKeyDown(0)){
            AddWeaponItem(testWeaponItem);
        }
    }

    public void AddWeaponItem(WeaponInfo item)
    {
        if(!photonView.IsMine)  
            return;
        GameObject newWeaponSlot = Instantiate(weaponSlot);
        newWeaponSlot.transform.SetParent(weaponGrid);
        newWeaponSlot.transform.localScale = Vector3.one;
        newWeaponSlot.GetComponent<WeaponSlotDisplay>().weaponItem = item;
        newWeaponSlot.transform.localRotation = Quaternion.identity;
        weaponSlots.Add(newWeaponSlot);
        weaponItems.Add(item);
    }
    public void AddArmourItem(ArmourInfo item)
    {
        if(!photonView.IsMine)  
            return;
        GameObject newArmourSlot = Instantiate(armourSlot);
        newArmourSlot.transform.SetParent(armourGrid);
        newArmourSlot.transform.localScale = Vector3.one;
        newArmourSlot.GetComponent<ArmourSlotDisplay>().armourItem = item;
        newArmourSlot.transform.localRotation = Quaternion.identity;
        armourSlots.Add(newArmourSlot);
        armourItems.Add(item);
    }
#region  Consumable  
    public void AddConsumableItem(ConsumableItem item)
    {
        if(!photonView.IsMine)  
            return;
        
        int itemsCounted = 0;
        for (int i = 0; i < consumableItems.Count; i++)
        {
            if(consumableItems[i].consumableInfo == item.consumableInfo){

            if(consumableItems.Count == 0){               
                AddNewConsumableItem(item);
            }
            }else{
                itemsCounted++;
            }
            if(itemsCounted > consumableItems.Count){

                if(consumableItems.Count == 0){           
                    AddNewConsumableItem(item);
                }
            }
        }

        if(consumableItems.Count == 0){
            AddNewConsumableItem(item);
        }
    }

    public void AddNewConsumableItem(ConsumableItem item)
    {
        
        GameObject newConsumableSlot = Instantiate(consumableSlot);

        switch (item.consumableInfo.healType)
        {
            case HealType.health:
                newConsumableSlot.transform.SetParent(medicalGrid);
            break;
            case HealType.radiation:
                newConsumableSlot.transform.SetParent(medicalGrid);
            break;
            case HealType.hunger:
                newConsumableSlot.transform.SetParent(foodGrid);
            break;
            case HealType.thirst:
                newConsumableSlot.transform.SetParent(drinksGrid);
            break;
        }
        newConsumableSlot.transform.localScale = Vector3.one;
        newConsumableSlot.GetComponent<ConsumableSlotDisplay>().consumableItem.consumableInfo = item.consumableInfo;
        newConsumableSlot.GetComponent<ConsumableSlotDisplay>().consumableItem.amount += item.amount;
        newConsumableSlot.transform.localRotation = Quaternion.identity;
        consumableSlots.Add(newConsumableSlot);
        consumableItems.Add(item);
        
    }
#endregion
#region Ammo
    public void AddAmmoItem(AmmoItem item)
    {
        if(!photonView.IsMine)  
            return;
        
        int itemsCounted = 0;
        for (int i = 0; i < ammoItems.Count; i++)
        {
            if(ammoItems[i] == item){

            if(ammoItems.Count == 0){               
                AddNewAmmoItem(item);
            }
            }else{
                itemsCounted++;
            }
            if(itemsCounted > ammoItems.Count){

                if(ammoItems.Count == 0){           
                    AddNewAmmoItem(item);
                }
            }
        }

        if(ammoItems.Count == 0){
            AddNewAmmoItem(item);
        }
    }

    public void AddNewAmmoItem(AmmoItem item)
    {
        GameObject newAmmoSlot = Instantiate(ammoSlot);

        newAmmoSlot.transform.SetParent(ammoGrid);
        newAmmoSlot.transform.localScale = Vector3.one;
        newAmmoSlot.GetComponent<AmmoSlotDisplay>().ammoInfo.ammoInfo = item.ammoInfo;
        newAmmoSlot.GetComponent<AmmoSlotDisplay>().ammoInfo.amount = item.amount;
        newAmmoSlot.transform.localRotation = Quaternion.identity;
        ammoSlots.Add(newAmmoSlot);
        ammoItems.Add(item);
    }
#endregion
#region  Misc
    public void AddMiscItem(MiscItem item)
    {
        if(!photonView.IsMine)  
            return;
        
        int itemsCounted = 0;
        for (int i = 0; i < miscItems.Count; i++)
        {
            if(miscItems[i] == item){

            if(miscItems.Count == 0){               
                AddNewMiscItem(item);
            }
            }else{
                itemsCounted++;
            }
            if(itemsCounted > miscItems.Count){

                if(miscItems.Count == 0){           
                    AddNewMiscItem(item);
                }
            }
        }

        if(miscItems.Count == 0){
            AddNewMiscItem(item);
        }
    }

    public void AddNewMiscItem(MiscItem item)
    {
        GameObject newAmmoSlot = Instantiate(miscSlot);

        newAmmoSlot.transform.SetParent(miscGrid);
        newAmmoSlot.transform.localScale = Vector3.one;
        newAmmoSlot.GetComponent<MiscSlotDisplay>().miscInfo.miscInfo = item.miscInfo;
        newAmmoSlot.GetComponent<MiscSlotDisplay>().miscInfo.amount += item.amount;
        newAmmoSlot.transform.localRotation = Quaternion.identity;
        miscSlots.Add(newAmmoSlot);
        miscItems.Add(item);
    }
#endregion
#region  junk 
    public void AddJunkItem(JunkItem item)
    {
        if(!photonView.IsMine)  
            return;
        
        int itemsCounted = 0;
        for (int i = 0; i < junkItems.Count; i++)
        {
            if(junkItems[i].junkInfo == item.junkInfo){

                if(consumableItems.Count == 0){               
                    AddNewJunkItem(item);
                }
            }else{
                itemsCounted++;
            }
            if(itemsCounted > junkItems.Count){

                if(consumableItems.Count == 0){           
                    AddNewJunkItem(item);
                }
            }
        }

        if(junkItems.Count == 0){
            AddNewJunkItem(item);
        }
    }

    public void AddNewJunkItem(JunkItem item)
    {
        GameObject newAmmoSlot = Instantiate(junkSlot);

        newAmmoSlot.transform.SetParent(junkGrid);
        newAmmoSlot.transform.localScale = Vector3.one;
        newAmmoSlot.GetComponent<JunkSlotDisplay>().junkInfo.junkInfo = item.junkInfo;
        newAmmoSlot.GetComponent<JunkSlotDisplay>().junkInfo.amount += item.amount;
        newAmmoSlot.transform.localRotation = Quaternion.identity;
        junkSlots.Add(newAmmoSlot);
        junkItems.Add(item);
    }
#endregion
}
