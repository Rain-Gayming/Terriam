using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Items/Ammo")]
public class AmmoInfo : ItemInfo
{
    [BoxGroup("Ammo Info")]
    public float velocity;
    [BoxGroup("Ammo Info")]
    public int damage;
    [BoxGroup("Ammo Info")]
    public AmmoCaliber caliber;
    public override void SetInfo()
	{
		base.SetInfo();
		
		infoDisplays.Add(new InfoPannel("Caliber: ", caliber.ToString()));
		infoDisplays.Add(new InfoPannel("Damage: ", damage.ToString()));
		infoDisplays.Add(new InfoPannel("Velocity: ", velocity.ToString()));
	}
}

[System.Serializable]
public class AmmoItem
{
    public AmmoInfo ammoInfo;
    public int amount;
}

public enum AmmoCaliber
{

}
