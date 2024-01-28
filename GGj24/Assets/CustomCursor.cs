using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Vector2 offset;

    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite onClick;
    [SerializeField] private Image image;


    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition + new Vector3(offset.x,offset.y,0);
        UnityEngine.Cursor.visible = false;
        if (Input.GetMouseButtonDown(0))
        {
            image.sprite = onClick;

        }
        if (Input.GetMouseButtonUp(0))
        {
            image.sprite = normal;
        }
    }
}
