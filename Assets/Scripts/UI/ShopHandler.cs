using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShopHandler : MonoBehaviour {

    public Image shopUI;
    bool moving_shopUI = false;
    private Vector3 lerpEndPos_shopUI;
    private bool onScreen_shopUI = false;
    private float lerpTime_shopUI = 0,
        onScreen_y,
        offScreen_y;
    private float transitionTime = 5f;

    public void Awake()
    {
        //Puts the shop menu under the player's visable canvas.
        onScreen_y = shopUI.rectTransform.position.y;
        shopUI.rectTransform.position = new Vector3(Screen.width / 2, -shopUI.rectTransform.position.y, 0);
        offScreen_y = shopUI.rectTransform.position.y;
        lerpEndPos_shopUI = new Vector3(Screen.width / 2, shopUI.rectTransform.position.y, 0);
    }

    private void MoveShopUI()
    {
        if (lerpTime_shopUI < 1)
        {
            shopUI.rectTransform.position = Vector3.Lerp(shopUI.rectTransform.position,
            lerpEndPos_shopUI,
            lerpTime_shopUI);

            lerpTime_shopUI += Time.deltaTime * transitionTime;
        }
        else
        {
            //Lerp is finished here.
            moving_shopUI = false;
        }
    }

    private void Update()
    {
        if(moving_shopUI)
        {
            MoveShopUI();
        }
    }

    public void ToggleShop()
    {
        print("Toggling shop...");
        lerpTime_shopUI = 0;
        if (onScreen_shopUI == false)
        {
            lerpEndPos_shopUI.y = onScreen_y;
        }
        else
        {
            lerpEndPos_shopUI.y = offScreen_y;
        }
        onScreen_shopUI = !onScreen_shopUI;
        moving_shopUI = true;
    }

}
