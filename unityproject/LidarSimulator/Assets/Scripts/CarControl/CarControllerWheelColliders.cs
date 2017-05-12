using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerWheelColliders : MonoBehaviour
{

    public WheelCollider WFL;
    public WheelCollider WFR;
    public WheelCollider WBL;
    public WheelCollider WBR;
    public Transform FL;
    public Transform FR;
    public Transform BL;
    public Transform BR;
    public float speed;
    public float brakeCoef = 100f;
    public float acCoef = 10;
    public float maxSpeed = 10f;
    public float maxWheelAngle = 45f;
    public float directionAngleMax = 40f;
    public float directionAngle = 20;

    public float mTorque = 1000f;
    public float brake = 10000f;
    private bool isBraked = false;

    void Decelerate()
    {
        //Debug.Log("deaccelerate");
        //deaccelete

        if (!Input.GetKey(KeyCode.UpArrow) && !isBraked || speed > maxSpeed)
        {
            WBR.motorTorque = 0;
            WBL.motorTorque = 0;
            WFL.motorTorque = 0;
            WFR.motorTorque = 0;
            WBL.brakeTorque = brake * brakeCoef * Time.deltaTime;
            WBR.brakeTorque = brake * brakeCoef * Time.deltaTime;
            WFL.brakeTorque = brake * brakeCoef * Time.deltaTime;
            WFR.brakeTorque = brake * brakeCoef * Time.deltaTime;
        }
    }


    void Accelerate()
    {
        WFL.brakeTorque = 0;
        WFR.brakeTorque = 0;
        WBL.brakeTorque = 0;
        WBR.brakeTorque = 0;
        WBR.motorTorque = Input.GetAxis("Vertical") * mTorque * acCoef * Time.deltaTime;
        WBL.motorTorque = Input.GetAxis("Vertical") * mTorque * acCoef * Time.deltaTime;
    
    }



    void rotateWheels()
    {
        FL.Rotate(WFL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        FR.Rotate(WFR.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        BL.Rotate(WBL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        BR.Rotate(WBR.rpm / 60 * 360 * Time.deltaTime, 0, 0);
    }

    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0.0f, -0.9f, 0.2f);


    }
    void Updata()
    {
        speed = GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
    }
    void FixedUpdate()
    {
        //move forward on UpArrow
        if ((Input.GetKey(KeyCode.UpArrow) && !isBraked) || ((Input.GetKey(KeyCode.DownArrow) && !isBraked)))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
        
                if (speed < maxSpeed)
                {
                    Accelerate();
                    rotateWheels();
                }
                else
                {
                    Decelerate();

                }
            }
            else
            {
                Debug.Log("uparow released");
                Decelerate();
            }


            //move backward on downArrow
         
            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (speed < maxSpeed)
                {
                    Accelerate();
                    rotateWheels();

                }
                else
                {
                    Decelerate();

                }
            }
            else
            {
                Decelerate();
            }
        }

        //Vehicles steering

        directionAngle = (((maxWheelAngle - directionAngleMax) / maxSpeed) * speed) + directionAngleMax;
        WFL.steerAngle = Input.GetAxis("Horizontal") * directionAngle;
        WFR.steerAngle = Input.GetAxis("Horizontal") * directionAngle;
        rotateWheels();

        //Brake

        if (Input.GetKey(KeyCode.Space))
        {

            isBraked = true;
            WBR.brakeTorque = Mathf.Infinity;
            WBL.brakeTorque = Mathf.Infinity;
            WFR.brakeTorque = Mathf.Infinity;
            WFL.brakeTorque = Mathf.Infinity;
            WBL.motorTorque = 0;
            WBR.motorTorque = 0;

        }
        else
        {
            isBraked = false;

        }
        //Debug.Log("Motortorque: " + WBR.motorTorque.ToString() + " " + WBL.motorTorque.ToString());
    }
}
