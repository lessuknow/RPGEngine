using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShopControls : MonoBehaviour {

    [SerializeField] ShopUI sc;
    [SerializeField] private Image[] selections_image;
    [SerializeField] private Text[] selections_text;

    private bool inShop = false;
    private int curPosition = 0;

    public void EnterShop()
    {
        inShop = true;
    }

    public void LeaveShop()
    {
        inShop = false;
    }

    private void Update()
    {
        if(inShop)
        {
            if(Input.GetKeyDown(KeyCode.S))
            {
                if(curPosition!=(selections_image.Length-1))
                {
                    curPosition++;
                }
            }
        }
    }

}
