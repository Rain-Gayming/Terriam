using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Items/Armour")]
public class ArmourInfo : ItemInfo
{
    [BoxGroup("Armour Info")]
    public float armourValue;
    [BoxGroup("Armour Info")]
    public armourArea armourArea;
    [BoxGroup("Armour Info")]
    public armourType armourType;
    
    public override void SetInfo()
	{
		base.SetInfo();
		
		infoDisplays.Add(new InfoPannel("Armour: ", armourValue.ToString()));
		infoDisplays.Add(new InfoPannel("Type: ", armourType.ToString()));
		infoDisplays.Add(new InfoPannel("Area: ", armourArea.ToString()));
	}

}

public enum armourType
{
    light,
    medium,
    heavy
}
public enum armourArea
{
    head,
    face,
    chest,
    leftArm,
    rightArm,
    leftLeg,
    rightLeg
}
