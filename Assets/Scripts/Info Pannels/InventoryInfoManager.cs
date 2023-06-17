using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Photon.Pun;

public class InventoryInfoManager : MonoBehaviour
{
    public PhotonView photonView;
    [BoxGroup("References")]
    public Transform infoGrid;
    [BoxGroup("References")]
    public List<GameObject> infoPannels;
    [BoxGroup("References")]
    public GameObject infoPannelPrefab;

    private void Start() {
        if(photonView.IsMine == false){
            //Destroy(this);
        }
    }

    public virtual void DisplayInfo(ItemInfo info)
    {
        for (int i = 0; i < infoPannels.Count; i++)
        {
            Destroy(infoPannels[i]);
        }
        infoPannels.Clear();
        for (int i = 0; i < info.infoDisplays.Count; i++)
        {
            GameObject newPannel = Instantiate(infoPannelPrefab);
            newPannel.GetComponent<InfoPannelDisplay>().info = info.infoDisplays[i];
            newPannel.transform.SetParent(infoGrid);
            newPannel.transform.localScale = Vector3.one;
            newPannel.transform.localPosition = Vector3.zero;
            infoPannels.Add(newPannel);
        }
    }
}
