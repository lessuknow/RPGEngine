using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionUI : MonoBehaviour {

    public Text[] selectText;

    public void SetSelection(List<string> names)
    {
        int comp = Mathf.Min(selectText.Length, names.Count);
        for(int i=0;i<comp;i++)
        {
            selectText[i].text = names[i];
        }
        for (int i = comp; i < selectText.Length;i++)
        {
            selectText[i].text = "";
        }
    }

}
    