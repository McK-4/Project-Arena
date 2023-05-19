using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MasterManager : MonoBehaviour
{
    GameObject[] other;

    public bool setUp;

    public InputDevice player1Input;
    public InputDevice player2Input;
    public InputDevice player3Input;
    public InputDevice player4Input;
    public InputDevice uniInput;

    public bool player1Active;
    public bool player2Active;
    public bool player3Active;
    public bool player4Active;

    public int player1Skin;
    public int player2Skin;
    public int player3Skin;
    public int player4Skin;

    void Awake()
    {
        other = GameObject.FindGameObjectsWithTag("Master");

        foreach (GameObject oneOther in other)
        {
            if (oneOther.scene.buildIndex == -1)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(transform.gameObject);
    }
}
