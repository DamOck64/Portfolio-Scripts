using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class weaponselect : MonoBehaviour
{
    public static weaponselect Instance;

    [SerializeField] private Animator _animator;
    public int id;
    [SerializeField] private GameObject leftCycle, rightCycle;
    [SerializeField] private TextMeshProUGUI GoldAmount;

    [SerializeField] private Button targetButton;
    [SerializeField] private Sprite[] buttonSprites;
    // 0 = target equip, 1 = highlight equip, 2 = pressed equip.
    // 3 = target buy, 4 = highlight buy, 5 = pressed buy. 

    private SpriteState buyAble = new SpriteState();
    private SpriteState equipable = new SpriteState();
    [SerializeField] private ToolTip[] weaponPriceTagLink;
    [SerializeField] private DamageTypes DamageChanger;
    [SerializeField] private GameObject mapLock2, mapLock3;
    [SerializeField] private Button Map2, Map3;


    void Start()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        GoldAmount.text = FloatValueManager.Instance.CurrentGold.ToString();
        SetButtonSprites();
    }


    void Update()
    {
        if (id >= 0)
        {
            id = 0;
        }

        if (id <= -3)
        {
            id = -3;
        }

        if (id == 0) 
        {
        rightCycle.SetActive(false);
        }
        else
        {
            rightCycle.SetActive(true);
        }

        
        if (id == -3)
        {
            leftCycle.SetActive(false);
        }
        else
        {
            leftCycle.SetActive(true);
        }
        
    }

    public void UnlockMaps()
    {
        if (FloatValueManager.Instance.Map2Unlocked)
        {
            mapLock2.SetActive(false);
            Map2.interactable = true;
        }
        else
        {
            mapLock2.SetActive(true);
            Map2.interactable = false;
        }

        if (FloatValueManager.Instance.Map3Unlocked)
        {
            mapLock3.SetActive(false);
            Map3.interactable = true;
        }
        else
        {
            mapLock3.SetActive(true);
            Map3.interactable = false;
        }

    }

    public void SetAnimatorInteger()
    {
        _animator.SetInteger("weapon", id);
    }

    private void SetButtonSprites()
    {
        buyAble.highlightedSprite = buttonSprites[4];
        buyAble.pressedSprite = buttonSprites[5];
        equipable.highlightedSprite = buttonSprites[1];
        equipable.pressedSprite = buttonSprites[2];
    }

    public void BuyOrEquip()
    {
        targetButton.onClick.RemoveAllListeners();

        if (id == 0)
        {
                targetButton.image.sprite = buttonSprites[0];
                targetButton.spriteState = equipable;
                targetButton.onClick.AddListener(EquipWeapon);
        }

        if (id == -1)
        {
             if (!FloatValueManager.Instance.AxeOwned)
            {
                targetButton.image.sprite = buttonSprites[3];
                targetButton.spriteState = buyAble;
                weaponPriceTagLink[0].Price = "$ 400";
                targetButton.onClick.AddListener(BuyWeapon);
            }
             else
            {
                targetButton.image.sprite = buttonSprites[0];
                targetButton.spriteState = equipable;
                weaponPriceTagLink[0].Price = "$ Owned";
                targetButton.onClick.AddListener(EquipWeapon);
            }
        }

        if (id == -2)
        {
            if (!FloatValueManager.Instance.ShieldOwned)
            {
                targetButton.image.sprite = buttonSprites[3];
                targetButton.spriteState = buyAble;
                weaponPriceTagLink[1].Price = "$ 600";
                targetButton.onClick.AddListener(BuyWeapon);
            }
            else
            {
                targetButton.image.sprite = buttonSprites[0];
                targetButton.spriteState = equipable;
                weaponPriceTagLink[1].Price = "Owned";
                targetButton.onClick.AddListener(EquipWeapon);
            }
        }

            if (id == -3)
        {
            if (!FloatValueManager.Instance.SpearOwned)
            {
                targetButton.image.sprite = buttonSprites[3];
                targetButton.spriteState = buyAble;
                weaponPriceTagLink[2].Price = "$ 800";
                targetButton.onClick.AddListener(BuyWeapon);
            }
            else
            {
                targetButton.image.sprite = buttonSprites[0];
                targetButton.spriteState = equipable;
                weaponPriceTagLink[2].Price = "Owned";
                targetButton.onClick.AddListener(EquipWeapon);
            }
        }

    }

    public void Right()
    {
            id += 1;
            _animator.SetInteger("weapon", id);
        BuyOrEquip();

    }

    public void Left()
    {
            id -= 1;
            _animator.SetInteger("weapon", id);
        BuyOrEquip();
    }

    public void EquipWeapon()
    {
        switch(id) 
        {
            case 0:
                FloatValueManager.Instance.selectedWeapon = Weapon.Sword;
                DamageChanger.DamageTable[0].DamageModifier = 0.45f;
                DamageChanger.DamageTable[1].DamageModifier = 0.95f;
                break;

            case -1:
                FloatValueManager.Instance.selectedWeapon = Weapon.Axe;
                DamageChanger.DamageTable[0].DamageModifier = 0.6f;
                DamageChanger.DamageTable[1].DamageModifier = 1.15f;
                break;

            case -2:
                 FloatValueManager.Instance.selectedWeapon = Weapon.Shield;
                DamageChanger.DamageTable[0].DamageModifier = 0.25f;
                DamageChanger.DamageTable[1].DamageModifier = 1.4f;
                break;

            case -3:
                FloatValueManager.Instance.selectedWeapon = Weapon.Spear;
                DamageChanger.DamageTable[0].DamageModifier = 0.3f;
                DamageChanger.DamageTable[1].DamageModifier = 0.7f;
                break;
        }
    }

    public void BuyWeapon()
    {
        if (id == -1 && FloatValueManager.Instance.CurrentGold >= 400 && !FloatValueManager.Instance.AxeOwned)
        {
            FloatValueManager.Instance.AxeOwned = true;
            StartCoroutine(DepleteMoney(400));
        }


        if (id == -2 && FloatValueManager.Instance.CurrentGold >= 600 && !FloatValueManager.Instance.ShieldOwned)
        {
            FloatValueManager.Instance.ShieldOwned = true;
            StartCoroutine(DepleteMoney(600));
        }

        if (id == -3 && FloatValueManager.Instance.CurrentGold >= 800 && !FloatValueManager.Instance.SpearOwned)
        {
            FloatValueManager.Instance.SpearOwned = true;
            StartCoroutine(DepleteMoney(800));
        }
    }


    private IEnumerator DepleteMoney(float money)
    {
        for (int i = 0; i < money; i+=2)
        {
            FloatValueManager.Instance.CurrentGold -= 2;
            GoldAmount.text = FloatValueManager.Instance.CurrentGold.ToString();
            yield return new WaitForSeconds(0.0020f);
        }
        BuyOrEquip();
        yield break;
    }
}
