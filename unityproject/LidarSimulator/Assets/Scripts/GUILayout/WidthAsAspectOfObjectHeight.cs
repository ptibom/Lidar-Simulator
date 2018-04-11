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

/// <summary>
/// Sets the width of the RectTransform to an aspect of the squareroot of the height of the passed in object.
/// The reason for taking the squareroot of the height is so that, when changing to resolutions with different 
/// aspect ratios, the height difference will have less impact on the width. Thus not streching the widht too much.
/// 
/// @author: Jonathan Jansson
/// </summary>
public class WidthAsAspectOfObjectHeight : MonoBehaviour {

    public float aspect = 1f;
    public RectTransform objectRectTransform;

	void Start () {
        SetWidthAsAspectOfObject();
	}
	
    void SetWidthAsAspectOfObject()
    {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(Mathf.Sqrt(objectRectTransform.rect.height) * aspect, rectTransform.rect.height);
    }
}
