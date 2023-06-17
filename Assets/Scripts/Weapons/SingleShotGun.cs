using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class SingleShotGun : WeaponItem
{
	public bool isActive;
	[BoxGroup("References")]
	public Camera cam;	
	[BoxGroup("References")]
	public Transform shootPoint;
	[BoxGroup("References")]
	public WeaponInfo gunInfo;


	[BoxGroup("Components")]
	public InputManager inputManager;

	[BoxGroup("Components")]
	public Rigidbody playerRigidBody;

	[BoxGroup("Components")]
	public PlayerController controller;
	[BoxGroup("Components")]
	public GameObject ui;

	PhotonView PV;

	float fireTime;
	float reloadTimer;
	
	[BoxGroup("UI")]
	public TMP_Text currentAmmoText;
	[BoxGroup("UI")]
	public TMP_Text ammoReserveText;
	[BoxGroup("UI")]
	public TMP_Text ammoTypeText;

	[BoxGroup("Sway")]
	public float step = 0.01f;
	[BoxGroup("Sway")]
	public float maxStepDistance = 0.06f;
	Vector3 swayPos;
	
	[BoxGroup("Sway Rotation")]
	public float rotationStep = 4f;
	[BoxGroup("Sway Rotation")]
	public float maxRotationStep = 5f;
	[BoxGroup("Sway Rotation")]

	
	[BoxGroup("Composite")]
	public float smooth = 0.6f;
	[BoxGroup("Composite")]
	public float smoothRot = 0.8f;

	
	[BoxGroup("Bobbing")]
	public float speedCurve;
	[BoxGroup("Bobbing")]
	public float curveSin { get => Mathf.Sin(speedCurve); }
	[BoxGroup("Bobbing")]
	public float curveCos { get => Mathf.Cos(speedCurve); }
	[BoxGroup("Bobbing")]
	public Vector3 travelLimit = Vector3.one * 0.025f;
	[BoxGroup("Bobbing")]
	public Vector3 bobLimit = Vector3.one * 0.01f;
	
	[BoxGroup("Bobbing Rotation")]
	public Vector3 multiplier;
	[BoxGroup("Bobbing Rotation")]
	public float rotSpeed;
	[BoxGroup("Bobbing Rotation")]
	Vector3 bobEulerRotation;
	
	[BoxGroup("Bobbing Rotation")]
	public Vector3 recoilVector;

	[BoxGroup("Bobbing Rotation")]
	Vector3 bobPosition;
	[BoxGroup("Bobbing Rotation")]

	Vector3 swayEulorRot;

	void Start()
	{
		PV = GetComponent<PhotonView>();
		ammo = gunInfo.ammo;	
		
		if(!PV.IsMine){
			Destroy(ui);
			Destroy(this);
		}

		cam = GetComponentInParent<Camera>();
		inputManager = GetComponentInParent<InputManager>();
		playerRigidBody = GetComponentInParent<Rigidbody>();
		controller = GetComponentInParent<PlayerController>();

		transform.localPosition = Vector3.zero;
	}

	public override void Use()
	{
	}

	private void Update()
	{
		if(!isActive)
			return;

		Sway();
		SwayRotation();
		BobOffset();
		BobRotation();
		CompositePositionRotation();
			
		fireTime -= Time.deltaTime;
		if(fireTime <= 0 && ammo > 0 && controller.paused == false){
			
			if(gunInfo.automatic){
				if(Input.GetMouseButton(0)){
					Shoot();
					fireTime = gunInfo.fireRate;
					ammo--;
				}
			}else{
				if(Input.GetMouseButtonDown(0)){
					Shoot();
					fireTime = gunInfo.fireRate;
					ammo--;
				}
			}
		}

		if(Input.GetKeyDown(KeyCode.R)){
			StartCoroutine(ReloadCo());
		}

		
		currentAmmoText.text = ammo.ToString();
	
	}

	public void Sway()
	{
		Vector3 invertLook = inputManager.mouseMove * -step;
		invertLook.x = Mathf.Clamp(invertLook.x, -maxStepDistance, maxStepDistance);
		invertLook.y = Mathf.Clamp(invertLook.y, -maxStepDistance, maxStepDistance);

		swayPos = invertLook;
	}

	public void SwayRotation()
	{
		Vector3 invertLook = inputManager.mouseMove * -rotationStep;
		invertLook.y = Mathf.Clamp(invertLook.y, -maxRotationStep, maxRotationStep);
		invertLook.x = Mathf.Clamp(invertLook.x, -maxRotationStep, maxRotationStep);

		swayEulorRot = new Vector3(invertLook.y, invertLook.x, invertLook.z);
	}


	public void BobOffset()
	{
		speedCurve += Time.deltaTime * (controller.grounded ? playerRigidBody.velocity.magnitude : 1f) + 0.01f;

		bobPosition.x = (curveCos * bobLimit.x * ((controller.grounded) ? 1 : 0)) - (inputManager.move.x * travelLimit.x);	

		bobPosition.y = (curveSin * bobLimit.y) - (playerRigidBody.velocity.y * travelLimit.y);
		
		bobPosition.z = -(playerRigidBody.velocity.z * travelLimit.z);
	}

	public void BobRotation()
	{
		bobEulerRotation.x = (inputManager.move != Vector2.zero ? multiplier.x * (Mathf.Sin(2 * speedCurve) * rotSpeed) : multiplier.x * (Mathf.Sin(2* speedCurve) / rotSpeed));
		
		bobEulerRotation.y = (inputManager.move != Vector2.zero ? multiplier.y * curveCos : 0);
		bobEulerRotation.z = (inputManager.move != Vector2.zero ? multiplier.z * curveCos * inputManager.move.x : 0);

	}
	public void CompositePositionRotation()
	{
		transform.localPosition = Vector3.Lerp(transform.localPosition, swayPos + bobPosition, Time.deltaTime * smooth);
		transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(swayEulorRot) * Quaternion.Euler(bobEulerRotation), Time.deltaTime * smoothRot);
	}

	public IEnumerator ReloadCo()
	{
		
		yield return new WaitForSeconds(gunInfo.reloadSpeed);
		ammo = gunInfo.ammo;
		
	}

	void Shoot()
	{
		Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
		ray.origin = shootPoint.position;
		if(Physics.Raycast(ray, out RaycastHit hit))
		{
			hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((WeaponInfo)itemInfo).damage);
			hit.collider.gameObject.GetComponentInParent<IDamageable>()?.TakeDamage(((WeaponInfo)itemInfo).damage);
			PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
		}
		StartCoroutine(ShootRecoil());
	}

	public IEnumerator ShootRecoil()
	{
		Vector3 pos = transform.position;
		Vector3 lerpPoint = new Vector3(pos.x + recoilVector.x, pos.y + recoilVector.y, pos.z + recoilVector.z);
		transform.position = Vector3.Lerp(pos, lerpPoint, 0.2f);
		yield return new WaitForSeconds(0.2f);	
	}

	[PunRPC]
	void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
	{
		Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
		if(colliders.Length != 0)
		{
			GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
			Destroy(bulletImpactObj, 10f);
			bulletImpactObj.transform.SetParent(colliders[0].transform);
		}
	}
}
