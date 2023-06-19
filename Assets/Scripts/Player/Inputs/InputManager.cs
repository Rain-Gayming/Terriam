using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Sirenix.OdinInspector;
using System;
using System.IO;
using UnityEngine.InputSystem;
using TMPro;
using ES3Internal;
using ES3Types;

public class InputManager : MonoBehaviour
{
    public PhotonView view;
    public static PlayerInputs inputs;
    
    public static event Action rebindComplete;
    public static event Action rebindCanceled;
    public static event Action<InputAction, int> rebindStarted;

    static KeybindData myKeybindData = new KeybindData();
    public KeybindData shownKeybindData;
    public static string saveFile;
    public bool ignorePhoton;

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
    [BoxGroup("Keyboard")]
    public bool leftLean;
    [BoxGroup("Keyboard")]
    public bool rightLean;

    [BoxGroup("Mouse")]
    public Vector2 mouseMove;
    [BoxGroup("Mouse")]
    public bool leftMouse;
    [BoxGroup("Mouse")]
    public bool rightMouse;

    private void OnEnable() {
        saveFile = Application.persistentDataPath + "/keybindings.json";
        inputs = new PlayerInputs();
        inputs.Enable();
    }


    void Update()
    {
        shownKeybindData = myKeybindData;
        if( view != null && ignorePhoton){
            Debug.Log("View is null or youre ignoring photn");
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
        
        inputs.Keyboard.LeanLeft.performed += _ => leftLean = true;
        inputs.Keyboard.LeanLeft.canceled += _ => leftLean = false;

        inputs.Keyboard.LeanRight.performed += _ => rightLean = true;
        inputs.Keyboard.LeanRight.canceled += _ => rightLean = false;
        #endregion 
   
        #region mouse
        mouseMove = inputs.Mouse.Move.ReadValue<Vector2>();
        
        inputs.Mouse.LeftClick.performed += _ => leftMouse = true;
        inputs.Mouse.LeftClick.canceled += _ => leftMouse = false;
        
        inputs.Mouse.RightClick.performed += _ => rightMouse = true;
        inputs.Mouse.RightClick.canceled += _ => rightMouse = false;

        #endregion
    }
    


    public static void StartRebind(string actionName, int bindingIndex, TMP_Text statusText)
    {
        
        InputAction action = inputs.asset.FindAction(actionName);

        if(action == null || action.bindings.Count <= bindingIndex){
            Debug.Log("Couldn't find action or binding");
            return;
        }
        
        if(action.bindings[bindingIndex].isComposite){
            var firstPartIndex = bindingIndex + 1;
            if(firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isComposite){
                DoRebind(action, bindingIndex,statusText,true);
            }

        }else{
            DoRebind(action, bindingIndex, statusText, false);
        }
    }

    public static void DoRebind(InputAction actionToRebind, int bindingIndex, TMP_Text statusText, bool allCompositeParts)
    {
        if(actionToRebind == null || bindingIndex < 0){
            return;
        }

        statusText.text = $"Press a {actionToRebind.expectedControlType}";

        actionToRebind.Disable();

        var rebind = actionToRebind.PerformInteractiveRebinding(bindingIndex);

        rebind.OnComplete(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();

            if(allCompositeParts){
                var nextBindingIndex = bindingIndex + 1;
                if(nextBindingIndex < actionToRebind.bindings.Count && actionToRebind.bindings[nextBindingIndex].isComposite){
                    DoRebind(actionToRebind, nextBindingIndex, statusText, allCompositeParts);
                }
            }

            rebindComplete?.Invoke();

            SaveBindingOverride(actionToRebind);
        });
        rebind.OnCancel(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();
            rebindCanceled?.Invoke();
        });

        rebindStarted?.Invoke(actionToRebind, bindingIndex);
        rebind.Start();
    }

    public static string GetBindingName(string actionName, int bindingIndex)
    {
        if(inputs == null){
            inputs = new PlayerInputs();
            inputs.Enable();
        }

        InputAction action = inputs.asset.FindAction(actionName);

        return action.GetBindingDisplayString(bindingIndex);
    }

    public static void ResetBinding(string actionName, int bindingIndex)
    {
        InputAction action = inputs.asset.FindAction(actionName);

        if(action == null || action.bindings.Count <= bindingIndex){
            Debug.Log("Coudnt find action or binding");
            return;
        }

        if(action.bindings[bindingIndex].isComposite){
            for (int i = bindingIndex; i < action.bindings.Count && action.bindings[i].isComposite; i++)
            {
                action.RemoveBindingOverride(i);
                SaveBindingOverride(inputs.FindAction(actionName));
            }
        }else
        {
            action.RemoveBindingOverride(bindingIndex);
                SaveBindingOverride(inputs.FindAction(actionName));
        }
    }

    public static void SaveBindingOverride(InputAction action)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            //replace with something like json saving
            PlayerPrefs.SetString(action.actionMap + action.name + i, action.bindings[i].overridePath);

            string jsonString = inputs.SaveBindingOverridesAsJson();
            //jsonString = JsonUtility.ToJson(jsonString, true);
            myKeybindData.action = action.name;
            myKeybindData.keybind = inputs.FindAction(action.name).bindings[0].overridePath;
            ES3.Save("MyKeybinds " + action.name, myKeybindData);
            
            //Debug.Log(jsonString);
            
            //File.WriteAllText(saveFile, jsonString);
            //JsonUtility.ToJson(jsonString, true);
            
        }
    }

    public static void LoadBindingOverride(InputAction action)
    {
        if(inputs == null){
            inputs = new PlayerInputs();
        }
        if(ES3.KeyExists("MyKeybinds " + action.name)){
            myKeybindData = ES3.Load<KeybindData>("MyKeybinds "  + action.name);

            InputBinding inputBinding = action.bindings[0];
            inputBinding.overridePath = myKeybindData.keybind;
            action.ApplyBindingOverride(0, inputBinding);

            InputAction newAction = inputs.asset.FindAction(action.name);

            newAction.ApplyBindingOverride(inputBinding.overridePath);
            Debug.Log(action.bindings[0].overridePath);

            
        }else if(action.bindings[0].overridePath == null){
            inputs = new PlayerInputs();
            inputs.Enable();
        }

            //inputs = ES3.Load<PlayerInputs>("MyKeybinds", saveFile);
        //inputs.LoadBindingOverridesFromJson(saveFile, true);
    }
}

[System.Serializable]
public class KeybindData
{
    //public PlayerInputs inputAction;
    public string action;
    public string keybind;
}