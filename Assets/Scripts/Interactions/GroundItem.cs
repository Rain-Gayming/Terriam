using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : Interactable
{
    public AmmoItem ammoItem;
    public ConsumableItem consumableItem;
    public MiscItem miscItem;
    public JunkItem junkItem;
    public ArmourInfo armourInfo;
    public WeaponInfo weaponInfo;

    public override void Interact(GameObject interactedWith)
    {
        destroy = true;
        base.Interact(interactedWith);
    
        if(ammoItem.ammoInfo){
            interactedWith.GetComponent<PlayerInventory>().AddAmmoItem(ammoItem);
        }
        if(consumableItem.consumableInfo){
            interactedWith.GetComponent<PlayerInventory>().AddConsumableItem(consumableItem);
        }
        if(miscItem.miscInfo){
            interactedWith.GetComponent<PlayerInventory>().AddMiscItem(miscItem);
        }
        if(junkItem.junkInfo){
            interactedWith.GetComponent<PlayerInventory>().AddJunkItem(junkItem);
        }
        if(weaponInfo){
            interactedWith.GetComponent<PlayerInventory>().AddWeaponItem(weaponInfo);
        }
        if(armourInfo){
            interactedWith.GetComponent<PlayerInventory>().AddArmourItem(armourInfo);
        }            
    }
    
}
