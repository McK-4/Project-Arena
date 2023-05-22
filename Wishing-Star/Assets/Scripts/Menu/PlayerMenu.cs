using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using TMPro;

public class PlayerMenu : MonoBehaviour
{
    private MenuManager menuManager;

    public Vector2 playerSelectInput;

    public InputDevice inputDevice;
    PlayerInput pI;

    public List<Sprite> playerArtList;
    [SerializeField] Image playerArt;
    public int currentArtNum;
    public bool skinSelected;
    [SerializeField] Image box;
    [SerializeField] List<Sprite> playerBox;
    [SerializeField] Color selected;
    [SerializeField] List<Image> arrows;

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
        pI = gameObject.GetComponent<PlayerInput>();
        pI.user.UnpairDevices();
    }

    public void PlayerSelect(InputAction.CallbackContext context)
    {
        playerSelectInput = context.ReadValue<Vector2>();

        if (playerSelectMenu && skinSelected == false)
        {
            if (playerSelectInput.y > 0)
            {
                arrows[0].color = Color.white;
                arrows[1].color = selected;

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
                arrows[1].color = Color.white;
                arrows[0].color = selected;

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
            else if (playerSelectInput.y == 0)
            {
                arrows[0].color = Color.white;
                arrows[1].color = Color.white;
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
                    menuManager.ButtonPressed.Play();
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
                box.sprite = playerBox[1];
                arrows[0].color = Color.white;
                arrows[1].color = Color.white;
                menuManager.playersReady++;
                inputDevice = gameObject.GetComponent<PlayerInput>().devices[0].device;
                menuManager.ButtonPressed.Play();
            }
            else if (context.performed && skinSelected == true)
            {
                skinSelected = false;
                box.sprite = playerBox[0];
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

    public void input(InputDevice device)
    {
        InputUser.PerformPairingWithDevice(device, pI.user, InputUserPairingOptions.UnpairCurrentDevicesFromUser);
        pI.SwitchCurrentControlScheme(device);
    }
}
