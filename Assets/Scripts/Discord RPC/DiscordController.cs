using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Discord;

public class DiscordController : MonoBehaviour
{
    public static DiscordController instance;
    public Discord.Discord discord;
    public RPCSettings rpc;
    public bool hasDiscord;
    RPCSettings preRPC;
    public ActivityManager activityManager;
    // Start is called before the first frame update
    void Start()
    {
        if(hasDiscord){

            instance = this;
            if(SceneManager.GetActiveScene().name == "Dev Area"){
                rpc.details = "Rains working on the game";
                rpc.state = "Its probably not working";
            }else if(SceneManager.GetActiveScene().name == "Main Menu"){
                rpc.details = "Is in the main menu";
                rpc.state = "Its probably not working";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(hasDiscord){       
            if(rpc != preRPC){
                discord = new Discord.Discord(1050570973046374420, (System.UInt64)Discord.CreateFlags.Default);
                activityManager = discord.GetActivityManager();       
                preRPC = rpc;
                var activity = new Discord.Activity{
                Details = rpc.details,
                State = rpc.state,
                Assets = {
                    LargeImage = "984097838269079562",
                }
            };
            activityManager.UpdateActivity(activity, (res) =>{
                if(res == Discord.Result.Ok){
                    Debug.Log("Discord Status Set");
                }else{
                    Debug.LogError("Discord Status Failed");
                }
            });   
        }

        discord.RunCallbacks();
        }

    }
    private void OnDisable()
    {
        discord?.Dispose();
        discord = null;    
    }
}

[System.Serializable]
public class RPCSettings
{
    public string details;
    public string state;
}