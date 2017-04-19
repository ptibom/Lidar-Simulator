using UnityEngine;

/// <summary>
/// Controlls the movement of the drivable car. Provides both moving and 
/// turning acceleration for a more dynamic feel
/// 
/// @author: Jonathan Jansson
/// </summary>
public class CarMovement : MonoBehaviour {

	public float moveSpeed;
    public float rotationSpeed;
    public GameObject carPivot;

    private float rotateAcc = 1f;
    private float moveAcc = 1f;
    private float maxSpeed = 30f;
    private float maxRotationSpeed = 140f;

    /// <summary>
    /// Checks if the arrowkeys on the keyboard is pressed and calls all functions based on those actions.
    /// Controlls the movement direction, movement acceleration and rotation direction based on if one is driving forward or backwards.
    /// </summary>
    void Update () {
        if (Input.GetKey(KeyCode.UpArrow) ^ Input.GetKey(KeyCode.DownArrow))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                Move(1);
                Rotate(1);
            }
            else
            {
                Move(-1);
                Rotate(-1);
            }

            MoveAcc(true);
        }
        else
        {
            MoveAcc(false);
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
            if ((moveAcc + moveSpeed) < maxSpeed)
            {
                moveAcc += 0.2f;
            }
        }
        else
        {
            moveAcc /= 1.5f;
        }
    }

    /// <summary>
    /// Controlls the rotation acceleration and deacceleration based on left or right keys being pressed and
    /// </summary>
    void RotateAcc()
    {
        if (Input.GetKey(KeyCode.LeftArrow) ^ Input.GetKey(KeyCode.RightArrow))
        {
            if (rotateAcc + rotationSpeed < maxRotationSpeed)
            {
                rotateAcc += 0.5f;
            }
        }
        else
        {
            if (rotateAcc < 1)
            {
                rotateAcc = 1f;
            }
            else if (rotateAcc > 1)
            {
                rotateAcc /= 1.5f;
            }
        }
    }


    /// <summary>
    /// Translates the gameObject which the script is attached to in corresponding direction to the parameter "dir"
    /// </summary>
    /// <param name="dir"></param>
    void Move(int dir)
    {
        transform.Translate(dir * Vector3.forward * (moveSpeed + moveAcc) * Time.deltaTime);
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
                transform.RotateAround(carPivot.transform.position, transform.up, dir * -(rotationSpeed + rotateAcc) * Time.deltaTime);
            }
            else
            {
                transform.RotateAround(carPivot.transform.position, transform.up, dir * (rotationSpeed + rotateAcc) * Time.deltaTime);
            }
        }
    }
}
