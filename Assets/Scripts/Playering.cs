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
    [SerializeField] [Range(0f, 100f)] float HP = 100f;
    [SerializeField] bool isAlive = true;
    [SerializeField] Vector3 RespawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        checkpoint();
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

        if (isAlive)
        {
            GetComponent<Animator>().SetBool("EikSerkat", false);
            if (HP1 <= 0f)
            {
                isAlive = false;
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
        } else
        {
            GetComponent<Animator>().SetBool("EikSerkat", true);
            if (HP1 > 0f)
            {
                isAlive = true;
            }
        }
    }

    [SerializeField] float Velocitying;
    [SerializeField] float AnalogStickValue;
    public float boost = 10f;
    public void MoveDirect(float Xcoord)
    {
        GetComponent<Animator>().SetBool("Walking", true);
        if (Xcoord < 0f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        } else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        if (isAlive)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * Xcoord * boost);
        }
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
            if (isAlive)
            {
                if (JumpToken > 0)
                {
                    GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpStrength);
                    JumpToken--;
                }
            } else
            {
                
            }
            HasJumpPressed = true;
        }
    }
    public void StopJump()
    {
        HasJumpPressed = false;
    }

    //HP manipulate
    public void heal(float howMuch)
    {
        HP1 += howMuch;
        correctHP();
    }
    public void damage(float howMuch)
    {
        HP1 -= howMuch;
        correctHP();
    }
    void correctHP()
    {
        if (HP1 < 0f)
        {
            HP1 = 0f;
        }
        if (HP1 > 100f)
        {
            HP1 = 100f;
        }
    }

    //Functional
    public void respawn()
    {
        HP1 = 100f;
        transform.position = RespawnLocation;
    }
    public void checkpoint()
    {
        RespawnLocation = transform.position;
    }
    public void checkpoint(Vector3 newLocation)
    {
        RespawnLocation = newLocation;
    }
    public void checkpoint(Vector2 newLocation)
    {
        RespawnLocation = newLocation;
    }


    //Grounded Colliding
    [SerializeField] bool CollideGrounded = false;

    public float HP1 { get => HP; set => HP = value; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollideGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        CollideGrounded = false;
    }
}
