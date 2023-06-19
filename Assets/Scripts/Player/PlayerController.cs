using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Sirenix.OdinInspector;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
	[BoxGroup("References")]
	public InputManager inputManager;
	[BoxGroup("References")]
	public GameObject camera;
	[BoxGroup("References")]
	public Rigidbody rb;
	[BoxGroup("References")]	
	public PhotonView PV;
	[BoxGroup("References")]
	public PlayerManager playerManager;
	[BoxGroup("References")]
	public Animator anim;
	
	
	[BoxGroup("UI")]
	public GameObject gameUI; 	
	[BoxGroup("UI")]
	public GameObject pauseUI;
	
	[BoxGroup("UI/Equipment")]
	public TMP_Text equipmentNameText;
	[BoxGroup("UI/Equipment")]
	public TMP_Text equipmentAmountText;

	[BoxGroup("Movements")]
	public float sprintSpeed;
	[BoxGroup("Movements")]
	public float walkSpeed;
	[BoxGroup("Movements")]
	public float jumpForce;
	[BoxGroup("Movements")]
	public float smoothTime;

	
	[BoxGroup("Sensitvity")]
	public float mouseSensitivityX;
	[BoxGroup("Sensitvity")]
	public float mouseSensitivityY;
	
	[BoxGroup("Server")]
	public GameObject[] serverDisabled;
	[BoxGroup("Server")]
	public bool isServer;
	 
	[BoxGroup("Weapon")]
	public WeaponItem primaryItem;
	[BoxGroup("Weapon")]
	public WeaponInfo primaryInfo;
	[BoxGroup("Weapon")]
	public Transform weaponPosition;
	[BoxGroup("Weapon")]
	public GameObject weaponEquippedObject;
	[BoxGroup("Items")]
	public Item equipmentItem;
	
	[BoxGroup("Grounded")]
	public LayerMask groundMask;
	[BoxGroup("Grounded")]
	public float groundDistance;
	[BoxGroup("Grounded")]
	public Transform groundPoint;
	[BoxGroup("Grounded")]
	public bool grounded;

	float verticalLookRotation;
	Vector3 smoothMoveVelocity;
	Vector3 moveAmount;


	[BoxGroup("Survival")]
	[BoxGroup("Survival/Health")]
	public float maxHealth = 100f;
	[BoxGroup("Survival/Health")]
	public float currentMaxHealth;
	[BoxGroup("Survival/Health")]
	public float currentHealth;
	[BoxGroup("Survival/Radiation")]
	public float radiation;
	[BoxGroup("Survival/Radiation")]
	public bool activeRadiation;
	[BoxGroup("Survival/Radiation")]
	public bool canTakeRadiationDamage = true;
	[BoxGroup("Survival/Health")]
	public Image healthSlider;

	[BoxGroup("Settings")]
	public GameObject settingsMenu;
	[BoxGroup("Settings")]
	public GameObject controlsMenu;
	public bool paused;

	void Awake()
	{
		playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
	}

	void Start()
	{
		if(PV.IsMine)
		{
			currentMaxHealth = maxHealth;
			currentHealth = maxHealth;
		}
		else
		{
			Destroy(GetComponentInChildren<Camera>().gameObject);
			//Destroy(rb);
			Destroy(gameUI);
			Destroy(pauseUI);
		}
		StartCoroutine(LoadSettingsCo());
	}

	public IEnumerator LoadSettingsCo()
	{
		settingsMenu.SetActive(true);
		controlsMenu.SetActive(true);
		yield return new WaitForSeconds(0.1f);
		settingsMenu.SetActive(false);
		controlsMenu.SetActive(false);
	}

	void FixedUpdate()
	{
		if(!PV.IsMine)
			return;
		if(inputManager.move != Vector2.zero)
			rb.MovePosition(rb.position + camera.transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
		else
			rb.velocity = new Vector3(0, rb.velocity.y, 0);
	}

	void Update()
	{
		if(!PV.IsMine)
			return;
			
		healthSlider.fillAmount = currentHealth / maxHealth;
		if(currentHealth > currentMaxHealth){
			currentHealth = currentMaxHealth;
		}

		if(!paused && !isServer){
			Look();
			Move();
			Jump();
			
			if(transform.position.y < -50f){
				Die();
			}

			grounded = Physics.CheckSphere(groundPoint.position, groundDistance, groundMask);
	
			if(Input.GetMouseButtonDown(0)){
				primaryItem.Use();
			}
		}else{
			moveAmount = new Vector3(0, 0, 0);
			rb.velocity = Vector3.zero;
		}
		
		if(inputManager.leftLean){
			anim.SetBool("Left Lean", true);
		}else if(inputManager.rightLean){
			anim.SetBool("Right Lean", true);
		}else{
			anim.SetBool("Left Lean", false);
			anim.SetBool("Right Lean", false);
		}



		if(isServer){
			for (int i = 0; i < serverDisabled.Length; i++)
			{
				serverDisabled[i].SetActive(false);
			}
		}else{
			for (int i = 0; i < serverDisabled.Length; i++)
			{
				serverDisabled[i].SetActive(true);
			}
		}
		
		if(paused){
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}else{
			
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.F1) && !paused){
			paused = true;
			inputManager.pause = false;
			pauseUI.SetActive(true);
			gameUI.SetActive(false);	
			GetComponent<PlayerInventory>().inventoryObject.SetActive(false);
		}

		if(activeRadiation && canTakeRadiationDamage){
			StartCoroutine(TakeActiveRadiation(radiation));
		}

		if(currentHealth <= 0){
			Die(); 
		}
	}

	public void EquipWeapon(WeaponInfo weapon)
	{
		if(PV.IsMine == false){
			return;
		}

		Destroy(weaponEquippedObject);
		GameObject newWeapon = PhotonNetwork.Instantiate(Path.Combine("Items",weapon.weaponObject), Vector3.zero, Quaternion.identity);
		newWeapon.transform.parent = weaponPosition.transform;
		weaponEquippedObject = newWeapon;
	}

#region movement
	void Look()
	{
		if(paused || isServer)
			return;
			
		transform.Rotate(Vector3.up * inputManager.mouseMove.x * mouseSensitivityX);

		verticalLookRotation += inputManager.mouseMove.y * mouseSensitivityY;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

		camera.transform.localEulerAngles = Vector3.left * verticalLookRotation;
	}

	void Move()
	{
		if(paused || isServer)
			return;

		Vector3 moveDir = new Vector3(inputManager.move.x, 0, inputManager.move.y).normalized;

		moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * ((inputManager.run) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
	}

	void Jump()
	{
		if(paused || isServer)
			return;

		if(inputManager.jump && grounded){
			rb.AddForce(transform.up * jumpForce);
			inputManager.jump = false;
		}
	}

	public void TakeDamage(float damage)
	{
		PV.RPC(nameof(RPC_TakeDamage), PV.Owner, damage);
	}
#endregion

#region  health
	[PunRPC]
	void RPC_TakeDamage(float damage, PhotonMessageInfo info)
	{
		currentHealth -= damage;

		if(currentHealth <= 0){
			Die();
			PlayerManager.Find(info.Sender).GetKill();
		}
	}

	void Die()
	{
		playerManager.Die();
	}
#endregion

#region radiation

	public void TakePassiveRadiation(float radiation)
	{
		currentMaxHealth -= radiation;
	}

	public void RemoveRadiation(float radiation)
	{
		currentMaxHealth += radiation;
	}

	public IEnumerator TakeActiveRadiation(float radiation)
	{
		canTakeRadiationDamage = false;
		yield return new WaitForSeconds(0.25f);
		currentMaxHealth -= radiation;
		canTakeRadiationDamage = true;
	}

#endregion
}


	/*
		for (int i = 0; i < guns.Length; i++)
		{
			if(currentWeapon == currentGun){
				currentGun = guns[i];
				currentGun.isActive = true;
				return;
			}else{
				guns[i].isActive = false;
				Debug.Log(currentWeapon.name + " is not " + guns[i].gunInfo);
			}
		}*/
