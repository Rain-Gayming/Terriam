using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class WeaponItem : Item
{
	public abstract override void Use();

	[BoxGroup("Weapon")]
	public GameObject bulletImpactPrefab;
	[BoxGroup("Weapon")]
	public int ammo;
}
