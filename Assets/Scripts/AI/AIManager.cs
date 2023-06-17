using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Photon.Pun;

public class AIManager : MonoBehaviour, IDamageable
{
    
    [BoxGroup("References")]
    public PlayerManager playerManager;
    [BoxGroup("References")]
    public PhotonView photonView;
    [BoxGroup("References")]
    public Slider healthBar;    
    [BoxGroup("References")]
    public NavMeshAgent agent;
    [BoxGroup("References")]
    public bool inSpawn;

    [BoxGroup("Health")]
    public float currentHealth;
    [BoxGroup("Health")]
    public float maxHealth;

    
    [BoxGroup("Combat")]
    public float attackTime;
    
    [BoxGroup("Combat")]
    public GameObject[] players;
    [BoxGroup("Combat")]
    public Transform target;

    private void Start() {
        healthBar.maxValue = maxHealth;
        currentHealth = maxHealth;
        playerManager = FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
		healthBar.value = currentHealth;
        //Updates the players when one joins or leaves
        if(players != GameObject.FindGameObjectsWithTag("Player")){
            players = GameObject.FindGameObjectsWithTag("Player");
            //Debug.Log("Updating Players");
        }


        if(target == null && !inSpawn){
            target = GetClosestPlayer(players).transform;
        }else{
            
            float targetDiff = Vector3.Distance(transform.position, target.position);
            if(targetDiff > agent.stoppingDistance){

                agent.SetDestination(target.position);
            }else{
                StartCoroutine(AttackCo());
            }
        }
    }	

    public IEnumerator AttackCo()
    {
        yield return new WaitForSeconds(attackTime);
        Debug.Log("Player (" + target.GetComponent<PhotonView>().Owner.NickName +  ") is close enough to " + gameObject.name + " to be attacked"); 
    }

    GameObject GetClosestPlayer(GameObject[] players)
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in players)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }


    public void TakeDamage(float damage)
	{
		photonView.RPC(nameof(RPC_TakeDamage), photonView.Owner, damage);
	}

	[PunRPC]
	void RPC_TakeDamage(float damage, PhotonMessageInfo info)
	{
		currentHealth -= damage;

		healthBar.value = currentHealth;
        healthBar.maxValue = maxHealth;
        Debug.Log(gameObject.name + " Is taking damage: " + damage.ToString()); 

		if(currentHealth <= 0){
            Destroy(gameObject);
			PlayerManager.Find(info.Sender).GetKill();
		}
	}
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "AI Spawn Zone"){
            inSpawn = true;
        }    
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "AI Spawn Zone"){
            inSpawn = false;
        }    
    }
}
