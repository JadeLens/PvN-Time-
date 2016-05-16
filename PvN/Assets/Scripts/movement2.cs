using UnityEngine;
using System.Collections;

public class movement2 : MonoBehaviour
{
    public float speed = 1.5f;
    public float jump = 3.0f;

    [Header("Physics/Movement")]
    public Rigidbody2D rb;
    public Transform self;
    public Transform EndPoint1;
    public Transform EndPoint2;
    public LayerMask floor;

    [Header("Animations")]
    public Animator anim;
    public float deathanim;
    public bool isGrounded = false;
    public bool isBlocking = false;

    [Header("Dash")]
    public bool isdashready = false;
    public float secondTapTime = 0.2f;
    public float waitTime = 0.3f;
    public float timetotstop = 1.5f;
    public float timetotstart = 0.0f;
    public float currentDashTime;
    public float _lastDashButtonTime;
    public float _lastDashTime;
    public int dash = 30;

    public GameObject dasheffect;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        self = transform;
    }

    void FixedUpdate()
    {
        deathanim = GameObject.Find("Player").GetComponent<playerUI>().currHP;
        if (deathanim <= 0)
        {
            anim.SetTrigger("death");
        }
        else
        {
            //Animations
            rb.velocity = new Vector3(Input.GetAxis("Horizontal") * speed, 0, 0);
            anim.SetFloat("hSpeed", Mathf.Abs(rb.velocity.x));

            //Left Right
            if (Input.GetKey(KeyCode.J) && isBlocking == false)
            {
                rb.AddForce(Vector2.right * -speed * .2f);
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            if (Input.GetKey(KeyCode.L) && isBlocking == false)
            {
                rb.AddForce(transform.right * speed * .2f);
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

            //Block
            if (Input.GetKey(KeyCode.K))
            {
                isBlocking = true;
                anim.SetBool("Block", true);
            }
            else
            {
                isBlocking = false;
                anim.SetBool("Block", false);
            }

            //Attack
            if (Input.GetKeyDown(KeyCode.P) && isGrounded == true)
            {
                Debug.Log("Slash");
                anim.SetTrigger("Attack");
            }
            if (Input.GetKeyDown(KeyCode.P) && isGrounded == false)
            {
                Debug.Log("Jump Slash");
                anim.SetTrigger("jumpSlash");
            }


            //Jump
            RaycastHit2D hit1 = Physics2D.Linecast(transform.position, EndPoint1.position, floor);
            RaycastHit2D hit2 = Physics2D.Linecast(transform.position, EndPoint2.position, floor);
            if (hit1.collider != null || hit2.collider != null)
            {
                isGrounded = true;
                anim.SetBool("Grounded", true);
            }
            if (Input.GetKeyDown(KeyCode.I) && isGrounded == true)
            {
                Debug.Log("JUMP");
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jump), ForceMode2D.Force);

                isGrounded = false;
                anim.SetBool("Grounded", false);
            }

            //Dash
            if (canDash && Input.GetButtonDown("Horizontal"))
            {
                if (Time.time - _lastDashButtonTime < secondTapTime)
                {
                    timetotstart += 0.1f;
                    if (timetotstart <= timetotstop)
                    {
                        if (Input.GetAxisRaw("Horizontal") > 0)
                        {
                            DoDoubleDashright();
                        }
                        else if (Input.GetAxisRaw("Horizontal") < 0)
                        {
                            DoDoubleDashleft();
                        }

                    }
                    else if (timetotstart > timetotstop) { speed = 5; }
                }
                else { speed = 5; }
                _lastDashButtonTime = Time.time;
            }

        }

        //Clamp X
        Vector3 clampVel = rb.velocity;
        clampVel.x = Mathf.Clamp(clampVel.x, -speed, speed);

        rb.velocity = clampVel;
    }

    bool canDash
    {
        get
        {
            return Time.time - _lastDashTime > waitTime;
        }
    }

    void DoDoubleDashright()
    {
        _lastDashTime = Time.time;

        Debug.Log("DASH");
        Instantiate(dasheffect, transform.position, transform.rotation);
        rb.AddForce(Vector2.right * 5000, ForceMode2D.Force);
    }
    void DoDoubleDashleft()
    {
        _lastDashTime = Time.time;

        Debug.Log("DASH");
        Instantiate(dasheffect, transform.position, transform.rotation);
        rb.AddForce(Vector2.right * -5000, ForceMode2D.Force);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(20, 70, 200, 200), "rigidbody velocity: " + rb.velocity);
    }
}