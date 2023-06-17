using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class Item : MonoBehaviour
{
	[BoxGroup("Item")]
	public ItemInfo itemInfo;
	[BoxGroup("Item")]
	public GameObject itemGameObject;

	public abstract void Use();
}