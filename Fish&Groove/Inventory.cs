using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum CurrentRod
{
    Flimsy,
    Firm,
    Titanium
}
public class Inventory : MonoBehaviour
{
    public List<GameObject> inventoryList;

    [SerializeField] private Animator animator;
    private bool toggled;


    //Money area
    [SerializeField] private float bucks = 0;
    [SerializeField] public float cents = 0;
    [SerializeField] public float totalCash = 0;
    [SerializeField] private TextMeshProUGUI cashText;
    

    //Your resources
    public int StarterBait = 200000000, MedmiumBait = 0, HardBait = 0, BossBait = 0;
    // 0 = start, 1 = medium, 2 = hard, 3 = boss 
    private int selectedBait = 0;
    [SerializeField] private Sprite[] baitPictures;
    [SerializeField] private Image displayedBait;
    [SerializeField] private TextMeshProUGUI amountOfBait;

    [SerializeField] private GameObject[] tackleButtons;
    public bool HasTackle1, HasTackle2;

    public CurrentRod currentRod;
    public int AllowedMisses;
    [SerializeField] private Image currentRodGraphic;
    [SerializeField] private Sprite[] alternateRods;
    [SerializeField] private TextMeshProUGUI allowedMisses;
    


    private PlayerCamera currentCamera;
    private PlayerMovement currentMovement;

    public void AddFish(GameObject fish)
    {
        inventoryList.Add(fish);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            toggled = !toggled;
            ToggleInventory();
            //CalculateCash();
        }

    }

    private void Start()
    {
        currentCamera = FindObjectOfType<PlayerCamera>();
        currentMovement = FindObjectOfType<PlayerMovement>();
        AllowedMisses = 10;
    }

    public void CalculateCash()
    {
        bucks = cents / 100;
        cents = 0;
        totalCash += bucks;
        cashText.text = "$ " + totalCash.ToString("F2");
        cashText.text.Replace(",", ".");
    }

    private void ToggleInventory()
    {
        if (toggled)
        {
            animator.SetBool("Toggle", true);
            Cursor.lockState = CursorLockMode.Confined;
            currentMovement.allowedToMove = false;
            currentCamera.AllowedToMoveCamera = false;
        }

        else
        {
            animator.SetBool("Toggle", false);
            Cursor.lockState = CursorLockMode.Locked;
            currentCamera.AllowedToMoveCamera = true;
            currentMovement.allowedToMove = true;
        }

    }

    public void UpdateRod()
    {
        if (currentRod == CurrentRod.Firm)
        {
            currentRodGraphic.sprite = alternateRods[0];
            AllowedMisses = 15;
            allowedMisses.text = "Allowed misses 15";
        }

        if (currentRod == CurrentRod.Titanium)
        {
            currentRodGraphic.sprite = alternateRods[1];
            AllowedMisses = 25;
            allowedMisses.text = "Allowed misses 25";
        }
    }

    public void UpdateTackles()
    {
        if (HasTackle1)
        {
            tackleButtons[0].SetActive(true);
        }

        if (HasTackle2) 
        {
            tackleButtons[1].SetActive(true);
        }
    }


    public void CycleBait(bool side)
    {
        //true = right, false = left
        if (side) 
        {
            selectedBait += 1;
            if (selectedBait == 4)
            {
                selectedBait = 0;
            }
        }
        else
        {
            selectedBait -= 1;
            if (selectedBait == -1)
            {
                selectedBait = 3;
            }
        }

        switch (selectedBait) 
        {
            case 0:
                displayedBait.sprite = baitPictures[0];
                amountOfBait.text = "∞";
                break;
            case 1:
                displayedBait.sprite = baitPictures[1];
                amountOfBait.text = MedmiumBait.ToString();
                break;
            case 2:
                displayedBait.sprite = baitPictures[2];
                amountOfBait.text = HardBait.ToString();
                break;
            case 3:
                displayedBait.sprite = baitPictures[3];
                amountOfBait.text = BossBait.ToString();
                break;
        }
    }
}
