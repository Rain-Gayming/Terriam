using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Sirenix.OdinInspector;

public class InputManager : MonoBehaviour
{
    public PhotonView view;
    public PlayerInputs inputs;

    [BoxGroup("Keyboard")]
    public Vector2 move;
    [BoxGroup("Keyboard")]
    public bool jump;
    [BoxGroup("Keyboard")]
    public bool crouch;
    [BoxGroup("Keyboard")]
    public bool run;
    [BoxGroup("Keyboard")]
    public bool reload;
    [BoxGroup("Keyboard")]
    public bool interact;
    [BoxGroup("Keyboard")]
    public bool primaryItem;
    [BoxGroup("Keyboard")]
    public bool secondaryItem;
    [BoxGroup("Keyboard")]
    public bool equipmentItem;
    [BoxGroup("Keyboard")]
    public bool meleeItem;
    [BoxGroup("Keyboard")]
    public bool pause;
    [BoxGroup("Keyboard")]
    public bool inventory;

    [BoxGroup("Mouse")]
    public Vector2 mouseMove;
    [BoxGroup("Mouse")]
    public bool leftMouse;
    [BoxGroup("Mouse")]
    public bool rightMouse;

    private void Awake() {
        inputs = new PlayerInputs();
        inputs.Enable();
    }

    void Update()
    {
        if(!view.IsMine){
            return;
        }
        #region keyboards

        move = inputs.Keyboard.Move.ReadValue<Vector2>();

        inputs.Keyboard.Jump.performed += _ => jump = true;
        inputs.Keyboard.Jump.canceled += _ => jump = false;

        inputs.Keyboard.Crouch.performed += _ => crouch = true;
        inputs.Keyboard.Crouch.canceled += _ => crouch = false;

        inputs.Keyboard.Run.performed += _ => run = true;
        inputs.Keyboard.Run.canceled += _ => run = false;

        inputs.Keyboard.Reload.performed += _ => reload = true;
        inputs.Keyboard.Reload.canceled += _ => reload = false;
        
        inputs.Keyboard.Interact.performed += _ => interact = true;
        inputs.Keyboard.Interact.canceled += _ => interact = false;
        
        inputs.Keyboard.PrimaryItem.performed += _ => primaryItem = true;
        inputs.Keyboard.PrimaryItem.canceled += _ => primaryItem = false;
        
        inputs.Keyboard.SecondaryItem.performed += _ => secondaryItem = true;
        inputs.Keyboard.SecondaryItem.canceled += _ => secondaryItem = false;

        inputs.Keyboard.EquipmentItem.performed += _ => equipmentItem = true;
        inputs.Keyboard.EquipmentItem.canceled += _ => equipmentItem = false;

        inputs.Keyboard.MeleeItem.performed += _ => meleeItem = true;
        inputs.Keyboard.MeleeItem.canceled += _ => meleeItem = false;
        
        inputs.Keyboard.Pause.performed += _ => pause = true;
        inputs.Keyboard.Pause.canceled += _ => pause = false;

        inputs.Keyboard.Inventory.performed += _ => inventory = true;
        inputs.Keyboard.Inventory.canceled += _ => inventory = false;

        #endregion 
   
        #region mouse
        mouseMove = inputs.Mouse.Move.ReadValue<Vector2>();
        
        inputs.Mouse.LeftClick.performed += _ => leftMouse = true;
        inputs.Mouse.LeftClick.canceled += _ => rightMouse = false;
        
        inputs.Mouse.RightClick.performed += _ => rightMouse = true;
        inputs.Mouse.RightClick.canceled += _ => rightMouse = false;

        #endregion
    }
}
