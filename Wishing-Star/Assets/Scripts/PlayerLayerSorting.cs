using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class PlayerLayerSorting : MonoBehaviour
{

    SerializedProperty pName;
    SerializedProperty pOrder;

    private SpriteRenderer rend;
    void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
