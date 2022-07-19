using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl2 : MonoBehaviour
{
    public int viewIndex = 0;
    private float angle = 0;

    void Start()
    {
        
    }

    void Update()
    {
        var direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        

        //viewIndex = Mathf.RoundToInt(angle / 45) % 8;
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    //public struct TupleAngle
    //{
    //    public TupleAngle(float minAngle, float maxAngle)
    //    {
    //        MinAngle = minAngle;
    //        MaxAngle = maxAngle;
    //    }

    //    public float MinAngle { get; set; }
    //    public float MaxAngle { get; set; }
    //}
}
