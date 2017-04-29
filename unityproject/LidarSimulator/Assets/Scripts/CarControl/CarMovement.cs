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
    private float moveAcc = 20f;
    private float moveDeAcc = 30f;
    private float maxMoveSpeed = 35f;
   
    private float baseRotationSpeed;
    private float rotationAcc = 40f;
    private float rotationDeAcc = 60f;
    private float maxRotationSpeed = 140f;

    private bool simulationModeOn = false;

    private void Awake()
    {
        PlayButton.OnPlayToggled += SetControllerActive;
    }


    void SetControllerActive(bool simulationMode)
    {
        simulationModeOn = simulationMode;
    }


    /// <summary>
    /// Initializes the base move and rotationspeed
    /// </summary>
    private void Start()
    {
        baseMoveSpeed = moveSpeed;
        moveSpeed = 0f;
        baseRotationSpeed = rotationSpeed;
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