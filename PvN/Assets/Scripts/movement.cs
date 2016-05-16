using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour
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
    public int dashLength = 2;

    [Header("Dash Effect")]
    public TrailRenderer trail;
    public GameObject dasheffect;

    [Header("Melee")]
    public float nextSlash;
    public float slashCooldown = 0.5f;
    public float slashHitTime = .1f;
    public Collider2D swordCollider;
    public Collider2D swordJumpCollider;

    [Header("Ranged")]
    public int throwSpeed;
    public float nextThrow;
    public float throwCooldown = 2.0f;
    public Transform throwShuriken;
    public GameObject shuriken;

    [Header("Special")]
    public GameObject fadeBlackObj;
    public GameObject slashEffect;

    [Header("Invuln")]
    public bool isInvuln = false;
    public float nextNormalState;
    public float invulnCooldown = 2.0f;

    [Header("Colors")]
    Color HitWhite = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    Color HitRed = new Color(1.0f, 0.0f, 0.0f, 1.0f);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        self = transform;

        trail.sortingLayerName = "Player";
        trail.sortingOrder = 3;
    }

    void FixedUpdate()
    {
        deathanim = GetComponent<playerUI>().currHP;
        if (deathanim <= 0)
        {
            anim.SetTrigger("death");
        }
        else
        {
            //Animations
            anim.SetFloat("hSpeed", Mathf.Abs(rb.velocity.x));
            anim.SetFloat("vSpeed", Mathf.Abs(rb.velocity.y));
            Debug.Log((rb.velocity.x));

            //Left Right
            if (Input.GetKey(KeyCode.A) && isBlocking == false)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                transform.Translate(new Vector3(speed, 0, 0));
                anim.SetFloat("hSpeed", .1f);
            }
            else if (Input.GetKey(KeyCode.D) && isBlocking == false)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                transform.Translate(new Vector3(speed, 0, 0));
                anim.SetFloat("hSpeed", .1f);
            }
            else
            {
                anim.SetFloat("hSpeed", 0.0f);
            }

            //Dash Trail Effect
            if (Mathf.Abs(rb.velocity.x) > 0)
            {
                gameObject.GetComponent<TrailRenderer>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<TrailRenderer>().enabled = false;
            }


            //Block
            if (Input.GetKey(KeyCode.S))
            {
                isBlocking = true;
                anim.SetFloat("hSpeed", 0.0f);
                anim.SetBool("Block", true);
            }
            else
            {
                isBlocking = false;
                anim.SetBool("Block", false);
            }

            //Attack
            if (Input.GetKeyDown(KeyCode.F) && isGrounded == true && isBlocking == false && Time.time > nextSlash)
            {
                StartCoroutine(swordColliderFunc());
                Debug.Log("Slash");
                anim.SetTrigger("Attack");

                nextSlash = Time.time + slashCooldown;
            }
            if (Input.GetKeyDown(KeyCode.F) && isGrounded == false && isBlocking == false && Time.time > nextSlash)
            {
                StartCoroutine(swordJumpColliderFunc());

                Debug.Log("Jump Slash");
                anim.SetTrigger("jumpSlash");

                nextSlash = Time.time + slashCooldown;
            }

            //Ranged
            if (Input.GetKeyDown(KeyCode.G) && isBlocking == false && Time.time > nextThrow)
            {
                anim.SetTrigger("throw");
                nextThrow = Time.time + throwCooldown;

                //Spawn Shuriken
                GameObject shurikenSpawn;
                shurikenSpawn = Instantiate(shuriken, throwShuriken.position, throwShuriken.rotation) as GameObject;
                shurikenSpawn.GetComponent<Rigidbody2D>().AddForce(transform.right * throwSpeed);
            }

            //Special
            if (Input.GetKey(KeyCode.Q) && GetComponent<playerUI>().currPower == GetComponent<playerUI>().maxPower)
            {
                GetComponent<playerUI>().currPower = 0;

                //Fade screen to black
                GameObject fadeBlack;
                fadeBlack = Instantiate(fadeBlackObj, self.transform.position, self.transform.rotation) as GameObject;

                GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
                //Find all targets
                foreach (GameObject target in gameObjects)
                {
                    //Spawn slash effect
                    GameObject slash;
                    slash = Instantiate(slashEffect, target.transform.position, target.transform.rotation) as GameObject;
                }

                //Delete all targets
                destroyObjectTag.DestroyGameObjectsWithTag("Enemy");
            }

            //Jump
            RaycastHit2D hit1 = Physics2D.Linecast(transform.position, EndPoint1.position, floor);
            RaycastHit2D hit2 = Physics2D.Linecast(transform.position, EndPoint2.position, floor);
            if (hit1.collider != null || hit2.collider != null)
            {
                isGrounded = true;
                anim.SetBool("Grounded", true);
            }
            else
            {
                isGrounded = false;
                anim.SetBool("Grounded", false);
            }

            if (Input.GetKeyDown(KeyCode.W) && isGrounded == true)
            {
                Debug.Log("JUMP");
                rb.velocity = new Vector2(0, jump);

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
                    else if (timetotstart > timetotstop)
                    {
                    }
                }
                else
                {
                }
                _lastDashButtonTime = Time.time;
            }
        }
    }

    bool canDash
    {
        get
        {
            return Time.time - _lastDashTime > waitTime;
        }
    }

    void DoDoubleDashleft()
    {
        if (GetComponent<playerUI>().currDash == 100)
        {
            _lastDashTime = Time.time;
            Instantiate(dasheffect, transform.position, transform.rotation);

            GetComponent<playerUI>().currDash = 0;
            transform.Translate(new Vector3(dashLength, 0, 0));
        }
    }

    void DoDoubleDashright()
    {
        if (GetComponent<playerUI>().currDash == 100)
        {
            _lastDashTime = Time.time;
            Instantiate(dasheffect, transform.position, transform.rotation);

            GetComponent<playerUI>().currDash = 0;
            transform.Translate(new Vector3(dashLength, 0, 0));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isInvuln == false)
        {
            if (isBlocking == false)
            {
                if (other.gameObject.tag == "enemyMelee")
                {
                    StartCoroutine(InvlunFrames());
                    GetComponent<playerUI>().currHP -= 30;
                }
                if (other.gameObject.tag == "enemyRanged")
                {
                    StartCoroutine(InvlunFrames());
                    GetComponent<playerUI>().currHP -= 10;
                }
            }
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(20, 100, 200, 200), "rigidbody velocity: " + rb.velocity);
    }

    IEnumerator swordColliderFunc()
    {
        swordCollider.enabled = true;
        yield return new WaitForSeconds(slashHitTime);
        swordCollider.enabled = false;
    }

    IEnumerator swordJumpColliderFunc()
    {
        swordJumpCollider.enabled = true;
        yield return new WaitForSeconds(slashHitTime);
        swordJumpCollider.enabled = false;
    }

    IEnumerator InvlunFrames()
    {
        isInvuln = true;
        GetComponent<SpriteRenderer>().color = HitWhite;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = HitRed;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = HitWhite;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = HitRed;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = HitWhite;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = HitRed;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = HitWhite;
        yield return new WaitForSeconds(0.1f);
        isInvuln = false;
    }
}