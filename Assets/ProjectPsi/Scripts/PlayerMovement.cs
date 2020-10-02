using UnityEngine;
using Valve.VR;

public class PlayerMovement : MonoBehaviour
{
    public SteamVR_Input_Sources movementHand;
    public SteamVR_Input_Sources teleportHand;
    public SteamVR_Action_Vector2 movementJoystick;
    public SteamVR_Action_Boolean snapLeftAction;
    public SteamVR_Action_Boolean snapRightAction;
    public SteamVR_Action_Boolean teleportAction;
    public bool useHand;//if true we use your hand for movement. if false we use the head. default false
    public float speed;
    public float deadzone;//the Deadzone of the joystick. used to prevent unwanted walking.
    public LineRenderer teleportLine;
    public Transform teleportReticle;

    private Transform head;
    private Transform leftHand;
    private Transform rightHand;
    private Vector2 joystickInput;
    private bool snapLeft, shouldTurnLeft;
    private bool snapRight, shouldTurnRight;
    private bool teleport, shouldTeleport;
    private Vector3 moveDirection;
    private new CapsuleCollider collider;
    private Rigidbody body;

    private void Start()
    {
        head = GlobalVars.Get("head").transform;
        leftHand = GlobalVars.Get("left_hand").transform;
        rightHand = GlobalVars.Get("right_hand").transform;
        collider = GetComponent<CapsuleCollider>();
        body = GetComponent<Rigidbody>();
    }

    RaycastHit tpHit;//nice

    private void FixedUpdate()
    {
        UpdateCollider();
        UpdateInput();

        moveDirection = Quaternion.AngleAxis(GetAngle(joystickInput) + (useHand ? leftHand : head).transform.rotation.eulerAngles.y, Vector3.up) * Vector3.forward;//get the angle of the touch and correct it for the rotation of the controller
        
        if (body.velocity.magnitude < speed && joystickInput.magnitude > deadzone)
        {
            body.AddForce(moveDirection * 30, ForceMode.Acceleration);
        }

        if(snapLeft)
        {
            shouldTurnLeft = true;
        }
        else if(shouldTurnLeft)
        {
            shouldTurnLeft = false;
            body.rotation = body.rotation * Quaternion.Euler(0, -15,0);
        }

        if (snapRight)
        {
            shouldTurnRight = true;
        }
        else if (shouldTurnRight)
        {
            shouldTurnRight = false;
            body.rotation = body.rotation * Quaternion.Euler(0, 15, 0);
        }

        if (teleport)
        {
            shouldTeleport = true;

            if (Physics.Raycast(rightHand.position, rightHand.forward - rightHand.up, out tpHit, 10f))
            {
                teleportReticle.gameObject.SetActive(true);
                teleportReticle.position = tpHit.point;
                teleportLine.enabled = true;
                teleportLine.SetPositions(new Vector3[] {rightHand.position, tpHit.point});
            }
        }
        else if (shouldTeleport)
        {
            shouldTeleport = false;
            body.position = teleportReticle.position + new Vector3(0, 0.1f, 0);
            teleportReticle.gameObject.SetActive(false);
            teleportLine.enabled = false;
        }
        else
        {
            teleportReticle.gameObject.SetActive(false);
            teleportLine.enabled = false;
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
        collider.height = head.localPosition.y;
        collider.center = new Vector3(head.localPosition.x, head.localPosition.y / 2, head.localPosition.z);
    }

    private void UpdateInput()
    {
        //todo catch any exceptions from turning controller offf
        joystickInput = movementJoystick.GetAxis(movementHand);

        snapLeft = snapLeftAction.GetState(teleportHand);
        snapRight = snapRightAction.GetState(teleportHand);
        teleport = teleportAction.GetState(teleportHand);
    }
}