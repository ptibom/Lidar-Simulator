using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlFontSizes : MonoBehaviour {

    public Text t1;
    public Text t2;
    public Text t3;
    public Text t4;
    public Text t5;
    public Text t6;
    public Text t7;
    public Text t8;
    public Text t9;

    LinkedList<Text> texts = new LinkedList<Text>();
    
    void Start () {
        texts.AddLast(t1);
        texts.AddLast(t2);
        texts.AddLast(t3);
        texts.AddLast(t4);
        texts.AddLast(t5);
        texts.AddLast(t6);
        texts.AddLast(t7);
        texts.AddLast(t8);
        texts.AddLast(t9);

        Invoke("SetMaxFontSizeAsSmallestFontDoubleRows", 0.01f);
    }
	
	void SetMaxFontSizeAsSmallestFontDoubleRows () {
        int smallestFontSize = 50;
        foreach (Text t in texts)
        {
            if(t.cachedTextGenerator.fontSizeUsedForBestFit < smallestFontSize)
            {
                smallestFontSize = t.cachedTextGenerator.fontSizeUsedForBestFit;
            }
        }

        foreach(Text t in texts)
        {
            t.resizeTextForBestFit = false;
            t.fontSize = smallestFontSize;
        }
	}
}
