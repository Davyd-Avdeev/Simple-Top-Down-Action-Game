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
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f); // ���������� ������������� ���������� ���� �� ���� � ������
        objPosition = Camera.main.ScreenToWorldPoint(mousePosition); // ���������� - ������� �������������� ���������� � ������������ ����
        
    }

    private void FixedUpdate()
    {
        transform.position = objPosition;
    }
}
