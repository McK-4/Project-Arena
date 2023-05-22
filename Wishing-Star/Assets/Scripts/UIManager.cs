using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using static UnityEditor.PlayerSettings;

public class UIManager : MonoBehaviour
{
    [SerializeField]Image icon;
    [SerializeField]Image manaFill;
    [SerializeField]Image manaBar;
    [SerializeField]List<Sprite> manaBarSprites;
    [SerializeField]GameObject player;
    PlayerController pC;
    [SerializeField]List<Image> healthIconsP1;
    [SerializeField]List<Sprite> healthSprites;
    [SerializeField]List<Image> buttons;
    [SerializeField]List<Sprite> buttonSprites;
    [SerializeField]List<Image> itemIcons;
    [SerializeField]List<Sprite> itemSprites;
    [SerializeField]Image swordIcon;
    [SerializeField]Sprite swordUpgrade, swordSprite;
    [SerializeField]Image shieldIcon;
    [SerializeField]Sprite shieldUpgrade, shieldSprite;
    [SerializeField]TextMeshProUGUI pointsText;

    float mana;
    float manaFillAmount;
    int hP;
    float mHP;
    int points;
    int activeBar = 1;
    string item1;
    string item2;
    bool sword;
    bool shield;
    bool manaUpgrade;

    void Start()
    {
        pC = player.GetComponent<PlayerController>();
        hP = pC.health;
        mHP = pC.maxHealth;
        mana = pC.mana;
        manaFillAmount = mana / pC.manaMax;
        points = pC.points;
        UIUpdate();
    }

    void Update()
    {
        if (!player.activeInHierarchy)
        {
            icon.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
        if (hP != pC.health || mHP != pC.maxHealth || points != pC.points || mana != pC.mana || item1 != pC.itemTag1 || item2 != pC.itemTag2 || sword != pC.bladeUp || manaUpgrade != pC.manaUp || shield != pC.shieldUpgrade)
        {
            hP = pC.health;
            mHP = pC.maxHealth;
            mana = pC.mana;
            manaFillAmount = mana / pC.manaMax;
            points = pC.points;
            UIUpdate();
        }

        var screenPos = player.transform.position;
        screenPos.y += 1.5f;
        icon.transform.position = screenPos;
    }

    void UIUpdate()
    {
        sword = pC.bladeUp;
        if (sword)
        {
            swordIcon.sprite = swordUpgrade;
        }
        else
        {
            swordIcon.sprite = swordSprite;
        }
        manaUpgrade = pC.manaUp;
        if (manaUpgrade)
        {
            activeBar = 2;
        }
        shield = pC.shieldUpgrade;
        if (shield)
        {
            shieldIcon.sprite = shieldUpgrade;
        }
        else
        {
            shieldIcon.sprite = shieldSprite;
        }

        item1 = pC.itemTag1;
        item2 = pC.itemTag2;
        switch (item1)
        {
            case "Bow":
                itemIcons[0].sprite = itemSprites[0];
                itemIcons[0].gameObject.SetActive(true);
                break;
            case "Bomb":
                itemIcons[0].sprite = itemSprites[1];
                itemIcons[0].gameObject.SetActive(true);
                break;
            case "Dark Leech":
                itemIcons[0].sprite = itemSprites[2];
                itemIcons[0].gameObject.SetActive(true);
                break;
            case "Glove of Thunder":
                itemIcons[0].sprite = itemSprites[3];
                itemIcons[0].gameObject.SetActive(true);
                break;
            case "Tome of Ash":
                itemIcons[0].sprite = itemSprites[4];
                itemIcons[0].gameObject.SetActive(true);
                break;
            case "Invisibility Mask":
                itemIcons[0].sprite = itemSprites[5];
                itemIcons[0].gameObject.SetActive(true);
                break;
            default:
                itemIcons[0].sprite = null;
                itemIcons[0].gameObject.SetActive(false);
                break;
        }

        switch (item2)
        {
            case "Bow":
                itemIcons[1].sprite = itemSprites[0];
                itemIcons[1].gameObject.SetActive(true);
                break;
            case "Bomb":
                itemIcons[1].sprite = itemSprites[1];
                itemIcons[1].gameObject.SetActive(true);
                break;
            case "Dark Leech":
                itemIcons[1].sprite = itemSprites[2];
                itemIcons[1].gameObject.SetActive(true);
                break;
            case "Glove of Thunder":
                itemIcons[1].sprite = itemSprites[3];
                itemIcons[1].gameObject.SetActive(true);
                break;
            case "Tome of Ash":
                itemIcons[1].sprite = itemSprites[4];
                itemIcons[1].gameObject.SetActive(true);
                break;
            case "Invisibility Mask":
                itemIcons[1].sprite = itemSprites[5];
                itemIcons[1].gameObject.SetActive(true);
                break;
            default:
                itemIcons[1].gameObject.SetActive(false);
                break;
        }

        for (int i = healthIconsP1.Count - 1; i >= 0; i--)
        {
            //checking if hp is greater than the max hp there.
            //ex : heart 1 is 2 and hp is 9, therefor it runs and gives us a full heart
            if (mHP > (i) * (healthSprites.Count - 1))
            {
                healthIconsP1[i].GetComponent<Transform>().gameObject.SetActive(true);
                if (hP > (i + 1) * (healthSprites.Count - 1))
                {
                    healthIconsP1[i].sprite = healthSprites[healthSprites.Count - 1];
                }
                else if (hP - (i) * (healthSprites.Count - 1) >= 0)
                {
                    healthIconsP1[i].sprite = healthSprites[hP - (i) * (healthSprites.Count - 1)];
                }
                else
                {
                    healthIconsP1[i].sprite = healthSprites[0];
                }
            }
            else
            {
                healthIconsP1[i].GetComponent<Transform>().gameObject.SetActive(false);
            }
        }
        //Points UI
        pointsText.text = points.ToString();

        //Mana UI
        if(mana > 0)
        {
            manaBar.sprite = manaBarSprites[activeBar];
        }
        else if(mana <= 0)
        {
            manaBar.sprite = manaBarSprites[0];
        }

        manaFill.fillAmount = manaFillAmount;

    }

    public void Button1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            buttons[0].sprite = buttonSprites[1];
        }
        if (context.canceled)
        {
            buttons[0].sprite = buttonSprites[0];
        }
    }

    //Button Presses
    public void Button2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            buttons[1].sprite = buttonSprites[3];
        }
        if (context.canceled)
        {
            buttons[1].sprite = buttonSprites[2];
        }
    }

    public void Button3(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            buttons[2].sprite = buttonSprites[5];
        }
        if (context.canceled)
        {
            buttons[2].sprite = buttonSprites[4];
        }
    }

    public void Button4(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            buttons[3].sprite = buttonSprites[7];
        }
        if (context.canceled)
        {
            buttons[3].sprite = buttonSprites[6];
        }
    }
}
