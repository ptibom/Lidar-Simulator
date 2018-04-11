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
/// A script which sets the aspect ratio of an AspectRatioFitter component as the aspect ratio of an image component on the gameObject
/// 
/// @author: Jonathan Jansson
/// </summary>
public class SpriteAspect : MonoBehaviour {
  
    
    /// <summary>
    /// Checks so that the gameObject contains both an Image and an AspectRatioFitter
    /// </summary>
    void Start ()
    {
        if (gameObject.GetComponent<Image>() && gameObject.GetComponent<AspectRatioFitter>())
        {
            SetAspectRatioAsImage(gameObject.GetComponent<Image>());
        }
    }

    /// <summary>
    /// Sets the aspect ratio of the AspectRatioFitter as the aspect of the sprite of the Image component
    /// </summary>
    void SetAspectRatioAsImage(Image image)
    {
        Vector2 imageSize = image.sprite.rect.size;
        gameObject.GetComponent<AspectRatioFitter>().aspectRatio = imageSize.x / imageSize.y;
    }
}
