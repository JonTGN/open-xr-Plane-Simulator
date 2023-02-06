using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JetController : MonoBehaviour
{
    public JoystickController joystick;
    public LeverController lever;
    public float FlySpeed = 5;
    public float YawAmount = 120 / 2;
    private float yaw;
    private float inputPitch;
    private float inputYaw;

    private bool isMaxedOut;
    private float pitch, roll;

    public TextMeshPro text;
    public AudioSource rocket;

    void Start()
    {
        rocket.Play();
        rocket.volume = 0;
    }

    void Update()
    {
        float speed = Mathf.Min(10, lever.magnitude / 9);
        //Debug.Log("speed: " + speed);

        rocket.volume = (Mathf.Abs(speed) / 10) + .2f;

        // turn up/down and left/right into to vars instead of keeping 4
        inputPitch = (joystick.up != 0) ? -joystick.up : joystick.down;
        inputYaw = (joystick.left != 0) ? -joystick.left : joystick.right;

        inputPitch /= 40;
        inputYaw /= 40;

        // move forward
        transform.position += (transform.forward * speed * FlySpeed) * Time.deltaTime;

        // inputs
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        horizontalInput = inputYaw;
        verticalInput = inputPitch;

        // clamp (bad but idc)
        //horizontalInput = Mathf.Clamp(horizontalInput, -1, 1);
        //verticalInput = Mathf.Clamp(verticalInput, -1, 1);

        text.text = "pitch: " + horizontalInput + "\nyaw: " + verticalInput;

        // determine if pitch should go past max of 15,-15
        if (verticalInput >= 1 || verticalInput <= -1)
            isMaxedOut = true;
        else
            isMaxedOut = false;

        // yaw, pitch, roll
        if (speed > 0.1f || speed < -0.1f)
            yaw += horizontalInput * YawAmount * Time.deltaTime;

        if (!isMaxedOut)
            if (speed > 0.1f || speed < -0.1f)
                pitch = Mathf.Lerp(0, Mathf.Max(15, pitch), Mathf.Abs(verticalInput)) * Mathf.Sign(verticalInput);

        if (speed > 0.1f || speed < -0.1f)
            roll = Mathf.Lerp(0, 5, Mathf.Abs(horizontalInput)) * -Mathf.Sign(horizontalInput);

        // dampen
        //pitch /= 1.2f;

        //if (pitch >= 1 && isMaxedOut)
        //    pitch += 4f * Time.deltaTime;
//
        //else if (pitch <= -1 && isMaxedOut)
        //    pitch -= 4f * Time.deltaTime;

        //Debug.Log("pitch: " + pitch + " maxed out: " + isMaxedOut);

        //if (speed > -0.1f && speed < 0.1f)
        //{
        //    pitch = 0;
        //    roll = 0;
        //    yaw = 0;
        //}
            
        // apply rotation if moving
        if (speed > 0.1f || speed < -0.1f)
            transform.localRotation =  Quaternion.Euler(Vector3.up * yaw + Vector3.right * pitch + Vector3.forward * roll);

    }
}
