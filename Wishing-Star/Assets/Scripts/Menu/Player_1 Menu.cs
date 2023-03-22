using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_1Menu : MonoBehaviour
{
    private MenuManager menuManager;

    [SerializeField] GameObject playerSelectMenu;
    //public List<Sprite> playerArtList;
    //private Image playerArt;
    //private Vector2 playerSelectInput;
    public int currentArtNum;

    // Start is called before the first frame update
    void Start()
    {
        menuManager = GameObject.Find("Menu Manager").GetComponent<MenuManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerSelectMenu)
        {
            if (menuManager.playerSelectInput.y > 0)
            {
                if (currentArtNum <= 0)
                    currentArtNum++;

                menuManager.playerArt.sprite = menuManager.playerArtList[currentArtNum];
            }
            else if (menuManager.playerSelectInput.y < 0)
            {
                if (currentArtNum >= menuManager.playerArtList.Count - 1)
                    currentArtNum--;

                menuManager.playerArt.sprite = menuManager.playerArtList[currentArtNum];
            }
        }
    }
}
