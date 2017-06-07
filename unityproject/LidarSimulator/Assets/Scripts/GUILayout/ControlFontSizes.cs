using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Syncs the size of all passed in texts
/// 
/// @author: Jonathan Jansson
/// </summary>
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
    
    /// <summary>
    /// Adds the passed in texts to a list.
    /// As the "Best Fit" function of the Text objects are run after the "Start" method of all 
    /// scripts, this script Invokes the call to adjust according to the sizes with a delay which
    /// will execute the method during the next frame and thus after the "Best Fit" functions is run
    /// </summary>
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
	
    /// <summary>
    /// This method sets the size of every text in "texts" to the size of the smallest size among the texts in "texts"
    /// </summary>
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
