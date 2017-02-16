using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser {
    private Ray ray;
    private RaycastHit hit;
    private bool isHit;
    private float rayDistance;
    private float verticalAngle;
    private GameObject parentObject;

    public Laser(GameObject parent, float verticalAngle, float distance)
    {
        parentObject = parent;
        this.verticalAngle = verticalAngle;
        rayDistance = distance;
        ray = new Ray();
        UpdateRay();
    }

    // Should be called from Update(), for best performance.
    // This is only visual, for debugging.
    public void DrawRay()
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
        
        // For future reference
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
        ray.origin = parentObject.transform.position;
        ray.direction = direction;
    }

    public Ray GetRay()
    {
        return ray;
    }
}
