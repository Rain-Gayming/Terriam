using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Items/Junk")]
public class JunkInfo : ItemInfo
{
    [BoxGroup("Junk Info")]
    public List<JunkItem> turnsInto;

    public override void SetInfo()
    {
        base.SetInfo();
        for (int i = 0; i < turnsInto.Count; i++)
        {
		    infoDisplays.Add(new InfoPannel("Made Of: ", turnsInto[i].junkInfo.itemName.ToString() + "( "+ turnsInto[i].amount + " )"));            
        }
    }

}

[System.Serializable]
public class JunkItem
{
    public JunkInfo junkInfo;
    public int amount;
}