using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairMove : MonoBehaviour
{
    private Vector3 objPosition;
    private void Start()
    {
        Cursor.visible = false;
    }
    void Update()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f); // переменной записываються координаты мыши по иксу и игрику
        objPosition = Camera.main.ScreenToWorldPoint(mousePosition); // переменной - объекту присваиваеться переменная с координатами мыши
        
    }

    private void FixedUpdate()
    {
        transform.position = objPosition;
    }
}
