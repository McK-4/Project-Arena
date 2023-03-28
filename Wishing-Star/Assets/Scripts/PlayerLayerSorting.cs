using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class PlayerLayerSorting : MonoBehaviour
{
    public PlayerController playerCon;

    SerializedProperty pName;
    SerializedProperty pOrder;

    private SpriteRenderer rend;

    void OnEnable()
    {
        playerCon = Selection.activeGameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckRenderer()
    {
        rend = Selection.activeGameObject.GetComponent<SpriteRenderer>();

        //rend.sortingLayerName = playerCon.playerName;

        //rend.sortingOrder = playerCon.playerOrderInLayer;
    }

}