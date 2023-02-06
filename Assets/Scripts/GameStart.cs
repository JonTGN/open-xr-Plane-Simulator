using UnityEngine;
using UnityEngine.InputSystem;

public class GameStart : MonoBehaviour
{
    // controls
    public InputActionReference rhActivate;  // reference to vive's grab control in unity editor
    bool isTryingToGrab;
    bool isColliding;

    // virtual outputs
    public GameObject startSphere;
    public GameObject blinder;
    public GameObject planeRoot;
    public GameObject xrPlayer;
    public GameObject playerParentInPlane;

    private void Start()
    {
        // todo subscribe to methods on left hand
        rhActivate.action.started += DoPressedThing;
        rhActivate.action.performed += DoChangeThing;
        rhActivate.action.canceled += DoReleasedThing;
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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerHand") && isTryingToGrab)
        {
            isColliding = true;

            startSphere.SetActive(false);
            blinder.SetActive(false);

            planeRoot.SetActive(true);
            xrPlayer.transform.SetParent(playerParentInPlane.transform);



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
