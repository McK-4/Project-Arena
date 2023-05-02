using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelect : MonoBehaviour
{
    private MenuManager menuManager;

    //private float buttonTimer = 0;
    //private float buttonCooldownTime = 0.8f;

    public List<Sprite> playerArtList;
    public Image playerArt;
    public int currentArtNum;

    //private GameObject joinText;
   // private bool joined;

    // Start is called before the first frame update
    void Start()
    {
        menuManager = GameObject.Find("Menu Manager").GetComponent<MenuManager>();

        currentArtNum = 0;
        playerArt = GameObject.Find("Player Art").GetComponent<Image>();

        //joinText = GameObject.Find("Join Text_1");
    }

    // Update is called once per frame
    void Update()
    {
        //Change this when connecting controllers work

        /* If Controller has not connected yet and joined is false
        if (joinText.activeSelf)
        {
            if (buttonTimer < buttonCooldownTime)
            {
                buttonTimer += Time.deltaTime;
                //Debug.LogError(deathTimer);
            }
            else
            {
                buttonTimer = 0;
                joinText.SetActive(false);
            }
        }

        else if (!joinText.activeSelf)
        {
            if (buttonTimer < buttonCooldownTime)
            {
                buttonTimer += Time.deltaTime;
                //Debug.LogError(deathTimer);
            }
            else
            {
                buttonTimer = 0;
                joinText.SetActive(true);
            }
        }

        If controller has connected and joined = false

         joinText.SetActive(false);
         joined = true;
         */

        //Cycling Player Select Art
        if (menuManager.playerSelectInput.y > 0)
        {
            ArtNumChange();

            playerArt.sprite = playerArtList[currentArtNum];
        }
        else if (menuManager.playerSelectInput.y < 0)
        {
            ArtNumChange();

            playerArt.sprite = playerArtList[currentArtNum];
        }
    }

    private void ArtNumChange()
    {
        if (currentArtNum <= 0)
        {
            currentArtNum++;
        }

        if (currentArtNum >= playerArtList.Count - 1)
        {
            currentArtNum--;
        }
    }

}
