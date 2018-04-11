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

using UnityEngine;	
using UnityEngine.UI;

/// <summary>
/// Syncs the values of an InputField with the Slider component of the gameObject which the script attached to and handles bounds of both components
/// 
/// @author: Jonathan Jansson
/// </summary>
public class InputSync : MonoBehaviour {

    public InputField inputField;

    private int inputFieldMaxChars;

    void Awake()
    {
        inputFieldMaxChars = inputField.characterLimit;
    }

    /// <summary>
    /// Corrects any wrong inputs from the inputfield such as 'no input' and 'out of bounds values'
    /// </summary>
	public void InputCorrection()
    {
		if(inputField.text.Equals(""))
        {
			inputField.text = "0";
		}
        else if(gameObject.GetComponent<Slider> ().maxValue < float.Parse(inputField.text))
        {
			inputField.text = gameObject.GetComponent<Slider> ().maxValue.ToString ();
		}
	}

    /// <summary>
    /// Sets the value of the slider as the value of the inputfield
    /// </summary>
	public void SyncToSlider()
    {
		gameObject.GetComponent<Slider> ().value = float.Parse(inputField.text);
    }

    /// <summary>
    /// Sets the value of the inputfield as the value of the slider with a maximum number of characters, then syncs the slider to have the same refractured value
    /// </summary>
	public void SyncToInputField()
    {
		inputField.text = SetValueLength (gameObject.GetComponent<Slider> ().value.ToString ());
        SyncToSlider();
	}

    /// <summary>
    /// Returns the passed text with a maximum of chars according to the characterlimit of the inputfield
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
	private string SetValueLength(string text)
    {
		if(text.Length > inputFieldMaxChars)
        {
			text = text.Substring (0, inputFieldMaxChars);
		}
		return text;
	}
}
