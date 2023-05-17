using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Conveyor : MonoBehaviour
{
    enum Directions{Up,Down,Right,Left}
    [SerializeField]Directions conveyorDirection;
    Vector2 direction;
    [SerializeField]float force;
    void Start()
    {
        if (conveyorDirection == Directions.Up){direction = Vector2.up;}
        else if (conveyorDirection == Directions.Down){direction = Vector2.down;}
        else if (conveyorDirection == Directions.Left){direction = Vector2.left;}
        else if (conveyorDirection == Directions.Right){direction = Vector2.right;}
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bomb" || collision.gameObject.tag == "Power Up" || collision.gameObject.tag == "Pick Up")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Force);
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bomb" || collision.gameObject.tag == "Power Up" || collision.gameObject.tag == "Pick Up")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Force);
        }
    }
}
