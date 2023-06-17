using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class InfoPannelDisplay : MonoBehaviour
{
    public InfoPannel info;
    public Image iconImage;
    public TMP_Text infoText;
    public TMP_Text titleText;
    // Update is called once per frame
    void Update()
    {
        if(iconImage){
            iconImage.sprite = info.icon;
        }
        if(titleText){
            titleText.text = info.title;        
        }else{
            infoText.text = info.title + ": " + info.info;       
        } 
    }
}

[System.Serializable]
public class InfoPannel
{
    [PreviewField] public Sprite icon;
    
    [VerticalGroup("Text")]
    public string title;
    [VerticalGroup("Text")]
    [TextArea(5,5)] public string info;

    public InfoPannel(string _title, string _info)
    {
        title = _title;
        info = _info;
    }
}