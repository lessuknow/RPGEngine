using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour {

    [SerializeField]
    private Text displayName;
    [SerializeField] private Image hpBar;
    [SerializeField] private Image mpBar;

	// Use this for initialization
	void Start () {
        displayName.text = "Testing";
        hpBar.type = Image.Type.Filled;
        mpBar.type = Image.Type.Filled;
        hpBar.fillMethod = Image.FillMethod.Horizontal;
        mpBar.fillMethod = Image.FillMethod.Horizontal;
        
    }
	
    public void SetHPAmount(float x)
    {
        hpBar.fillAmount = x;
    }

    public void SetMPAmount(float x)
    {
        mpBar.fillAmount = x;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
