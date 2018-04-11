/*
* MIT License
* 
* Copyright (c) 2017 Philip Tibom, Jonathan Jansson, Rickard Laurenius, 
* Tobias Alldén, Martin Chemander, Sherry Davar
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

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
