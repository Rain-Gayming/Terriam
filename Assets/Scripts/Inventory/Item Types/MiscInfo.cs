using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Misc")]
public class MiscInfo : ItemInfo
{

}

[System.Serializable]
public class MiscItem
{
    public MiscInfo miscInfo;
    public int amount;
}