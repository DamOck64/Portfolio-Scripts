using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Buying : MonoBehaviour
{
    [SerializeField] private float rod1Cost, rod2Cost, tackle1Cost, tackle2Cost, bait1Cost, bait2Cost, bait3Cost;
    [SerializeField] private GameObject checkmark1, checkmark2;
    [SerializeField] private GameObject purchaseButton;
    [SerializeField] private Image rodOnDisplay;
    [SerializeField] private Sprite newRod;
    [SerializeField] private GameObject[] selectionBox = new GameObject[3];
    [SerializeField] private GameObject OOSS;
    [SerializeField] private TextMeshProUGUI rodCost;
    [SerializeField] public TextMeshProUGUI walletFunds;
    private int[] selectedBait = new int[3];
    private int baitIndex;

    private Inventory inventory;


    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        checkmark1.SetActive(false);
        checkmark2.SetActive(false);
        CalculateFromShop();
    }

    //Tackle buying code

    public void BuyTackle1(GameObject purchaseButton)
    {
        if (inventory.totalCash >= tackle1Cost) 
        {
            inventory.HasTackle1 = true;
            inventory.totalCash -= tackle1Cost;
            checkmark1.SetActive(true);
            Destroy(purchaseButton);
            CalculateFromShop();
        }
    }

    public void BuyTackle2(GameObject purchaseButton)
    {
        if (inventory.totalCash >= tackle2Cost)
        {
            inventory.HasTackle2 = true;
            inventory.totalCash -= tackle2Cost;
            checkmark2.SetActive(true);
            Destroy(purchaseButton);
            CalculateFromShop();
        }
    }

    //Rod upgrading Code

    public void UpgradeToReinforcedRod(Button purchaseButton)
    {
        if (inventory.totalCash >= rod1Cost)
        {
            inventory.currentRod = CurrentRod.Firm;
            inventory.totalCash -= rod1Cost;
            rodOnDisplay.sprite = newRod;
            rodCost.text = "$ 80,00";
            //Play animation of new rod replacing old one and price changing;
            purchaseButton.onClick.RemoveAllListeners();
            purchaseButton.onClick.AddListener(UpgradeToTitaniumRod);
            CalculateFromShop();
            inventory.UpdateRod();
        }
    }

    public void UpgradeToTitaniumRod()
    {
        if (inventory.totalCash >= rod2Cost)
        {
            inventory.currentRod = CurrentRod.Titanium;
            inventory.totalCash -= rod2Cost;
            //play animation of taking rod 2 and out of stock sign appearing
            OOSS.SetActive(true);
            rodCost.text = "";
            Destroy(rodOnDisplay);
            Destroy(purchaseButton);
            CalculateFromShop();
            inventory.UpdateRod();
        }
    }

    //Bait buying Code

    public void SelectBait(int index)
    {
        baitIndex = index;

        for (int i = 0; i < selectionBox.Length; i++)
        {
            selectionBox[i].gameObject.SetActive(false);
        }

        switch (index) 
        {
            case 0:
                selectionBox[0].SetActive(true);
                break;
            case 1:
                selectionBox[1].SetActive(true);
                break;
            case 2:
                selectionBox[2].SetActive(true);
                break;

        }
    }

    public void Buy1()
    {
        if (baitIndex == 0)
        {
            if (inventory.totalCash >= bait1Cost) 
            {
                inventory.MedmiumBait += 1;
                inventory.totalCash -= bait1Cost;
            }
        }

        if (baitIndex == 1)
        {
            if (inventory.totalCash >= bait2Cost)
            {
                inventory.HardBait += 1;
                inventory.totalCash -= bait2Cost;
            }
        }

        if (baitIndex == 2)
        {
            if (inventory.totalCash >= bait3Cost)
            {
                inventory.BossBait += 1;
                inventory.totalCash -= bait3Cost;
            }
        }
        CalculateFromShop();
    }

    public void Buy3() 
    {
        if (baitIndex == 0)
        {
            if (inventory.totalCash >= bait1Cost * 3)
            {
                inventory.MedmiumBait += 3;
                inventory.totalCash -= bait1Cost * 3;
            }
        }

        if (baitIndex == 1)
        {
            if (inventory.totalCash >= bait2Cost * 3)
            {
                inventory.HardBait += 3;
                inventory.totalCash -= bait2Cost * 3;
            }
        }

        if (baitIndex == 2)
        {
            if (inventory.totalCash >= bait3Cost * 3)
            {
                inventory.BossBait += 3;
                inventory.totalCash -= bait3Cost * 3;
            }
        }
        CalculateFromShop();
    }

    public void Buy10() 
    {
        if (baitIndex == 0)
        {
            if (inventory.totalCash >= bait1Cost * 9)
            {
                inventory.MedmiumBait += 10;
                inventory.totalCash -= bait1Cost * 9;
            }
        }

        if (baitIndex == 1)
        {
            if (inventory.totalCash >= bait2Cost * 9)
            {
                inventory.HardBait += 10;
                inventory.totalCash -= bait2Cost * 9;
            }
        }

        if (baitIndex == 2)
        {
            if (inventory.totalCash >= bait3Cost * 9)
            {
                inventory.BossBait += 10;
                inventory.totalCash -= bait3Cost * 9;
            }
        }
        CalculateFromShop();
    }



    private void CalculateFromShop()
    {
        walletFunds.text = "$ " + inventory.totalCash.ToString("F2");
        inventory.CalculateCash();
    }
}
