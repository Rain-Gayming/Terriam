using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class ReBindUI : MonoBehaviour
{
    public InputActionReference inputActionReference;
    InputManager inputManager;
    public bool excludeMouse = true;
    public int selectedBinding;
    public InputBinding.DisplayStringOptions displayStringOptions;
    [Header("Binding Info - Dont Edit")]
    public InputBinding inputBinding;
    public int bindingIndex;

    public string actionName;

    [Header("UI Fields")]
    public TMP_Text actionText;
    public Button rebindButton;
    public TMP_Text rebindText;
    public Button resetButton;

    public void OnEnable()
    {
        inputManager = GetComponentInParent<InputManager>();
        if(inputActionReference != null){
            InputManager.LoadBindingOverride(inputActionReference.action);
            GetBindingInfo();
            UpdateUI();  
        }

        InputManager.rebindComplete += UpdateUI;
        InputManager.rebindCanceled += UpdateUI;
    }

    private void OnDisable() 
    {        
        InputManager.rebindComplete -= UpdateUI;        
        InputManager.rebindCanceled -= UpdateUI;
    }
    private void Update() {
        //UpdateUI();
    }

    public void GetBindingInfo()
    {
        if(inputActionReference.action != null){
            actionName = inputActionReference.action.name;
        }

        if(inputActionReference.action.bindings.Count > selectedBinding){
            inputBinding = inputActionReference.action.bindings[selectedBinding];
            bindingIndex = selectedBinding;
        }
    }

    public void UpdateUI()
    {
        if(actionText != null){
            actionText.text = actionName;
        }


        if(rebindText != null){
            rebindText.text = InputManager.GetBindingName(actionName, bindingIndex);
        }
    }

    public void DoRebind()
    {
        InputManager.StartRebind(actionName, bindingIndex, rebindText);
    }

    public void ResetBinding()
    {
        InputManager.ResetBinding(actionName, bindingIndex);
        UpdateUI();
    }
}