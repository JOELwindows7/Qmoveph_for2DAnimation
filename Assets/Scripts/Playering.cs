using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental;
using UnityEngine.Animations;

public class Playering : MonoBehaviour
{
    //Checking
    public CanvasCore checkCanvasCore;
    public GroundRayTrigger RayTrigger;
    //Parametering
    [SerializeField] float FootGroundLength = 2f;

    //Statusing
    public bool ActualGrounded = false;
    [SerializeField] bool RayGrounded = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    [SerializeField] bool needInitiativeControls = false;
    public float Deadzoning = 0.1f;
    void Update()
    {
        //RaycastHit Hit; //Raycasting from Official Unity Tutorial
        //Ray GroundingRay = new Ray(transform.position, Vector3.down);
        //Debug.DrawRay(transform.position, Vector3.down * FootGroundLength);
        //RayGrounded = Physics.Raycast(GroundingRay, out Hit, FootGroundLength);

        if (!checkCanvasCore)
        {
            needInitiativeControls = true;
            GameObject go = GameObject.FindGameObjectWithTag("HexagonEngineCore");
            if (go)
            {
                checkCanvasCore = go.GetComponent<CanvasCore>();
            }
        }
        else
        {
            needInitiativeControls = false;
        }

        if (needInitiativeControls)
        {
            if (Input.GetAxisRaw("Horizontal") < -Deadzoning || Input.GetAxisRaw("Horizontal") > Deadzoning)
            {
                MoveDirect(Input.GetAxisRaw("Horizontal"));
            }
            else
            {
                StopMove();
            }

            if (Input.GetAxisRaw("Jump") > .5f)
            {
                JumpNow();
            }
            else
            {
                StopJump();
            }
        }

        if (RayTrigger.Colliding)
        {
            RayGrounded = true;
        } else
        {
            RayGrounded = false;
        }
        if (CollideGrounded && RayGrounded)
        {
            ActualGrounded = true;
        } else
        {
            ActualGrounded = false;
        }
        if (ActualGrounded)
        {
            if (JumpToken < 2 && !HasJumpPressed)
            {
                JumpToken = 2;
            }
        }
        Velocitying = GetComponent<Rigidbody2D>().velocity.x;
        GetComponent<Animator>().SetFloat("Velocity", Velocitying);
        GetComponent<Animator>().SetBool("Grounded", ActualGrounded);

        //GetComponent<Rigidbody2D>().AddForce(Vector2.right *20f);
    }

    [SerializeField] float Velocitying;
    [SerializeField] float AnalogStickValue;
    public float boost = 10f;
    public void MoveDirect(float Xcoord)
    {
        GetComponent<Animator>().SetBool("Walking", true);
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * Xcoord * boost);
        AnalogStickValue = Xcoord;
    }
    public void StopMove()
    {
        GetComponent<Animator>().SetBool("Walking", false);
        AnalogStickValue = 0f;
    }
    [SerializeField] bool HasJumpPressed = false;
    [SerializeField] int JumpToken = 2;
    [SerializeField] float JumpStrength = 350f;
    public void JumpNow()
    {
        if (!HasJumpPressed)
        {
            if (JumpToken > 0)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.up *JumpStrength);
                JumpToken--;
            }
            HasJumpPressed = true;
        }
    }
    public void StopJump()
    {
        HasJumpPressed = false;
    }


    //Grounded Colliding
    [SerializeField] bool CollideGrounded = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollideGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        CollideGrounded = false;
    }
}
