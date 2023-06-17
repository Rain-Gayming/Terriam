using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameOnLoad : MonoBehaviour
{
    void Update()
    {
        Launcher.Instance.StartGame();
        Destroy(this);       
    }
}
