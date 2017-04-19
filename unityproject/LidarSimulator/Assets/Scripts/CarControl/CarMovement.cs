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
    private float moveAcc = 0.2f;
    private float moveDeAcc = 1.1f;
    private float maxMoveSpeed = 35f;

    private float baseRotationSpeed;
    private float rotationAcc = 0.6f;
    private float rotationDeAcc = 1.015f;
    private float maxRotationSpeed = 140f;

    /// <summary>
    /// Initializes the base move and rotationspeed
    /// </summary>
    private void Start()
    {
        baseMoveSpeed = moveSpeed;
        baseRotationSpeed = rotationSpeed;
    }

    /// <summary>
    /// Checks if the arrowkeys on the keyboard is pressed and calls all functions based on those actions.
    /// Controlls the movement direction, movement acceleration and rotation direction based on if one is driving forward or backwards.
    /// </summary>
    void Update ()
    {
        if (Input.GetKey(KeyCode.UpArrow) ^ Input.GetKey(KeyCode.DownArrow))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if(direction == -1)
                {
                    moveSpeed = 0;
                }
                direction = 1;
                Rotate(1);
            }
            else
            {
                if(direction == 1)
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
       
        if (moveSpeed != 0)
        {
            Move(direction);
        }
        RotateAcc();
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
                moveSpeed += moveAcc;
            }
        }
        else if(moveSpeed > 0)
        {
            moveSpeed /= moveDeAcc;
            if (moveSpeed < 2)
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
        if (Input.GetKey(KeyCode.LeftArrow) ^ Input.GetKey(KeyCode.RightArrow))
        {
            if (rotationSpeed < maxRotationSpeed)
            {
                rotationSpeed += rotationAcc;
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
                rotationSpeed /= rotationDeAcc;
            }
        }
    }


    /// <summary>
    /// Translates the gameObject which the script is attached to in corresponding direction to the parameter "dir"
    /// </summary>
    /// <param name="dir"></param>
    void Move(int dir)
    {
        transform.Translate(dir * Vector3.forward * moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Rotates the gameObject which the script is attached to based on the parameter dir which is the driving direction 
    /// and if the left and right key is being pressed.
    /// </summary>
    /// <param name="dir"></param>
    void Rotate(int dir)
    {
        if (Input.GetKey(KeyCode.RightArrow) ^ Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.RotateAround(carPivot.transform.position, transform.up, dir * -rotationSpeed * Time.deltaTime);
            }
            else
            {
                transform.RotateAround(carPivot.transform.position, transform.up, dir * rotationSpeed * Time.deltaTime);
            }
        }
    }
}
