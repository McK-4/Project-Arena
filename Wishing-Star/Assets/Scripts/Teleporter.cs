using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]GameObject exit;
    [SerializeField]int layer;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.transform.position = exit.transform.position;
            StartCoroutine(collision.gameObject.GetComponent<PlayerController>().Teleported(layer));
        }
    }
}
