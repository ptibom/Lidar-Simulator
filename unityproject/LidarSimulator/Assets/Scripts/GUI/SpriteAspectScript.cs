using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAspectScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector2 imageSize = gameObject.GetComponent<Image> ().sprite.rect.size;
		gameObject.GetComponent<AspectRatioFitter> ().aspectRatio = imageSize.x / imageSize.y;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
