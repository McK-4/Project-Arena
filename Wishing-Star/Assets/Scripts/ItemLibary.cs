using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLibary : MonoBehaviour
{
    //PlayerController PlayerController;

    public string itemTag;

    //Bow
    [SerializeField] GameObject arrow;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemLibFind(string tag, Vector2 direction, Vector2 pos)
    {
        
        switch (itemTag)
        {
            case "Bow":
                //Bow(direction, pos);
                break;

            case "Bomb":
                break;

            default:
                Debug.Log("Nothing");
                break;
        }
        
    }
    /*
    private void Bow(Vector2 direction, Vector2 pos)
    {
        if(direction == new Vector2 (0, 1))
        {

        }
        else if (direction == new Vector2(0, -1))
        {

        }
        else if (direction == new Vector2(1, 0))
        {

        }
        else if (direction == new Vector2(-1, 0))
        {

        }

        GameObject a = Instantiate(arrow, pos, );
    }
    */

}
