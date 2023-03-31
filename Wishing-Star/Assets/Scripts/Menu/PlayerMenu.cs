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

    public InputDevice inputDevice;

    public List<Sprite> playerArtList;
    [SerializeField] Image playerArt;
    public int currentArtNum;
    public bool skinSelected;
    [SerializeField] TextMeshProUGUI readyText;

    [SerializeField] List<GameObject> maps;
    [SerializeField] List<GameObject> readyMarks;
    public GameObject mark;
    public int activeMap;
    public bool mapSelected;

    //Menus
    public bool mapSelectMenu;
    public bool playerSelectMenu;

    void Start()
    {
        menuManager = GameObject.Find("Menu Manager").GetComponent<MenuManager>();
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

        if (mapSelectMenu && !mapSelected)
        {
            if (playerSelectInput.x > 0)
            {
                if (activeMap == 0)
                {
                    activeMap = 1;
                    mark.transform.position = maps[activeMap].transform.position;
                }
            }
            else if (playerSelectInput.x < 0)
            {
                if (activeMap == 1)
                {
                    activeMap = 0;
                    mark.transform.position = maps[activeMap].transform.position;
                }
            }
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
                inputDevice = gameObject.GetComponent<PlayerInput>().devices[0].device;
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
            if (context.performed && mapSelected == false)
            {
                mapSelected = true;
                readyMarks[activeMap].SetActive(true);
                mark.SetActive(false);
                menuManager.playersVoted++;
                //menuManager.MapVote(activeMap, 1);
            }
            else if (context.performed && mapSelected == true)
            {
                mapSelected = false;
                readyMarks[activeMap].SetActive(false);
                mark.SetActive(true);
                menuManager.playersVoted--;
                //menuManager.MapVote(activeMap, -1);
            }
        }
    }
}
