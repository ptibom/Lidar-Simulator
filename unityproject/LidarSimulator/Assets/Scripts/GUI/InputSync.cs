using UnityEngine;	
using UnityEngine.UI;

/// <summary>
/// Syncs the values of an InputField with the Slider component of the gameObject which the script attached to and handles bounds of both components
/// 
/// @author: Jonathan Jansson
/// </summary>
public class InputSync : MonoBehaviour {

    public InputField inputField;

    private int inputFieldMaxChars = 5;

    void Start()
    {
        inputFieldMaxChars = inputField.characterLimit;
    }

    /// <summary>
    /// Corrects eventual wrong inputs from the inputfield such as "no input" and "out of bounds values"
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
