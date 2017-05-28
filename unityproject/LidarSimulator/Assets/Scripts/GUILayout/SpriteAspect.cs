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
