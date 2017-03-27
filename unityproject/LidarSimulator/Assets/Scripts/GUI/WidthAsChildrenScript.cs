using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WidthAsChildrenScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        float maxWidth = 0;
        for(int i =0; i< transform.childCount; i++)
        {
            Debug.Log("HEj" + transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.y);
            //if (childRectT.sizeDelta.x > maxWidth)
            {
              //  maxWidth = childRectT.sizeDelta.x;
            }
        }

        gameObject.GetComponent<LayoutElement>().preferredWidth = maxWidth;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
