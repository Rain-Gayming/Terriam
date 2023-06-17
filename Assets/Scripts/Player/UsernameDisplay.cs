using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsernameDisplay : MonoBehaviour
{
	public PhotonView playerPV;
	public TMP_Text userNameDisplay;

	void Start()
	{
		if(playerPV.IsMine)
		{
			gameObject.SetActive(false);
		}
	}
	private void Update() {
		if(userNameDisplay.text != playerPV.Owner.NickName){
			userNameDisplay.text = playerPV.Owner.NickName;
		}
	}
}
