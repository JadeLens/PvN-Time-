using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class overworldMove : MonoBehaviour
{
    public bool canMove;

    public Text currentLevel;
    public Text confirm;

    public GameObject confirmBox;

    public Transform self;
    public Transform EndPoint1A, EndPoint1B;
    public Transform EndPoint2A, EndPoint2B;
    public Transform EndPoint3A, EndPoint3B;
    public Transform EndPoint4A, EndPoint4B;

    public LayerMask levels;

    // Use this for initialization
    void Start()
    {
        canMove = true;

        currentLevel.enabled = true;
        confirm.enabled = false;
        confirmBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit1 = Physics2D.Linecast(EndPoint1A.position, EndPoint1B.position, levels);
        RaycastHit2D hit2 = Physics2D.Linecast(EndPoint2A.position, EndPoint2B.position, levels);
        RaycastHit2D hit3 = Physics2D.Linecast(EndPoint3A.position, EndPoint3B.position, levels);
        RaycastHit2D hit4 = Physics2D.Linecast(EndPoint4A.position, EndPoint4B.position, levels);

        if (canMove == true)
        {
            if (hit1.collider != null)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    transform.Translate(Vector3.up * 3);
                }
            }
            if (hit2.collider != null)
            {
                if (Input.GetKeyDown(KeyCode.D))
                {
                    transform.Translate(Vector3.right * 3);
                }
            }
            if (hit3.collider != null)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    transform.Translate(-Vector3.up * 3);
                }
            }
            if (hit4.collider != null)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    transform.Translate(-Vector3.right * 3);
                }
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        currentLevel.text = "World: " + other.gameObject.GetComponent<LevelSelect>().world.ToString() + " - " + other.gameObject.GetComponent<LevelSelect>().level.ToString();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("ENTER");
            currentLevel.enabled = false;
            confirm.enabled = true;
            confirmBox.SetActive(true);
            canMove = false;
            confirm.text = other.gameObject.GetComponent<LevelSelect>().world.ToString() + " - " + other.gameObject.GetComponent<LevelSelect>().level.ToString();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("LEAVE");
            currentLevel.enabled = true;
            confirm.enabled = false;
            confirmBox.SetActive(false);
            canMove = true;
        }
    }
}
