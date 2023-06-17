using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponInfo : ItemInfo
{

	[BoxGroup("Weapon Info")]
	public GameObject weaponObject;
	[BoxGroup("Weapon Info/Stats")]
	public float damage;
	[BoxGroup("Weapon Info/Stats")]
	public int ammo;
	[BoxGroup("Weapon Info/Stats")]
	public float reloadSpeed;
	[BoxGroup("Weapon Info/Stats")]
	public float fireRate;
	[BoxGroup("Weapon Info/Stats")]
	public bool automatic;

	public override void SetInfo()
	{
		base.SetInfo();
		
		infoDisplays.Add(new InfoPannel("Ammo: ", ammo.ToString()));
		infoDisplays.Add(new InfoPannel("Damage: ", damage.ToString()));
		infoDisplays.Add(new InfoPannel("Fire Rate: ", fireRate.ToString() + "ms"));
	}


	
}