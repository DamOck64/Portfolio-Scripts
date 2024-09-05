using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager Instance;

    [SerializeField] private TextMeshProUGUI WeaponDescription;
    [SerializeField] private TextMeshProUGUI WeaponName;
    [SerializeField] private Image WeaponIcon;
    [SerializeField] private Image AbilityIcon;
    [SerializeField] private TextMeshProUGUI WeaponPrice;
    [SerializeField] private TextMeshProUGUI WeaponAbility;
    [SerializeField] private RectTransform ToolTipSide;
    [SerializeField] private GameObject priceTag;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        transform.position = Input.mousePosition;
    }

        
    public void SetAndDisplayTT(string desc, string name, string price, string abilityname, Image icon, Image abilityIcon, Vector2 maxAnchor, Vector2 minAnchor, Vector2 pivot, bool priceTagEnabled)
    {
        gameObject.SetActive(true);
        WeaponDescription.text = desc.ToString();
        WeaponName.text = name.ToString();
        WeaponAbility.text = abilityname.ToString();
        WeaponIcon.sprite = icon.sprite;
        AbilityIcon.sprite = abilityIcon.sprite;
        WeaponPrice.text = price.ToString();
        ToolTipSide.anchorMin = minAnchor;
        ToolTipSide.anchorMax = maxAnchor;
        ToolTipSide.pivot = pivot;
        if (priceTagEnabled)
        {
            priceTag.SetActive(true);
        }
        else
        {
            priceTag.SetActive(false);
        }
    }

    public void ClearAndHideTT()
    {
        gameObject.SetActive(false);
        WeaponAbility.text = "";
        WeaponDescription.text = "";
        WeaponName.text = "";
        //WeaponIcon = null;
        WeaponPrice.text = "";
    }
}
