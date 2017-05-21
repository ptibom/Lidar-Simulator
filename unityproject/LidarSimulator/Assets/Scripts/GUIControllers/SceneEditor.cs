/*
* @author: Philip Tibom
*/

using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Author: Philip Tibom
/// Creates drag and drop functionality.
/// </summary>
public class SceneEditor : MonoBehaviour {
    public GameObject wayPoint;
    public GameObject playerCar;
	private GameObject instantiatedWayPoint;

    private GameObject go;
	private GameObject placedNavObj = null;
	private GameObject previousWayPoint = null;

    private bool isMovingGameObject = false;
    private bool isRotatingGameObject = false;
	private bool isPlacingWaypoint = false;
    private float previousMousePos;
    private float lastClick;
    
	
	// Update is called once per frame
	void Update ()
    {
        // Check if mouse is not over UI. And if we supposed to move object.
		if (isPlacingWaypoint && !EventSystem.current.IsPointerOverGameObject())
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit))
			{
				//go.transform.position = hit.point + new Vector3(0, 0.5f);
				go.transform.position = new Vector3(hit.point.x, 0.3f, hit.point.z) + new Vector3(0, 0.5f);

				if (Input.GetMouseButtonDown(0))
				{
					if (hit.collider.gameObject.GetComponent<WayPoint>() != null)
					// Click waypoint in the path.
					{
						if (previousWayPoint != null) 
						{
                            go.GetComponent<WayPoint>().SetColliderState(true);
                            go.GetComponent<WayPoint>().ClosePath(hit.collider.gameObject, previousWayPoint);
                            placedNavObj = null;
                            previousWayPoint = null;
							isPlacingWaypoint = false;
							WayPoint.SetAllColliders (true);
                            Destroy(go);
						}
					}
					else if(previousWayPoint == null)
					{	
						
						go.GetComponent<WayPoint>().SetStart(true);
						placedNavObj.GetComponent<AgentTest> ().target = go.transform;
                        go.GetComponent<WayPoint>().SetColliderState(true);
						instantiatedWayPoint = Instantiate(wayPoint);
						previousWayPoint = go;
						go = instantiatedWayPoint;
					}
					else
					{
						go.GetComponent<WayPoint>().AddToPath(previousWayPoint);
                        go.GetComponent<WayPoint>().SetColliderState(true);
                        instantiatedWayPoint = Instantiate(wayPoint);
						previousWayPoint = go;
						go = instantiatedWayPoint;
					}
					lastClick = Time.time;
				}
				if (Input.GetMouseButtonDown(1))
				{
                    // If waypoint exist, destroy and start over.
					Destroy(go);
					WayPoint.SetAllColliders (true);
					isPlacingWaypoint = false;
					placedNavObj = null;
					previousWayPoint = null;
				}
			}
		}
		else if (isMovingGameObject && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                go.transform.position = hit.point;
                if (go.GetComponent<WayPoint>() != null)
                {
                    go.transform.position = hit.point + new Vector3(0, 0.5f);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    lastClick = Time.time;
                    isMovingGameObject = false;
                    if (go.GetComponent<WayPoint>() == null)
                    {
                        isRotatingGameObject = true;
                    }
                    else
                    {
                        go.GetComponent<WayPoint>().SetColliderState(true);
                    }
                }
                if (Input.GetMouseButtonDown(1) && go != playerCar)
                {
                    if (go.GetComponent<WayPoint>() != null)
                    {
                        go.GetComponent<WayPoint>().RemoveFromPath();
                    }
                    Destroy(go);
                    isMovingGameObject = false;
                }
            }
        }
        else if (isRotatingGameObject && !EventSystem.current.IsPointerOverGameObject())
        {
            if (previousMousePos != 0)
            {
                go.transform.Rotate(Vector3.up, previousMousePos - Input.mousePosition.x);
            }
            previousMousePos = Input.mousePosition.x;

            if (Input.GetMouseButtonDown(0))
            {
                lastClick = Time.time;
                SetAlpha(1f);
                ActivateColliders(true);
                previousMousePos = 0;
                isRotatingGameObject = false;
				if (go.GetComponent<AgentTest>() != null) 
				{
					WayPoint.SetAllColliders (false);
					placedNavObj = go;
					isPlacingWaypoint = true;
                    go = Instantiate(wayPoint);

                }
            }
            if (Input.GetMouseButtonDown(1) && go != playerCar)
            {
                Destroy(go);
                previousMousePos = 0;
                isRotatingGameObject = false;
            }
        }
        // Move existing game object.
        else if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButton(0) && lastClick + 0.2 < Time.time)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("EditableObject")) // Object is head?
                {
                    lastClick = Time.time;
                    MoveObject(hit.collider.gameObject);
                }
                else // Check if parent has EditableObject tag.
                {
                    GameObject o = FindParentEditableObject(hit.collider.gameObject);
                    if (o != null && o.CompareTag("EditableObject"))
                    {
                        lastClick = Time.time;
                        MoveObject(o);
                    }
                }
            }
        }
	}

    /// <summary>
    /// Instantiates selected prefab.
    /// </summary>
    /// <param name="prefab">A game object prefab</param>
    public void PlaceObject(GameObject prefab)
    {
        if (!isMovingGameObject && !isRotatingGameObject)
        {
            MoveObject(Instantiate(prefab));
        }
    }

    /// <summary>
    /// Enables movement of game object. Following the mouse.
    /// </summary>
    /// <param name="obj">A game object.</param>
    private void MoveObject(GameObject obj)
    {
        if (!isMovingGameObject && !isRotatingGameObject)
        {
            if (obj.GetComponent<AgentTest>() != null && obj.GetComponent<AgentTest>().target != null)
            {
                obj.GetComponent<AgentTest>().target.gameObject.GetComponent<WayPoint>().RemovePath();
            }
            go = obj;
            SetAlpha(0.33f);
            isMovingGameObject = true;
            ActivateColliders(false);
        }
    }

    /// <summary>
    /// Sets transparency of game object and child objects.
    /// </summary>
    /// <param name="value">Alpha value 0 to 1</param>
    private void SetAlpha(float value)
    {
        if (go.GetComponent<WayPoint>() != null)
        {
            return; // Don't do anything if its a waypoint.
        }
        MeshRenderer[] allParts = go.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer o in allParts)
        {
            Color color = o.material.color;
            color.a = value;
            o.material.color = color;
        }
    }

    /// <summary>
    /// Activates colliders of object and child objects.
    /// </summary>
    /// <param name="isEnabled">Enabled or not</param>
    private void ActivateColliders(bool isEnabled)
    {
        Collider[] colliders = go.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = isEnabled;
        }
    }

    /// <summary>
    /// Finds parent object tagged with "EditableObject".
    /// </summary>
    /// <param name="o">A game object</param>
    /// <returns></returns>
    private GameObject FindParentEditableObject(GameObject o)
    {
        if (o.transform.parent != null && !o.transform.parent.gameObject.CompareTag("EditableObject"))
        {
            return FindParentEditableObject(o.transform.parent.gameObject);
        }
        if (o.transform.parent == null)
        {
            return null;
        }
        return o.transform.parent.gameObject;
    }
}
