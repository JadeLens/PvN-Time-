using UnityEngine;
using System.Collections;

public class shurikenCollision : MonoBehaviour
{
    public Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        transform.parent = other.transform;
        rb.isKinematic = true;
        gameObject.GetComponent<Collider2D>().enabled = false;
    }
}
