using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobUI : MonoBehaviour
{
    [SerializeField] private Image hpBar;
    // Use this for initialization
    void Start()
    {   
        hpBar.type = Image.Type.Filled;
        hpBar.fillMethod = Image.FillMethod.Horizontal;

    }

    public void SetHPAmount(float x)
    {
        hpBar.fillAmount = x;
    }



    // Update is called once per frame
    void Update()
    {

    }
}
