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

using UnityEngine;

/// <summary>
/// Controlls the movement of the drivable car. Provides both moving and turning acceleration for a more dynamic feel
/// 
/// @author: Jonathan Jansson
/// </summary>
public class CarMovement : MonoBehaviour {

    public float moveSpeed;
    public float rotationSpeed;
    public GameObject carPivot;

    private int direction = 1;
    private float baseMoveSpeed = 20f;
    private float moveAcc = 25f;
    private float moveDeAcc = 25f;
    private float maxMoveSpeed = 30f;
   
    private float baseRotationSpeed;
    private float rotationAcc = 90f;
    private float rotationDeAcc = 70f;
    private float maxRotationSpeed = 140f;

    private bool simulationModeOn = false;

    private Vector3 startPos;
    private Quaternion startRotation;

    private void Awake()
    {
        PlayButton.OnPlayToggled += SetControllerActive;
    }
    void OnDestroy()
    {
        PlayButton.OnPlayToggled -= SetControllerActive;
    }


    void SetControllerActive(bool simulationMode)
    {
        if (!simulationMode)
        {
            transform.position = startPos;
            transform.rotation = startRotation;
        }
        else
        {
            UpdateStartPosition();
        }
        simulationModeOn = simulationMode;
    }

    void UpdateStartPosition()
    {
        startPos = transform.position;
        startRotation = transform.rotation;
    }


    /// <summary>
    /// Initializes the base move and rotationspeed
    /// </summary>
    private void Start()
    {
        baseMoveSpeed = moveSpeed;
        moveSpeed = 0f;
        baseRotationSpeed = rotationSpeed;
        UpdateStartPosition();
    }

    /// <summary>
    /// Checks if the arrowkeys on the keyboard is pressed and calls all functions based on those actions.
    /// Controlls the movement direction, movement acceleration and rotation direction based on if one is driving forward or backwards.
    /// </summary>
    void FixedUpdate()
    {
        if (simulationModeOn)
        {
            if (Input.GetAxis("Vertical") != 0)
            {
                if (Input.GetAxis("Vertical") > 0)
                {
                    if (direction == -1)
                    {
                        moveSpeed = 0;
                    }
                    direction = 1;
                    Rotate(1);
                }
                else
                {
                    if (direction == 1)
                    {
                        moveSpeed = 0;
                    }
                    direction = -1;
                    Rotate(-1);
                }
                MoveAcc(true);
            }
            else
            {
                MoveAcc(false);
            }

            RotateAcc();

            if (moveSpeed != 0)
            {
                Move(direction);
            }
        }
    }

    /// <summary>
    /// Controlls the forwards and backwards driving acceleration and deacceleration
    /// </summary>
    /// <param name="driving"></param>
    void MoveAcc(bool driving)
    {
        if (driving)
        {
            if (moveSpeed < baseMoveSpeed)
            {
                moveSpeed = baseMoveSpeed;
            }
            else if (moveSpeed < maxMoveSpeed)
            {
                moveSpeed += moveAcc*Time.fixedDeltaTime;
            }
        }
        else if(moveSpeed > 0)
        {
            moveSpeed -= moveDeAcc*(moveSpeed/5)*Time.fixedDeltaTime;
            if (moveSpeed <= 1f)
            {
                moveSpeed = 0;
            }
        }
    }

    /// <summary>
    /// Controlls the rotation acceleration and deacceleration based on left or right keys being pressed or not
    /// </summary>
    void RotateAcc()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            if (rotationSpeed < maxRotationSpeed)
            {
                rotationSpeed += rotationAcc * Time.fixedDeltaTime;
            }
        }
        else
        {
            if (rotationSpeed <= baseRotationSpeed)
            {
                rotationSpeed = baseRotationSpeed;
            }
            else
            {
                rotationSpeed -= rotationDeAcc * Time.fixedDeltaTime;
            }
        }
    }

    /// <summary>
    /// Translates the gameObject which the script is attached to in corresponding direction to the parameter "dir"
    /// </summary>
    /// <param name="dir"></param>
    void Move(int dir)
    {
        transform.Translate(dir * Vector3.forward * moveSpeed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Rotates the gameObject which the script is attached to based on the parameter dir which is the driving direction 
    /// and if the left and right key is being pressed. Also enables steering trough click and drag with the mouse.
    /// </summary>
    /// <param name="dir"></param>
    void Rotate(int dir)
    {
        if (Input.GetButton("Fire2")) // Right click
        {
            float rotationAroundY = Input.GetAxis("Mouse X");
            transform.RotateAround(carPivot.transform.position, transform.up, rotationAroundY * 400 * Time.fixedDeltaTime);
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                transform.RotateAround(carPivot.transform.position, transform.up, dir * -rotationSpeed * Time.fixedDeltaTime);
            }
            else
            {
                transform.RotateAround(carPivot.transform.position, transform.up, dir * rotationSpeed * Time.fixedDeltaTime);
            }
        }
    }
}