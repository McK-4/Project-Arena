using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorEnd : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bomb" || collision.gameObject.tag == "Power Up" || collision.gameObject.tag == "Pick Up")
        {
            Destroy(collision.gameObject);
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bomb" || collision.gameObject.tag == "Power Up" || collision.gameObject.tag == "Pick Up")
        {
            Destroy(collision.gameObject);
        }
    }

}
