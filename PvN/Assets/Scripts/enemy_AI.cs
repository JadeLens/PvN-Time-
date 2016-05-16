using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class enemy_AI : MonoBehaviour
{
    public Transform player;
    public Rigidbody2D rb;
    public Transform self;
    public Animator anim;

    public float speed = 1.5f;
    public float jump = 3.0f;

    [Header("AI")]
    public bool isGrounded = false;
    public Transform EndPoint1;
    public Transform EndPoint2;
    public LayerMask floor;

    public float range = 1.0f;
    public bool inRange = false;

    public float distance;
    public GameObject coin;
    [Header("HealthBar")]
    public Image healthBar;
    public Canvas enemyUI;

    public float curHealth;
    public float maxHealth;

    [Header("Melee")]
    public float nextSlash;
    public float slashCooldown = 0.5f;
    public float slashHitTime = .1f;
    public Collider2D pSwordCollider;

    [Header("Invuln")]
    public bool isInvuln = false;
    public float nextNormalState;
    public float invulnCooldown = 2.0f;

    [Header("Colors")]
    Color HitRed = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    Color HitWhite = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    Color DeathWhite = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    Color DeathBlack = new Color(0.0f, 0.0f, 0.0f, 1.0f);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        self = transform;

        healthBar = transform.FindChild("Canvas").FindChild("enemyHealth BG").FindChild("enemyHealth").GetComponent<Image>();
        enemyUI = transform.FindChild("Canvas").GetComponent<Canvas>();
        enemyUI.enabled = false;
    }

    void FixedUpdate()
    {
        //HealthBar
        healthBar.fillAmount = curHealth / maxHealth;

        //Distance to player check
        distance = Vector3.Distance(self.transform.position, player.transform.position);

        if (curHealth <= 0)
        {
            anim.SetTrigger("death");

            //If dead and on floor
            RaycastHit2D hit1 = Physics2D.Linecast(transform.position, EndPoint1.position, floor);
            RaycastHit2D hit2 = Physics2D.Linecast(transform.position, EndPoint2.position, floor);
            if (hit1.collider != null || hit2.collider != null)
            {
                isGrounded = true;
                anim.SetBool("Grounded", true);
                StartCoroutine(death());
            }
        }
        else
        {
            //Movement
            if (self.transform.position.x > player.transform.position.x + range && distance <= 3)
            {
                self.transform.localRotation = Quaternion.Euler(0, 180, 0);
                self.transform.Translate(new Vector3(speed, 0, 0));

                anim.SetFloat("hSpeed", .1f);
                inRange = false;
            }
            else if (self.transform.position.x < player.transform.position.x - range && distance <= 3)
            {
                self.transform.localRotation = Quaternion.Euler(0, 0, 0);
                self.transform.Translate(new Vector3(speed, 0, 0));

                anim.SetFloat("hSpeed", .1f);
                inRange = false;
            }
            else
            {
                anim.SetFloat("hSpeed", 0.0f);
                inRange = true;
            }

            //Attack
            if (isGrounded == true &&
                Time.time > nextSlash &&
                inRange == true &&
                (self.transform.position.y > player.transform.position.y - .25f && self.transform.position.y < player.transform.position.y + .25f)
                && distance <= 1)
            {
                anim.SetTrigger("Attack");
                nextSlash = Time.time + slashCooldown;
                StartCoroutine(pSwordColliderFunc());
            }

            //Jump Attack
            if (isGrounded == false &&
                Time.time > nextSlash &&
                inRange == true &&
                (self.transform.position.y > player.transform.position.y - .5f && self.transform.position.y < player.transform.position.y + .5f)
                && distance <= 1)
            {
                anim.SetTrigger("jumpSlash");
                nextSlash = Time.time + slashCooldown;
            }

            //Jump
            RaycastHit2D hit1 = Physics2D.Linecast(transform.position, EndPoint1.position, floor);
            RaycastHit2D hit2 = Physics2D.Linecast(transform.position, EndPoint2.position, floor);
            if (hit1.collider != null || hit2.collider != null)
            {
                isGrounded = true;
                anim.SetBool("Grounded", true);
            }
            if (Input.GetKeyDown(KeyCode.W) && isGrounded == true)
            {
                rb.velocity = new Vector2(0, jump);

                isGrounded = false;
                anim.SetBool("Grounded", false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isInvuln == false)
        {
            if (other.gameObject.tag == "playerRanged")
            {
                StartCoroutine(InvlunFrames());
                enemyUI.enabled = true;
                curHealth -= 10;
            }

            if (other.gameObject.tag == "playerMelee")
            {
                StartCoroutine(InvlunFrames());
                enemyUI.enabled = true;
                curHealth -= 30;
            }
        }
    }

    IEnumerator pSwordColliderFunc()
    {
        pSwordCollider.enabled = true;
        yield return new WaitForSeconds(slashHitTime);
        pSwordCollider.enabled = false;
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

    IEnumerator death()
    {
        rb.isKinematic = true;
        gameObject.GetComponent<Collider2D>().enabled = false;

        GetComponent<SpriteRenderer>().color = DeathWhite;
        yield return new WaitForSeconds(0.4f);
        GetComponent<SpriteRenderer>().color = DeathBlack;
        yield return new WaitForSeconds(0.4f);
        GetComponent<SpriteRenderer>().color = DeathWhite;
        yield return new WaitForSeconds(0.4f);
        GetComponent<SpriteRenderer>().color = DeathBlack;
        yield return new WaitForSeconds(0.4f);

        enemyUI.enabled = false;

        GetComponent<SpriteRenderer>().color = DeathWhite;
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = DeathWhite;
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = DeathWhite;
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = DeathBlack;
        yield return new WaitForSeconds(0.2f);

        GetComponent<SpriteRenderer>().color = DeathWhite;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = DeathBlack;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = DeathWhite;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = DeathBlack;
        yield return new WaitForSeconds(0.1f);

        playerUI.Score += 10;
        Instantiate(coin, gameObject.transform.position, transform.rotation);

        Destroy(gameObject);
    }

    void OnDestroy()
    {
        print("Script was destroyed");
    }
}