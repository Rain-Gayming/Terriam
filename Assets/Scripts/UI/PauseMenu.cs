using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PauseMenu : MonoBehaviour
{
    public List<GameObject> menus;
    public GameObject gameUI;
    public PhotonView photonView;
    public PlayerController controller;
    
    private void Start() {
        Resume();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && controller.paused)
        {
            Resume();
        }    
        if(PhotonNetwork.IsConnected == false){
            SceneManager.LoadScene(0);
        }
    }

    public void Resume()
    {
        for (int i = 0; i < menus.Count; i++)
        {
            menus[i].SetActive(false);
        }
        gameUI.SetActive(true);
        controller.paused = false;
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
