using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RogueGameplayLoop : MonoBehaviour
{
    [SerializeField] private Image blood;
    [SerializeField] private Image Darkness;
    [SerializeField] private GameObject dialouge;
    [SerializeField] private TextMeshProUGUI wavesSurvived;
    [SerializeField] private TextMeshProUGUI amountOfUpgrades;
    [SerializeField] private TextMeshProUGUI FinalGold;
    [SerializeField] private TextMeshProUGUI FinalScore;
    [SerializeField] private TextMeshProUGUI HighScore;
    [SerializeField] private TextMeshProUGUI ButtonText;

    [SerializeField] private Button nextMenu;
    [SerializeField] private GameObject UpgradeScreen;
    [SerializeField] private GameObject Leaderboard;
    [SerializeField] private GameObject healthBarsToDisable;
    


    [HideInInspector] public int waveCountChecker;

    [SerializeField] private Animator ascendAnimation;
    [SerializeField] private GameObject ascendObject;


    private Animator animator;
    public int UpgradePoints;


    void Start()
    {
        animator = GetComponent<Animator>();
        waveCountChecker = WaveSystem.Instance.CurrentWave;
    }

    public IEnumerator ShowDeathDialouge()
    {
        if (FloatValueManager.Instance.CurrentScore > FloatValueManager.Instance.HighScore)
        {
            FinalScore.text = "Score hit: " + FloatValueManager.Instance.CurrentScore.ToString() + " Previous best broken!";
        }
        else
        {
            FinalScore.text = "Score hit: " + FloatValueManager.Instance.CurrentScore.ToString();
        }
        FinalGold.text = "Current Gold: " + FloatValueManager.Instance.CurrentGold.ToString();
        HighScore.text = "Previous best score: " + FloatValueManager.Instance.HighScore.ToString();

        for (int i = 0; i < 150; i += 10)
        {
            blood.color = new Color32(150, 1, 1, (byte)i) ;
            yield return new WaitForSeconds(.1f);
        }

        //yield return new WaitForSeconds(2f);
        healthBarsToDisable.SetActive(false);

        for (int i = 0; i < 255; i++)
        {
            Darkness.color = new Color32(0, 0, 0, (byte)i);
            yield return new WaitForSeconds(0.001f);
        }
        Debug.Log("test");
        yield return new WaitForSeconds(.5f);
        animator.SetBool("Died", true);
        dialouge.SetActive(true);
        wavesSurvived.text = "Because you reached wave: " + waveCountChecker.ToString() + ", you get";

        yield return new WaitForSeconds(7.5f);

        amountOfUpgrades.text = "..";
        yield return new WaitForSeconds(.6f);
        amountOfUpgrades.text = "...";
        yield return new WaitForSeconds(.6f);
        amountOfUpgrades.text = "....";
        yield return new WaitForSeconds(.6f);
        
        CalculateUpgrades();

        yield break;
    }

    private void CalculateUpgrades()
    {
        //If player didnt reach atleast wave 5
        if (waveCountChecker < 5)
        {
            amountOfUpgrades.text = "0";
            ButtonText.text = "Better luck next time";
            nextMenu.onClick.AddListener(SkipToLeaderboard);
        }

        //If player reached wave 5 and beyond
        else if (waveCountChecker >= 5)
        {
            // ↓↓ This gaurantees your 1 point for reaching wave 5 
            UpgradePoints += 1;
            waveCountChecker -= 5;

            // ↓↓ Every 3 extra waves extra it will give you another point
            for (int i = 3; i < waveCountChecker; i += 3)
            {
                print(i);
                UpgradePoints += 1;
            }

            Debug.Log(UpgradePoints);
            amountOfUpgrades.text = UpgradePoints.ToString();
            ButtonText.text = "Spend upgrade points";
            nextMenu.onClick.AddListener(GoToUpgrades);
        }

    }

    #region OnClick functions

    private void GoToUpgrades()
    {
        var upgradeGiverInQuestion = UpgradeScreen.GetComponentInChildren<UpgradeGiver>();
        UpgradeScreen.SetActive(true);
        upgradeGiverInQuestion.UpdateAmountShown();
        nextMenu.onClick.RemoveAllListeners();
    }

    private void SkipToLeaderboard()
    {
        //Leaderboard.SetActive(true);
        // Leaderboard isnt in right now so just return to the game again.
        OnClickAscend();
        nextMenu.onClick.RemoveAllListeners();
    }

    public void OnClickAscend()
    {
        ascendObject.SetActive(true);
        StartCoroutine(Ascend());
        
    }

    private IEnumerator Ascend()
    {
        ascendAnimation.SetTrigger("Time");
        yield return new WaitForSeconds(6f);
        FloatValueManager.Instance.CurrentScore = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion
}
