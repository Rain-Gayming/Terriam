using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ItemInfo : ScriptableObject
{
		
	[BoxGroup("Display Info")]
	public string itemName;
		
	[BoxGroup("Display Info")]
	[PreviewField(50, ObjectFieldAlignment.Left)]public Sprite icon;
	[BoxGroup("Display Info")]
	public bool stackable;
	[BoxGroup("Display Info")]
	public List<InfoPannel> infoDisplays;
	[BoxGroup("Display Info")]
	public float weight;
	[BoxGroup("Display Info")]
	public int value;

	[Button("Set Info")]
	public virtual void SetInfo()
	{
		infoDisplays.Clear();	
		infoDisplays.Add(new InfoPannel("Name: ", name));
		infoDisplays.Add(new InfoPannel("Weight: ", weight.ToString()));
		infoDisplays.Add(new InfoPannel("Value: ", value.ToString()));
	}
}