using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveConfirm : MonoBehaviour {
   
    public static bool confirm = false;
    // Use this for initialization
    private void OnGUI()
    {
        
        GUI.BeginGroup(new Rect((Screen.width - 400) / 2, (Screen.height - 200) / 2, 400, 200));
        GUI.Box(new Rect(0, 0, 400, 200), "");
        GUI.Label(new Rect((400 - 230) / 2, (200 - 30) / 2, 230,30), "Do you want to save?");
        if (GUI.Button(new Rect((400 - 230) / 2, (200 - 30) / 2 + 40, 100, 30), "Save")) { confirm = true; gameObject.SetActive(false);};
        if (GUI.Button(new Rect((400 - 230) / 2 + 130, (200 - 30) / 2 + 40, 100, 30), "Cancel")) { gameObject.SetActive(false);};

        GUI.EndGroup();
    }
}
