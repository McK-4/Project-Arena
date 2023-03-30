using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
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
    [SerializeField]TextMeshProUGUI pointsText;

    float mana;
    float manaFillAmount;
    int hP;
    int mHP;
    int points;

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
        if (player == null)
        {
            icon.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        if (hP != pC.health || mHP != pC.maxHealth || points != pC.points || mana != pC.mana)
        {
            hP = pC.health;
            mHP = pC.maxHealth;
            mana = pC.mana;
            manaFillAmount = mana / pC.manaMax;
            points = pC.points;
            UIUpdate();
        }

        var screenPos = Camera.main.WorldToScreenPoint(player.transform.position);
        screenPos.y += 60;
        icon.transform.position = screenPos;
    }

    void UIUpdate()
    {
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
        pointsText.text = points.ToString();

        if(mana <= 0)
        {
            manaBar.sprite = manaBarSprites[2];
        }
        else if(mana > 0)
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
