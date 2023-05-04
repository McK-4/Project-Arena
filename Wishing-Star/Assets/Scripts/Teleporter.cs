using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]GameObject exit;
    [SerializeField]int layer;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = exit.transform.position;
            StartCoroutine(collision.gameObject.GetComponent<PlayerController>().Teleported(layer));
        }
    }
}
