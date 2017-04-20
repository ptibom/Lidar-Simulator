/*
* @author: Philip Tibom
*/

using UnityEngine;

/// <summary>
/// Author: Philip Tibom
/// Ray casts and simulates individual lasers.
/// </summary>
public class Laser {
    private int laserId;
    private Ray ray;
    private RaycastHit hit;
    private bool isHit;
    private float rayDistance;
    private float verticalAngle;
    private GameObject parentObject;
    private RenderLine lineDrawer;
    private float offset;

    public Laser(GameObject parent, float verticalAngle, float distance, float offset, GameObject lineDrawer, int laserId)
    {
        this.laserId = laserId;
        parentObject = parent;
        this.offset = offset;
        this.verticalAngle = verticalAngle;
        rayDistance = distance;
        this.lineDrawer = lineDrawer.GetComponent<RenderLine>();
        lineDrawer.transform.position = parentObject.transform.position + (parentObject.transform.up * offset);
        ray = new Ray();
        UpdateRay();
    }

    // Should be called from Update(), for best performance.
    // This is only visual, for debugging.
    public void DrawRay()
    {
        if (isHit)
        {
            lineDrawer.DrawLine(hit.point);
        }
        else
        {
            lineDrawer.DrawLine(ray.GetPoint(rayDistance));
        }
    }

    public void DebugDrawRay()
    {
        float distance = rayDistance;
        if (isHit)
        {
            distance = hit.distance;
        }
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
    }

    // Should be called from FixedUpdate(), for best performance.
    public RaycastHit ShootRay()
    {
        // Perform raycast
        UpdateRay();

        isHit = Physics.Raycast(ray, out hit, rayDistance);
        DrawRay();

        if (isHit)
        {
            return hit;
        }
        return new RaycastHit();
    }

    // Update existing ray. Don't create 'new' ray object, that is heavy.
    private void UpdateRay()
    {
        Quaternion q = Quaternion.AngleAxis(verticalAngle, Vector3.right);
        Vector3 direction = parentObject.transform.TransformDirection(q * Vector3.forward);
        ray.origin = parentObject.transform.position + (parentObject.transform.up * offset);
        ray.direction = direction;
    }

    public Ray GetRay()
    {
        return ray;
    }

    public float GetVerticalAngle()
    {
        return verticalAngle;
    }

    public int GetLaserId()
    {
        return laserId;
    }

    public RenderLine GetRenderLine()
    {
        return lineDrawer;
    }
}
