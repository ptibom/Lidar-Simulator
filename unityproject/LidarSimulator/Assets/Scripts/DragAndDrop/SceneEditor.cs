/*
* @author: Philip Tibom
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneEditor : MonoBehaviour {
    private GameObject go;
    private bool moveGameObject;
    private bool rotateGameObject;
    private float previousMousePos;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Check if mouse is not over UI. And if we supposed to move object.
		if (moveGameObject && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Bounds bounds = go.GetComponent<MeshRenderer>().bounds;
                go.transform.position = hit.point + new Vector3(0, bounds.size.y / 2);
                if (Input.GetMouseButtonDown(0))
                {
                    moveGameObject = false;
                    rotateGameObject = true;
                }
                if (Input.GetMouseButtonDown(1))
                {
                    Destroy(go);
                    moveGameObject = false;
                }
            }
        }

        else if (rotateGameObject)
        {
            if (previousMousePos != 0)
            {
                go.transform.Rotate(Vector3.up, previousMousePos - Input.mousePosition.x);
            }
            previousMousePos = Input.mousePosition.x;

            if (Input.GetMouseButtonDown(0))
            {
                SetAlpha(1f);
                ActivateColliders();
                previousMousePos = 0;
                rotateGameObject = false;
            }
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(go);
                previousMousePos = 0;
                rotateGameObject = false;
            }
        }
	}

    public void PlaceObject(GameObject prefab)
    {
        MoveObject(Instantiate(prefab));
    }

    public void MoveObject(GameObject obj)
    {
        go = obj;
        SetAlpha(0.33f);
        moveGameObject = true;
    }

    private void SetAlpha(float value)
    {
        MeshRenderer[] allParts = go.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer o in allParts)
        {
            Color color = o.material.color;
            color.a = value;
            o.material.color = color;
        }
    }

    private void ActivateColliders()
    {
        Collider[] colliders = go.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
    }
}
