using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using Photon.Pun;
using Sirenix.OdinInspector;

public class AISpawner : MonoBehaviour
{
    [BoxGroup("References")]
    public GameObject[] spawnables;
    [BoxGroup("References")]
	public GameObject graphics;
    [BoxGroup("References")]
    public Transform barrierPoint;
    float timer;
    [BoxGroup("Time")]
    float time;
    [BoxGroup("Time")]
    public float minTime;
    [BoxGroup("Time")]
    public float maxTime;

    [BoxGroup("Spawning")]
    public bool canSpawn;
    [BoxGroup("Spawning")]
    public bool isBoss;

    // Start is called before the first frame update
    void Start()
    {
		graphics.SetActive(false);
        time = Random.Range(1, 8);
        timer = time;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0 && canSpawn){
            int ai = Random.Range(0, spawnables.Length);
            GameObject newAI = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", spawnables[ai].name), this.transform.position, Quaternion.identity);
            time = Random.Range(minTime, maxTime);
            timer = time;
            newAI.GetComponent<AIManager>().target = barrierPoint;
        }
    }
}
