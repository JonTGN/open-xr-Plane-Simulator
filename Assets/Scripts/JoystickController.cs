using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickController : MonoBehaviour
{
    // controls
    public InputActionReference rhActivate;  // reference to vive's grab control in unity editor
    bool isTryingToGrab;

    public GameObject plane;

    // virtual outputs
    public Transform topOfJoystick;
    private float pitch;  // up/down
    private float roll;  // tilt left/right (misleading since it will really turn plane left/right but then roll will be added later)
    public float up, down, left, right;
    Quaternion originalRotation;  // used to set joystick to 0,0,0 when not in use
    bool isColliding;

    private void Start()
    {
        // todo subscribe to methods on left hand
        rhActivate.action.started += DoPressedThing;
        rhActivate.action.performed += DoChangeThing;
        rhActivate.action.canceled += DoReleasedThing;

        //originalRotation = Quaternion.Euler(
        //    plane.transform.localRotation.x - 90,
        //    plane.transform.localRotation.y,
        //    plane.transform.localRotation.z
        //);

        //Debug.Log("orig: rot: " + transform.eulerAngles.z);
    }

    private void DoPressedThing(InputAction.CallbackContext context)
    {
        isTryingToGrab = true;
    }

    private void DoChangeThing(InputAction.CallbackContext context)
    {
        // pass
    }

    private void DoReleasedThing(InputAction.CallbackContext context)
    {
        isTryingToGrab = false;
    }

    void Update()
    {
        // ensure joystick's rotation is always -90, 0, 0
        //transform.eulerAngles = new Vector3(90, 0, 0);

        // up/down
        pitch = topOfJoystick.rotation.eulerAngles.x;
        if (pitch < 360 && pitch > 290)
        {
            pitch = Mathf.Abs(pitch - 360);
            //Debug.Log("up: " + pitch);
            up = pitch;
            down = 0;
        }
        else if (pitch > 0 && pitch < 74)
        {
            //Debug.Log("down: " + pitch);
            down = pitch;
            up = 0;
        }
        
        // left/right
        roll = topOfJoystick.rotation.eulerAngles.z;
        if (roll < 360 && roll > 290)
        {
            roll = Mathf.Abs(roll - 360);
            //Debug.Log("Right: " + roll);
            right = roll;
            left = 0;
        }
        else if (roll > 0 && roll < 74)
        {
            //Debug.Log("Left: " + roll);
            left = roll;
            right = 0;
        }

        clampDirection();

        // plane rotates and joystick stays in same dir on z axis. Will mess up controls
        //originalRotation = Quaternion.Euler(
        //    transform.eulerAngles.x,
        //    transform.eulerAngles.y,
        //    plane.transform.eulerAngles.y 
        //);

        originalRotation = Quaternion.Euler(
            plane.transform.eulerAngles.x - 90,
            plane.transform.eulerAngles.y,
            plane.transform.eulerAngles.z
        );

        //Debug.Log("plane dir: " + plane.transform.eulerAngles.y);
        //Debug.Log("orgin rot: " + originalRotation.eulerAngles.z);


        if (!isColliding || !isTryingToGrab)
        {
            // go back to original rotation
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                originalRotation,
                Time.deltaTime * 2f
            );
        }

    }

    private void clampDirection()
    {
        if (up > 40)
            up = 40;
        if (down > 40)
            down = 40;
        if (right > 40)
            right = 40;
        if (left > 40)
            left = 40;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerHand") && isTryingToGrab)
        {
            isColliding = true;
            // clamp pitch/roll at 40, get abs since could be -40 in future
            //if (up < 40 && down < 40 && right < 40 && left < 40)

            //transform.rotation = Quaternion.Euler(0, 0, plane.transform.rotation.y);
            // set hand pos

            transform.LookAt(other.transform.position, transform.up);


            //var targetRot = Quaternion.LookRotation(other.transform.position - transform.position);
            //Debug.Log("target rot: " + targetRot);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5.0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            isColliding = false;
        }
    }
}
