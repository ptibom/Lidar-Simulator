using UnityEngine;
/// <summary>
/// Author: Philip Tibom
/// Handles laser rendering.
/// </summary>
public class RenderLine : MonoBehaviour {

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
    }

    /// <summary>
    /// Draws line from object origin towards direction.
    /// </summary>
    /// <param name="direction">Direction in Vector3</param>
    public void DrawLine(Vector3 direction)
    {
        gameObject.GetComponent<LineRenderer>().SetPosition(0, gameObject.transform.position);
        gameObject.GetComponent<LineRenderer>().SetPosition(1, direction);
    }
}
