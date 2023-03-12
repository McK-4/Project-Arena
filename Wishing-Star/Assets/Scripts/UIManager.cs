using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]GameObject[] icons;
    [SerializeField]GameObject[] players;
    PlayerController pC1;
    [SerializeField]List<Image> healthIconsP1;
    [SerializeField]List<Sprite> healthSprites;

    int p1HP;
    int p1MHP;

    void Start()
    {
        pC1 = players[0].GetComponent<PlayerController>();
        p1HP = pC1.health;
        p1MHP = pC1.maxHealth;
    }

    void Update()
    {
        if (p1HP != pC1.health || p1MHP != pC1.maxHealth)
        {
            p1HP = pC1.health;
            p1MHP = pC1.maxHealth;
            UIUpdate();
        }
    }

    void UIUpdate()
    {
        for (int i = healthIconsP1.Count - 1; i >= 0; i--)
        {
            //checking if hp is greater than the max hp there.
            //ex : heart 1 is 2 and hp is 9, therefor it runs and gives us a full heart
            if (p1MHP > (i) * (healthSprites.Count - 1))
            {
                healthIconsP1[i].GetComponent<Transform>().gameObject.SetActive(true);
                if (p1HP > (i + 1) * (healthSprites.Count - 1))
                {
                    healthIconsP1[i].sprite = healthSprites[healthSprites.Count - 1];
                }
                else if (p1HP - (i) * (healthSprites.Count - 1) >= 0)
                {
                    healthIconsP1[i].sprite = healthSprites[p1HP - (i) * (healthSprites.Count - 1)];
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
    }
}
