using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Player_1Menu : MonoBehaviour
{
    private MenuManager menuManager;

    public Vector2 playerSelectInput;

    //Player Select Menu 
    [SerializeField] GameObject playerSelectMenu;
    public List<Sprite> playerArtList;
    [SerializeField] Image playerArt;
    public int currentArtNum;

    //Map Select Menu
    [SerializeField] GameObject mapSelectMenu;



    // Start is called before the first frame update
    void Start()
    {
        menuManager = GameObject.Find("Menu Manager").GetComponent<MenuManager>();

        //Player Select Menu
        currentArtNum = 0;
        //playerArt = GameObject.Find("Player Art").GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        if (playerSelectMenu)
        {
            if (playerSelectInput.y > 0)
            {
                if (currentArtNum <= 0)
                    currentArtNum++;

                playerArt.sprite = playerArtList[currentArtNum];
            }
            else if (playerSelectInput.y < 0)
            {
                if (currentArtNum >= playerArtList.Count - 1)
                    currentArtNum--;

                playerArt.sprite = playerArtList[currentArtNum];
            }
        }

        if (mapSelectMenu)
        {

        }

    }

    public void PlayerSelect(InputAction.CallbackContext context)
    {
        playerSelectInput.y = context.ReadValue<Vector2>().y;
    }
}
