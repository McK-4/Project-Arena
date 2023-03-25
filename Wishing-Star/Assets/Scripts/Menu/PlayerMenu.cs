using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerMenu : MonoBehaviour
{
    private MenuManager menuManager;

    public Vector2 playerSelectInput;


    public List<Sprite> playerArtList;
    [SerializeField] Image playerArt;
    public int currentArtNum;
    public bool skinSelected;
    [SerializeField] TextMeshProUGUI readyText;

    //Menus
    [SerializeField] GameObject mapSelectMenu;
    [SerializeField] GameObject playerSelectMenu;

    void Start()
    {
        menuManager = GameObject.Find("Menu Manager").GetComponent<MenuManager>();
    }

    void Update()
    {

    }

    public void PlayerSelect(InputAction.CallbackContext context)
    {
        playerSelectInput = context.ReadValue<Vector2>();

        if (playerSelectMenu && skinSelected == false)
        {
            if (playerSelectInput.y > 0)
            {
                if (currentArtNum != playerArtList.Count - 1)
                {
                    currentArtNum++;
                }
                else if (currentArtNum == playerArtList.Count - 1)
                {
                    currentArtNum = 0;
                }


                playerArt.sprite = playerArtList[currentArtNum];
            }
            else if (playerSelectInput.y < 0)
            {
                if (currentArtNum != 0)
                {
                    currentArtNum--;
                }
                else if (currentArtNum == 0)
                {
                    currentArtNum = playerArtList.Count - 1;
                }


                playerArt.sprite = playerArtList[currentArtNum];
            }
        }

        if (mapSelectMenu)
        {

        }
    }

    public void Ready(InputAction.CallbackContext context)
    {
        if (playerSelectMenu)
        {
            if (context.performed && skinSelected == false)
            {
                skinSelected = true;
                readyText.text = "- Ready -";
                menuManager.playersReady++;
            }
            else if (context.performed && skinSelected == true)
            {
                skinSelected = false;
                readyText.text = "- Press Blue -";
                menuManager.playersReady--;
            }
        }

        if (mapSelectMenu)
        {

        }
    }
}
