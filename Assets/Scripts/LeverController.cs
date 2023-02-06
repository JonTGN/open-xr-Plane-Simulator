using UnityEngine;
using UnityEngine.InputSystem;

public class LeverController : MonoBehaviour
{
    // controls
    public InputActionReference rhActivate;  // reference to vive's grab control in unity editor
    bool isTryingToGrab;

    public GameObject plane;

    // virtual outputs
    public Transform topOfJoystick;
    Quaternion originalRotation;  // used to set joystick to 0,0,0 when not in use
    bool isColliding;
    public float magnitude;
    float forward, backward;

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
       

        // plane rotates and joystick stays in same dir on z axis. Will mess up controls
        //originalRotation = Quaternion.Euler(
        //    transform.eulerAngles.x,
        //    transform.eulerAngles.y,
        //    plane.transform.eulerAngles.y 
        //);

        
        magnitude = topOfJoystick.rotation.eulerAngles.x;
        if (magnitude < 360 && magnitude > 200)
        {
            magnitude = Mathf.Abs(magnitude - 360);
            magnitude = -magnitude + 40;
        }
        else if (magnitude > 0 && magnitude < 74)
        {
           magnitude += 40;
        }
        

        if (!isColliding || !isTryingToGrab)
        {
            if (magnitude < 0 || magnitude > 100)
            {
                //Debug.Log("switching");
                // go back to original rotation
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, 
                    originalRotation,
                    Time.deltaTime * 2f
                );
            }
        }
        
        

        //Debug.Log("plane dir: " + plane.transform.eulerAngles.y);
        //Debug.Log("orgin rot: " + originalRotation.eulerAngles.z);


        //if (!isColliding || !isTryingToGrab)
        //{
        //    // go back to original rotation
        //    transform.rotation = Quaternion.Slerp(
        //        transform.rotation, 
        //        originalRotation,
        //        Time.deltaTime * 2f
        //    );
        //}

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerHand") && isTryingToGrab)
        {
            isColliding = true;

            /* stops half-way
            Vector3 currentRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);

            transform.LookAt(other.transform.position, transform.up);

            Vector3 newRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);

            Debug.Log("hands are at: " + other.transform.eulerAngles);
            Debug.Log("new rot is: " + new Vector3(newRotation.x, currentRotation.y, currentRotation.z));

            transform.eulerAngles = new Vector3(newRotation.x, currentRotation.y, currentRotation.z);
            */

            //Quaternion rot = Quaternion.Euler(other.transform.position.x, other.transform.position.y, other.transform.position.z);
            //transform.localRotation = Quaternion.Lerp(transform.localRotation, rot, 2.0f * Time.deltaTime);

            transform.LookAt(other.transform.position, transform.up);

            //Debug.Log("mag: " + magnitude);

            //var r = (transform.rotation.eulerAngles.x <= 180) ? transform.rotation.eulerAngles.x : transform.rotation.eulerAngles.x - 360;
            //Debug.Log("euler angle x: " + r);

            //if (forward != 0)
            //    transform.localRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, 0);
            //else if (backward != 0)
            //    transform.localRotation = Quaternion.Euler(transform.rotation.eulerAngles.x - 360, 0, 0);

            originalRotation = Quaternion.Euler(
            plane.transform.eulerAngles.x - 130,
            plane.transform.eulerAngles.y,
            plane.transform.eulerAngles.z
            );

            //Debug.Log("plane dir: " + plane.transform.eulerAngles.y);
            //Debug.Log("orgin rot: " + originalRotation.eulerAngles.z);

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
