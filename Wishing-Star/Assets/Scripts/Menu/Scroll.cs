using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
    Material material;
    Vector2 offSet;
    public float speedY = 0.2f;
    public float speedX = 0.2f;

    private void Awake()
    {
        material = GetComponent<Image>().material;
        material.mainTextureOffset = Vector2.zero;
    }

    void FixedUpdate()
    {
        offSet = new Vector2(speedX, speedY);

        material.mainTextureOffset += offSet * Time.deltaTime;
    }
}
