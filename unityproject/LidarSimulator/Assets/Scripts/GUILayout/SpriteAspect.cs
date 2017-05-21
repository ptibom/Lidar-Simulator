using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A script which sets the aspect ratio of an AspectRatioFitter component as the aspect ratio of an image
/// 
/// @author: Jonathan Jansson
/// </summary>
public class SpriteAspect : MonoBehaviour {

  
    /// <summary>
    /// Sets the initial aspect ratio as image of component
    /// </summary>
    void Start ()
    {
        if (gameObject.GetComponent<Image>() && gameObject.GetComponent<AspectRatioFitter>())
        {
            SetAspectRatioAsImage(gameObject.GetComponent<Image>());
        }
        else
        {
            Debug.Log("GameObject has no Image or no AspectRatoFitter");
        }
    }

    void SetAspectRatioAsImage(Image image)
    {
        Vector2 imageSize = image.sprite.rect.size;
        gameObject.GetComponent<AspectRatioFitter>().aspectRatio = imageSize.x / imageSize.y;
    }
}
