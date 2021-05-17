using UnityEngine;
using Valve.VR;

public class PlayerMovement : MonoBehaviour
{
    [Header("SteamVR")]
    public SteamVR_Input_Sources movementHand;
    public SteamVR_Input_Sources turnHand;
    public SteamVR_Action_Vector2 movementJoystick;
    public SteamVR_Action_Boolean snapLeftAction;
    public SteamVR_Action_Boolean snapRightAction;
    public SteamVR_Action_Boolean teleportAction;

    [Header("Settings")]
    public bool useHand;//if true we use your hand for movement. if false we use the head. default false
    public float speed;
    public float deadzone;//the Deadzone of the joystick. used to prevent unwanted walking.
    public new CapsuleCollider collider;

    private Transform head;
    private Transform leftHand;
    private Transform rightHand;
    private Vector2 joystickInput;
    private bool snapLeft, shouldTurnLeft;
    private bool snapRight, shouldTurnRight;
    private bool teleport;
    private Vector3 moveDirection;
    
    private Rigidbody body;

    private void Start()
    {
        head = GlobalVars.Get("head").transform;
        leftHand = GlobalVars.Get("left_hand").transform;
        rightHand = GlobalVars.Get("right_hand").transform;
        body = GetComponent<Rigidbody>();
    }

    RaycastHit tpHit;//nice

    private void FixedUpdate()
    {
        UpdateCollider();
        UpdateInput();

        moveDirection = Quaternion.AngleAxis(GetAngle(joystickInput) + (useHand ? leftHand : head).transform.rotation.eulerAngles.y, Vector3.up) * Vector3.forward;//get the angle of the touch and correct it for the rotation of the controller

        if ((!PlayerPrefs.HasKey("smoothmove") || PlayerPrefs.GetInt("smoothmove") > 0) && body.velocity.magnitude<speed && joystickInput.magnitude > deadzone)
        {
            body.position = body.position + moveDirection * speed * Time.deltaTime;
        }

        if(snapLeft && !teleport)
        {
            shouldTurnLeft = true;
        }
        else if(shouldTurnLeft)
        {
            shouldTurnLeft = false;
            //body.rotation = body.rotation * Quaternion.Euler(0, -15,0);
            RotateRigidBodyAroundPointBy(body, collider.transform.position, Vector3.up, -15);
        }

        if (snapRight && !teleport)
        {
            shouldTurnRight = true;
        }
        else if (shouldTurnRight)
        {
            shouldTurnRight = false;
            //body.rotation = body.rotation * Quaternion.Euler(0, 15, 0);
            RotateRigidBodyAroundPointBy(body, collider.transform.position, Vector3.up, 15);
        }
    }

    public static float GetAngle(Vector2 p_vector2)//put this in a library???
    {
        if (p_vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }

    private void UpdateCollider()
    {
        //Debug.Log(head);
        //Debug.Log(collider);
        collider.height = head.localPosition.y;
        collider.transform.localPosition = new Vector3(head.localPosition.x, head.localPosition.y / 2, head.localPosition.z);
        collider.transform.eulerAngles = new Vector3(0,head.eulerAngles.y,0);
    }

    private void UpdateInput()
    {
        //todo catch any exceptions from turning controller offf
        joystickInput = movementJoystick.GetAxis(movementHand);

        snapLeft = snapLeftAction.GetState(turnHand);
        snapRight = snapRightAction.GetState(turnHand);
        teleport = teleportAction.GetState(turnHand);
    }

    //code by Sandy Gifford of the unity answers forums
    public void RotateRigidBodyAroundPointBy(Rigidbody rb, Vector3 origin, Vector3 axis, float angle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, axis);
        rb.MovePosition(q * (rb.transform.position - origin) + origin);
        rb.MoveRotation(rb.transform.rotation * q);
    }
}