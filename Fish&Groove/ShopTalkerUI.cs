using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum talkingState
{
    FirstTime,
    SecondPart,
    Endless
}

public class ShopTalkerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI shopKeeperText;
    [SerializeField] private GameObject pressenter;
    [TextArea(3, 5)]
    [SerializeField] private string[] Sentances;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform putBackLocation;

    private Queue<string> currentlyQueuedSentances = new Queue<string>();
    private talkingState state;
    private PlayerCamera camRestrictor;
    private PlayerMovement movementRestrictor;
    private IEnumerator activeCoroutine;
    private bool allowedToPopUp;
    private bool dothisonce;

    private void Start()
    {
        camRestrictor = FindObjectOfType<PlayerCamera>();
        movementRestrictor = FindObjectOfType<PlayerMovement>();
        //Time.timeScale = 5f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && activeCoroutine == null && currentlyQueuedSentances != null)
        {
            DisplaySentance();
            pressenter.SetActive(false);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        Cursor.lockState = CursorLockMode.Confined;
        shopKeeperText.text = "";

        if (state == talkingState.FirstTime)
        {
            FirstTime();
        }


        if ((state == talkingState.SecondPart))
        {
            Tutorial();
        }

        if (state == talkingState.Endless)
        {
            EnterShop();
        }

        animator.SetBool("on", true);
        camRestrictor.AllowedToMoveCamera = false;
        movementRestrictor.allowedToMove = false;
        activeCoroutine = ShowTextOnDelay();
        StartCoroutine(activeCoroutine);

    }

    private void OnTriggerExit(Collider other)
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (state == talkingState.SecondPart)
        {
            state = talkingState.Endless;
            shopKeeperText.text = string.Empty;
        }

        if (state == talkingState.FirstTime)
        {
            state = talkingState.SecondPart;
            shopKeeperText.text = string.Empty;
        }

        dothisonce = false;

    }

    private void FirstTime()
    {
        for (int i = 0; i < 6; i++)
        {
            currentlyQueuedSentances.Enqueue(Sentances[i]);
            
        }
    }

    private void DisplaySentance()
    {
        if (currentlyQueuedSentances.Count == 0)
        {
            if (animator.GetBool("tutorial"))
            {
                animator.SetBool("tutorial", false);
            }

            if (state == talkingState.Endless && allowedToPopUp)
            {
                animator.SetBool("tutorial", false);
                animator.SetBool("ShopVisible", true);
            }
            else
            {
                animator.SetBool("on", false);
                allowedToPopUp = false;
            }
            camRestrictor.AllowedToMoveCamera = true;
            movementRestrictor.allowedToMove = true;
        }
        else
        {
            camRestrictor.AllowedToMoveCamera = false;
            movementRestrictor.allowedToMove = false;
            string currentmessage = currentlyQueuedSentances.Dequeue();
            activeCoroutine = TypeWriterMessage(currentmessage);
            StartCoroutine(activeCoroutine);
        }
    }

    private IEnumerator TypeWriterMessage(string receivedMessage)
    {
        shopKeeperText.text = null;
        foreach (char letter in receivedMessage.ToCharArray())
        {
            shopKeeperText.text += letter;
            //yield return new WaitForSeconds(0.025f);
        }

        yield return new WaitForEndOfFrame();
        pressenter.SetActive(true);
        activeCoroutine = null;
    }

    private IEnumerator ShowTextOnDelay()
    {
        yield return new WaitForSeconds(5f);
        DisplaySentance();
        if (state == talkingState.Endless)
        {
            allowedToPopUp = true;
        }
    }


    public void Leave()
    {
        int index = Random.Range(0, 3);
        
        switch (index)
        {
            case 0:
                currentlyQueuedSentances.Enqueue(Sentances[19]);
                    break;
            case 1:
                currentlyQueuedSentances.Enqueue(Sentances[20]);
                break;
            case 2:
                currentlyQueuedSentances.Enqueue(Sentances[21]);
                break;

        }
        DisplaySentance();
        animator.SetBool("ShopVisible", false);
        animator.SetBool("on", false);
        camRestrictor.AllowedToMoveCamera = true;
        movementRestrictor.allowedToMove = true;
    }



    public void Tutorial()
    {
        if (state == talkingState.Endless)
        {
            animator.SetBool("ShopVisible", false);
            animator.SetBool("tutorial", true);
            animator.SetBool("on", false);
            allowedToPopUp = false;
            for (int i = 8; i < 16; i++)
            {
                currentlyQueuedSentances.Enqueue(Sentances[i]);
            }
            DisplaySentance();

        }
        else
        {
            for (int i = 6; i < 18; i++)
            {
                currentlyQueuedSentances.Enqueue(Sentances[i]);
            }
        }

    }

    private void EnterShop()
    {
        currentlyQueuedSentances.Enqueue(Sentances[18]);
    }

}
