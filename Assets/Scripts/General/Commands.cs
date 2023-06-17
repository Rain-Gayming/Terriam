using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using QFSW.QC;

public class Commands : MonoBehaviour
{
    public PhotonView view;
    public PlayerController controller;
    public UsernameDisplay usernameDisplay;
    public Vector3 devRoomArea;


    [Command]
    public string UserName(string input)
    {
        if(view.IsMine){
            return usernameDisplay.playerPV.Owner.NickName = input;
        }
        return null;
    }
    
    [Command]
    public bool IsServer(bool server)
    {
        if(view.IsMine){
            return controller.isServer = server;
        }
        return false;
    }
    [Command]
    public void PlaceAtDevRoom()
    {
        if(view.IsMine){
            transform.position = new Vector3(devRoomArea.x, devRoomArea.y + 5, devRoomArea.z);
        }
    }
}
