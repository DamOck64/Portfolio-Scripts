using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpgradeGiver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountOfUpgrades;
    [SerializeField] private RogueGameplayLoop TakeUpgradePoints;
    [SerializeField] private TextMeshProUGUI[] StatTexts;
    [SerializeField] private GameObject statScreenToEnable;
    [SerializeField] private GameObject reincarnationButton;

    private FloatValueManager setStats;

    private Animator selectedButton;

    //Points to allocate to Each Stat
    private int HP_UP, ATK_UP, STAMINA_UP, REGEN_UP;

    //Number in the array is related to the order up top (0 = HP)
    private float[] valuesToAdd = new float[4];

    //Button object related to advanced prestiging
    [SerializeField] private GameObject advancedButton;

    void Start()
    {
       setStats = FindObjectOfType<FloatValueManager>();

        if (FloatValueManager.Instance.AdvancedPrestigingUnlocked)
        {
            advancedButton.SetActive(true);
        }
        else
        {
            advancedButton.SetActive(false);
        }
    }


    public void PurchaseUpgradePoint()
    {
        if (FloatValueManager.Instance.CurrentGold >= 600)
        {
            TakeUpgradePoints.UpgradePoints += 1;
            FloatValueManager.Instance.CurrentGold -= 600;
            amountOfUpgrades.text = TakeUpgradePoints.UpgradePoints.ToString();
        }
    }

    public void Upgrade(int ID)
    {
        if (TakeUpgradePoints.UpgradePoints > 0)
        {
            if (ID == 0)
            {
                HP_UP += 1;
                TakeUpgradePoints.UpgradePoints -= 1;
            }
            if (ID == 1)
            {
                ATK_UP += 1;
                TakeUpgradePoints.UpgradePoints -= 1;
            }
            if (ID == 2)
            {
                STAMINA_UP += 1;
                TakeUpgradePoints.UpgradePoints -= 1;
            }
            if (ID == 3)
            {
                REGEN_UP += 1;
                TakeUpgradePoints.UpgradePoints -= 1;
            }
            UpdateAmountShown();

        }
    }

    public void UpdateAmountShown()
    {
        amountOfUpgrades.text = TakeUpgradePoints.UpgradePoints.ToString();
    }

    private void CalculateAddedStats()
    {
        valuesToAdd[0] = HP_UP * 10; //+ setStats.DefaultMaxHealth;
        valuesToAdd[1] = ATK_UP * 2; // + setStats.DefaultDamage;
        valuesToAdd[2] = STAMINA_UP * 10; // + setStats.DefaultMaxStamina;
        valuesToAdd[3] = REGEN_UP * 2; // + setStats.DefaultStaminaRegen;
    }


    public void ConfirmUpgrades()
    {
        CalculateAddedStats();
        setStats.SaveFloatValues(valuesToAdd[0], valuesToAdd[1], valuesToAdd[2], valuesToAdd[3]);
        StartCoroutine(UpgradeStatsLive());

    }

    private IEnumerator UpgradeStatsLive()
    {
        statScreenToEnable.SetActive(true);

        float hp = setStats.OldMaxHealth;
        float dmg = setStats.OldDamage;
        float stam = setStats.OldStamina;
        float regen = setStats.OldRegen;


        StatTexts[0].text = hp.ToString();
        StatTexts[1].text = dmg.ToString();
        StatTexts[2].text = stam.ToString();
        StatTexts[3].text = regen.ToString();


        for (int i = 0; i < valuesToAdd[0]; i++)
        {
            hp++;
            StatTexts[0].text = hp.ToString();
            yield return new WaitForSeconds(0.04f);
        }

        for (int i = 0; i < valuesToAdd[2]; i++)
        {
            stam++;
            StatTexts[2].text = stam.ToString();
            yield return new WaitForSeconds(0.04f);
        }

        for (int i = 0; i < valuesToAdd[3]; i++)
        {
            regen++;
            StatTexts[3].text = regen.ToString();
            yield return new WaitForSeconds(0.05f);
        }

        for (int i = 0; i < valuesToAdd[1]; i++)
        {
            dmg++;
            StatTexts[1].text = dmg.ToString();
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(2f);

        reincarnationButton.SetActive(true);

        yield break;
    }



}
