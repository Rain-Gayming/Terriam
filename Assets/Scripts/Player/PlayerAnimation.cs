using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimation : MonoBehaviour
{
    public SingleShotGun currentGun;
    public TwoBoneIKConstraint leftHand;
    public TwoBoneIKConstraint rightHand;
    public RigBuilder builder;

    private void Start() {
    }

    public void Update()
    {
        if(currentGun == null){
            currentGun = GetComponentInParent<PlayerController>().weaponEquippedObject.GetComponent<SingleShotGun>();
            leftHand.data.target = currentGun.barrelPoint;
            rightHand.data.target = currentGun.gripPoint;
            leftHand.weight = 1;
            rightHand.weight = 1;
            builder.Build();
        }
    }
}
