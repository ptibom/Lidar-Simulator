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
/// Sets the preferredWidth of the recttransform size as the same as the largest width of the children of the object
/// 
/// @author: Jonathan Jansson
/// </summary>
public class WidthAsChildren : MonoBehaviour {

    /// <summary>
    /// Waits for all other GUI initiations to be set before calling the method which changes the width
    /// </summary>
	void Start ()
    {
        if (gameObject.GetComponent<LayoutElement>())
        {
            Invoke("SetPreferedWidthAsChildren", 0.01f);
        }
    }

    /// <summary>
    /// Itterates through all children of the gameObject and sets the preferred width of the layoutelement component as the width of the widest child
    /// </summary>
	private void SetPreferedWidthAsChildren()
    {
        float maxWidth = 0f;
        
        foreach (RectTransform childRect in transform.GetComponentsInChildren<RectTransform>())
        {
            if (!childRect.name.Equals(transform.name) && childRect.rect.width + childRect.localPosition.x > maxWidth)
            {
                maxWidth = childRect.rect.width + childRect.localPosition.x;
            }
        }
        
        gameObject.GetComponent<LayoutElement>().preferredWidth = Mathf.Abs(maxWidth);
    }
}
