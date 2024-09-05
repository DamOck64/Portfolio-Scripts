using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [TextArea(5, 3)]
    public string Description;
    public string Name;
    public string Ability;
    public Image Icon;
    public Image AbilityIcon;
    public string Price;
    public Vector2 AnchorMax;
    public Vector2 AnchorMin;
    public Vector2 Pivot;
    public bool PriceTagEnabled;



    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTipManager.Instance.SetAndDisplayTT(Description, Name, Price, Ability, Icon, AbilityIcon, AnchorMax, AnchorMin, Pivot, PriceTagEnabled);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipManager.Instance.ClearAndHideTT();
    } 
}
