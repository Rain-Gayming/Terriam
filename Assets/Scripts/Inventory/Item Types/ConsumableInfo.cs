using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Items/Consumable")]
public class ConsumableInfo : ItemInfo
{

    [BoxGroup("Consumable Info")]
    public HealType healType;
    [BoxGroup("Consumable Info/Health")]
    public float health;
    [BoxGroup("Consumable Info/Health")]
    public LimbRestoreType limbRestoreType;
    [BoxGroup("Consumable Info/Health")]
    public bool restoresLimb;
    [BoxGroup("Consumable Info/Radiation")]
    public bool radiationChange;
    [BoxGroup("Consumable Info/Radiation")]
    public float radiation;
    [BoxGroup("Consumable Info/Thirst")]
    public bool thirstChange;
    [BoxGroup("Consumable Info/Thirst")]
    public float thirst;
    [BoxGroup("Consumable Info/Hunger")]
    public bool hungerChange;
    [BoxGroup("Consumable Info/Hunger")]
    public float hunger;

    
	public override void SetInfo()
	{
		base.SetInfo();
		
        infoDisplays.Add(new InfoPannel("Health: ", health.ToString()));
		if(restoresLimb){
		    infoDisplays.Add(new InfoPannel("Type: ", limbRestoreType.ToString()));
        }
        if(radiationChange){
		    infoDisplays.Add(new InfoPannel("Radiation: ", radiation.ToString()));
        }
        if(thirstChange){
		    infoDisplays.Add(new InfoPannel("Thirst: ", thirst.ToString()));
        }
        if(hungerChange){
		    infoDisplays.Add(new InfoPannel("Hunger: ", hunger.ToString()));
        }
	}
}


[System.Serializable]
public class ConsumableItem
{
    public ConsumableInfo consumableInfo;
    public int amount;
}

public enum LimbRestoreType
{
    all,
    leg,
    arm,
    chest,
    painKiller,
}
public enum HealType
{
    health,
    thirst,
    hunger,
    radiation
}