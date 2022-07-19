using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{

    public Image Bar;
    [Range(0f, 1f)]
    public float fill;
    
    void Start()
    {
        fill = 0.5f;   
    }

    // Update is called once per frame
    void Update()
    {
        Bar.fillAmount = fill;
    }

    public void TakeDamage(float damage)
    {
    }
}
