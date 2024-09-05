using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum TalkingStates
{
    FirstTime,
    SecondPart,
    EndlessCycle
}

public enum Moods
{
    Happy,
    Normal,
    Mad,
    Confused1,
    Confused2
}
public class FishSellingUI : MonoBehaviour
{
    [SerializeField] private Sprite[] diffrentMoods;
    [SerializeField] private Image ActiveMood;
    [SerializeField] private TextMeshProUGUI shopKeeperText;
    [TextArea(3, 5)]
    [SerializeField] private string[] Sentances;
    [SerializeField] private TextMeshProUGUI[] responsButons;
    [SerializeField] private GameObject[] theButtonsGO = new GameObject[2];
    [SerializeField] private Button[] buttons = new Button[2];
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject pressenter;

    private Queue<string> currentlyQueuedSentances = new Queue<string>();
    private TalkingStates state;
    private Moods moodstate;
    private Inventory inventoryacces;
    private IEnumerator activeCoroutine;
    private PlayerCamera camRestrictor;
    private PlayerMovement movementRestrictor;
    private SellFish sellingFunction;

    private bool doThisOnce;
    private bool allowedToGetOut;
    private bool THISISACTIVE;


    void Start()
    {
        inventoryacces = FindObjectOfType<Inventory>();
        camRestrictor = FindObjectOfType<PlayerCamera>();
        movementRestrictor = FindObjectOfType<PlayerMovement>();
        sellingFunction = GetComponent<SellFish>();
        FirstTime(); 

       // theButtonsGO[0].SetActive(false);
       // theButtonsGO[1].SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && activeCoroutine == null && currentlyQueuedSentances != null && THISISACTIVE) 
        {
            if (allowedToGetOut) 
            {
                camRestrictor.AllowedToMoveCamera = true;
                movementRestrictor.allowedToMove = true;
                animator.SetBool("On", false);
            }
            else
            {
                DisplaySentance();
                pressenter.SetActive(false);
                EmotionHandler();
            }
        }
        DisplayButtonsWhenNeeded();
    }

    private void OnTriggerEnter(Collider other)
    {
        Cursor.lockState = CursorLockMode.Confined;
        THISISACTIVE = true;
        animator.SetBool("On", true);
        activeCoroutine = ShowTextOnDelay();
        StartCoroutine(activeCoroutine);
        allowedToGetOut =  false;

        if (state == TalkingStates.EndlessCycle)
        {
            currentlyQueuedSentances.Enqueue(Sentances[9]);
            theButtonsGO[0].SetActive(true);
            theButtonsGO[1].SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        THISISACTIVE = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void EmotionHandler()
    {
        if (moodstate == Moods.Normal)
        {
            ActiveMood.sprite = diffrentMoods[4];
        }

        else if (moodstate == Moods.Mad)
        {
            ActiveMood.sprite = diffrentMoods[0];
        }

        else if (moodstate == Moods.Happy)
        {
            ActiveMood.sprite = diffrentMoods[3];
        }

        else if (moodstate == Moods.Confused1)
        {
            ActiveMood.sprite = diffrentMoods[1];
        }

        else if (moodstate == Moods.Confused2)
        {
            ActiveMood.sprite = diffrentMoods[2];
        }
    }

    private void FirstTime()
    {
        state = TalkingStates.FirstTime;
        moodstate = Moods.Confused1;
        for (int i = 0; i < 2; i++)
        {
            currentlyQueuedSentances.Enqueue(Sentances[i]);
        }
    }

    private void DisplayButtonsWhenNeeded()
    {
        if (state == TalkingStates.FirstTime && currentlyQueuedSentances.Count == 0 && !doThisOnce)
        {
            theButtonsGO[0].SetActive(true);
            theButtonsGO[1].SetActive(true);

            buttons[0].onClick.AddListener(Truth);
            buttons[1].onClick.AddListener(NoneOfYourBusiness);

            responsButons[0].text = "Tell him your story";
            responsButons[1].text = "None of your busniness.";
            doThisOnce = true;
        }

        else if (state == TalkingStates.SecondPart && currentlyQueuedSentances.Count == 0)
        {
            theButtonsGO[0].SetActive(true);
            theButtonsGO[1].SetActive(true);

            responsButons[0].text = "Sure";
            responsButons[1].text = "Nah im good.";
        }

        else if (state == TalkingStates.EndlessCycle && currentlyQueuedSentances.Count == 0)
        {
            responsButons[0].text = "I do, here you go";
            responsButons[1].text = "Nothing yet.";
        }
    }

    private void DisplaySentance()
    {
            camRestrictor.AllowedToMoveCamera = false;
            movementRestrictor.allowedToMove = false;
            string currentmessage = currentlyQueuedSentances.Dequeue();
            activeCoroutine = TypeWriterMessage(currentmessage);
            StartCoroutine(activeCoroutine);
    }

    private IEnumerator TypeWriterMessage(string receivedMessage)
    {
        shopKeeperText.text = null;
        foreach (char letter in receivedMessage.ToCharArray())
        {
            shopKeeperText.text += letter;
            yield return new WaitForSeconds(0.025f);
        }

        yield return new WaitForEndOfFrame();
        pressenter.SetActive(true);
        activeCoroutine = null;
    }

    private IEnumerator ShowTextOnDelay()
    {
        yield return new WaitForSeconds(1f);
        DisplaySentance();
    }


    //Down below are the buttons for reactions

    public void NoneOfYourBusiness()
    {
        currentlyQueuedSentances.Enqueue(Sentances[2]);
        moodstate = Moods.Mad;
        EmotionHandler();

        for (int i = 4; i < 7; i++)
        {
            currentlyQueuedSentances.Enqueue(Sentances[i]);
        }

        moodstate = Moods.Normal;
        buttons[0].onClick.RemoveListener(Truth);
        buttons[1].onClick.RemoveListener(NoneOfYourBusiness);

        buttons[0].onClick.AddListener(TellYes);
        buttons[1].onClick.AddListener(TellNo);

        theButtonsGO[0].SetActive(false);
        theButtonsGO[1].SetActive(false);
        DisplaySentance();
        state = TalkingStates.SecondPart;
    }

    public void Truth()
    {
        currentlyQueuedSentances.Enqueue(Sentances[3]);
        moodstate = Moods.Happy;
        EmotionHandler();

        for (int i = 4; i < 7; i++)
        {
            currentlyQueuedSentances.Enqueue(Sentances[i]);
        }

        moodstate = Moods.Normal;
        buttons[0].onClick.RemoveListener(Truth);
        buttons[1].onClick.RemoveListener(NoneOfYourBusiness);

        buttons[0].onClick.AddListener(TellYes);
        buttons[1].onClick.AddListener(TellNo);

        theButtonsGO[0].SetActive(false);
        theButtonsGO[1].SetActive(false);
        DisplaySentance();
        state = TalkingStates.SecondPart;
    }

    public void TellYes()
    {
        currentlyQueuedSentances.Enqueue(Sentances[7]);
        DisplaySentance();
        moodstate = Moods.Confused2;
        EmotionHandler();

        state = TalkingStates.EndlessCycle;

        buttons[0].onClick.RemoveListener(TellYes);
        buttons[1].onClick.RemoveListener(TellNo);

        buttons[0].onClick.AddListener(Sell);
        buttons[1].onClick.AddListener(DontSell);

        theButtonsGO[0].SetActive(false);
        theButtonsGO[1].SetActive(false);
        allowedToGetOut = true;
    }

    public void TellNo() 
    {
        currentlyQueuedSentances.Enqueue(Sentances[8]);
        DisplaySentance();
        moodstate = Moods.Confused1;
        EmotionHandler();

        state = TalkingStates.EndlessCycle;

        buttons[0].onClick.RemoveListener(TellYes);
        buttons[1].onClick.RemoveListener(TellNo);

        buttons[0].onClick.AddListener(Sell);
        buttons[1].onClick.AddListener(DontSell);

        theButtonsGO[0].SetActive(false);
        theButtonsGO[1].SetActive(false);
        allowedToGetOut = true;
    }

    public void Sell()
    {
        if (inventoryacces.inventoryList.Count == 0)
        {
            currentlyQueuedSentances.Enqueue(Sentances[12]);
            DisplaySentance();
            moodstate= Moods.Confused2;
            EmotionHandler();
        }
        else if (inventoryacces.inventoryList.Count == 1)
        {
            currentlyQueuedSentances.Enqueue(Sentances[10]);
            DisplaySentance();
            moodstate = Moods.Happy;
            EmotionHandler();
            sellingFunction.OnSell();
        }
        else if (inventoryacces.inventoryList.Count > 1) 
        {
            currentlyQueuedSentances.Enqueue("Awesome, I'll take those " + inventoryacces.inventoryList.Count.ToString() + " fish off of your hands, here you have your cash.");
            DisplaySentance();
            moodstate = Moods.Happy;
            EmotionHandler();
            sellingFunction.OnSell();
        }

        theButtonsGO[0].SetActive(false);
        theButtonsGO[1].SetActive(false);
        allowedToGetOut = true;
    }

    public void DontSell()
    {
        currentlyQueuedSentances.Enqueue(Sentances[11]);
        DisplaySentance();
        moodstate = Moods.Confused1;
        EmotionHandler();

        theButtonsGO[0].SetActive(false);
        theButtonsGO[1].SetActive(false);
        allowedToGetOut = true;
    }
}
