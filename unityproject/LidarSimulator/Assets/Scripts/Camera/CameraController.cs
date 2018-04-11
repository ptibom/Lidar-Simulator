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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public GameObject followThis;

    public int scrollPixelMargin = 10;

    public float behindDistance = 10f;
    public float aboveheight = 5f;
    public float roamingSpeed = 30f;
    public float roamingHeight = 30f;

	public float maxZPos = 250;
	public float minZPos = -90;
	public float maxXPos = 410;
	public float minXPos = 80;

	public float minHeight = 1;
	public float maxHeight = 1000;


    private float smoothingSpeed = 4f;
    private float oldSmoothingSpeed = 0f;
    private float oldSmoothingSpeed2 = 0f;

	public float exponentialScrollingFactor = 1000;

	public Vector3 centerPosition = new Vector3(200, 200, 65);

    private float timeClicked = 0f;
    private bool heldMouseLastFrame = false;

	private float risebonus = 10;

    private Vector3 targetPosition;
    private Vector3 targetDirection;
    private Vector3 rotation;
    private float previousHorizontalRotationAngle = 0;
    private float previousVerticalRotationAngle = 25;



    enum CameraState { FOLLOW, ROAM }
    CameraState state = CameraState.FOLLOW;
    // Use this for initialization
    void Start()
    {
        rotation = followThis.transform.forward;
        oldSmoothingSpeed = smoothingSpeed;
        Cursor.visible = true;
        SetRoam();
    }

    // Update is called once per frame
    void Update()
    {
        /*så att jag enkelt kan avsluta när jag testar kameran med ett build*/
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void LateUpdate()
    {
        //Om state är follow så kör follow logiken för att följa objektet. Om state är roam så kör roam logiken för att flyga runt fritt
        switch (state)
        {
            case CameraState.FOLLOW:
                Follow();
                break;
            case CameraState.ROAM:
                Roam();
                break;
        }

        //Följande kod är bara för att testa övergången mellan following och roaming. Den kan tas bort sen så kan något externt kontrollerskript kalla på SetRoam() eller SetFollow()
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (state == CameraState.FOLLOW)
            {
                SetRoam();
            }
            else
            {
                SetFollow();
            }
        }
    }

    public void SetFollow()
    {
        //ändra tillståndet så att kameran nu följer objektet istället.
        state = CameraState.FOLLOW;
    }

    public void SetRoam()
    {
        state = CameraState.ROAM;
        transform.rotation = Quaternion.Euler(90, 0, 0);
        transform.position = new Vector3(transform.position.x, roamingHeight, transform.position.z);
        Cursor.visible = true;
    }

    void Roam()
    {
        Vector3 moveDirection = new Vector3(0, 0, 0);
		Vector3 riseDirection = new Vector3 (0, 0, 0);
        if (Input.GetAxis("Vertical") > 0 || Input.mousePosition.y >= Screen.height - scrollPixelMargin)
        {
            moveDirection += Vector3.up;
        }
        else if (Input.GetAxis("Vertical") < 0 || Input.mousePosition.y < scrollPixelMargin)
        {
            moveDirection += Vector3.down;
        }
        if (Input.GetAxis("Horizontal") < 0 || Input.mousePosition.x < scrollPixelMargin)
        {
            moveDirection += Vector3.left;
        }
        else if (Input.GetAxis("Horizontal") > 0 || Input.mousePosition.x >= Screen.width - scrollPixelMargin)
        {
            moveDirection += Vector3.right;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            riseDirection += Vector3.forward * roamingSpeed;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            riseDirection += Vector3.back * roamingSpeed;
        }

        transform.Translate(moveDirection * roamingSpeed * Time.deltaTime);
		transform.Translate(riseDirection * roamingSpeed *(Mathf.Abs((transform.position.y)/exponentialScrollingFactor)) * Time.deltaTime * risebonus);

		if (transform.position.x < minXPos) {
			transform.position = new Vector3 (minXPos, transform.position.y, transform.position.z);
		}
		if (transform.position.x > maxXPos) {
			transform.position = new Vector3 (maxXPos, transform.position.y, transform.position.z);
		}
		if (transform.position.z < minZPos) {
			transform.position = new Vector3 (transform.position.x, transform.position.y, minZPos);
		}
		if (transform.position.z > maxZPos) {
			transform.position = new Vector3 (transform.position.x, transform.position.y, maxZPos);
		}
		if (transform.position.y < minHeight) {
			transform.position = new Vector3 (transform.position.x, minHeight, transform.position.z);
		} 
		if (transform.position.y > maxHeight) {
			transform.position = new Vector3 (transform.position.x, maxHeight, transform.position.z);
		} 

		if (Input.GetKey (KeyCode.Tab)) {
			transform.position = centerPosition;
		}

    }


    void Follow()
    {

        targetPosition = new Vector3(0, 0, 0);

        Vector3 smoothVelocity;
        if (followThis.GetComponent<Rigidbody>() != null)
        {
            smoothVelocity = followThis.GetComponent<Rigidbody>().velocity;
        }
        else
        {
            smoothVelocity = new Vector3(0, 1, 0);
        }
        
        if (Input.GetButton("Fire1") && !EventSystem.current.IsPointerOverGameObject()) // Left click, and not over GUI.
        {
            float rotationAroundY = Input.GetAxis("Mouse X");
            float rotationAroundX = -Input.GetAxis("Mouse Y");
            previousHorizontalRotationAngle += rotationAroundY * 10;
            previousVerticalRotationAngle += rotationAroundX * 10;
        }
        behindDistance -= Input.GetAxis("Mouse ScrollWheel") * 10; // Sets the zoom.
        

        rotation = Quaternion.AngleAxis(previousHorizontalRotationAngle, Vector3.up) * 
            Quaternion.AngleAxis(previousVerticalRotationAngle, followThis.transform.right) * followThis.transform.forward;

        Vector3 followerPosition = followThis.transform.position - rotation * behindDistance;

        //fixar riktningen för raycasten
        targetDirection = followerPosition - followThis.transform.position;

        //Raycast för att kolla om något är mellan kameran och det den följer
        RaycastHit hit;
        bool hitEnvironment;
        hitEnvironment = false;
        Ray cameraRay = new Ray(followThis.transform.position, targetDirection);

        if (Physics.Raycast(cameraRay, out hit, Vector3.Distance(followThis.transform.position, followerPosition)))
        {
            targetPosition = hit.point + hit.normal.normalized * 0.5f; /* Flytta kameran till en bit innan det som är mellan kameran och bilen enl normalen på ytan*/;
            hitEnvironment = true;
        }

        if (!hitEnvironment)
        {
            targetPosition = followerPosition;  // ingenting var ivägen och andra spökpositionen tar samma värde som första
        }
        
        if (hitEnvironment)
        {
            smoothingSpeed = oldSmoothingSpeed;
            heldMouseLastFrame = false;
        }
        else if (!hitEnvironment && Input.GetButton("Fire1") && !EventSystem.current.IsPointerOverGameObject() || Input.GetAxis("Vertical") < 0) // Resolves sliding.
        {
            if (!heldMouseLastFrame)
            {
                oldSmoothingSpeed = smoothingSpeed;
                smoothingSpeed *= 4f;
            }
            heldMouseLastFrame = true;
            timeClicked = Time.time;
        }
        else if (!hitEnvironment && Time.time - timeClicked > 0.2f)
        {
            smoothingSpeed = oldSmoothingSpeed;
            heldMouseLastFrame = false;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothingSpeed * Time.deltaTime);

        transform.LookAt(followThis.transform);
    }
}