﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviour
{
	PhotonView PV;

	public GameObject controller;

	int kills;
	int deaths;
	Transform lastSpawnpoint;
	Transform spawnpoint;
	void Awake()
	{
		PV = GetComponent<PhotonView>();
	}

	void Start()
	{
		if(PV.IsMine)
		{
			CreateController();
		}
	}

	void CreateController()
	{
		if(lastSpawnpoint == null){
			
			spawnpoint = SpawnManager.Instance.GetSpawnpoint();
			lastSpawnpoint = spawnpoint;
		}else{
			spawnpoint = SpawnManager.Instance.GetSpawnpoint();
			if(spawnpoint == lastSpawnpoint){
				Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
			lastSpawnpoint = spawnpoint;
			}
			lastSpawnpoint = spawnpoint;
		}
		controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
	}

	public void Die()
	{
		PhotonNetwork.Destroy(controller);
		CreateController();

		deaths++;

		Hashtable hash = new Hashtable();
		hash.Add("deaths", deaths);
		PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
	}

	public void GetKill()
	{
		PV.RPC(nameof(RPC_GetKill), PV.Owner);
	}

	[PunRPC]
	void RPC_GetKill()
	{
		kills++;

		Hashtable hash = new Hashtable();
		hash.Add("kills", kills);
		PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
	}

	public static PlayerManager Find(Player player)
	{
		return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.PV.Owner == player);
	}
}