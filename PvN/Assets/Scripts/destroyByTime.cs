using UnityEngine;
using System.Collections;

public class destroyByTime : MonoBehaviour
{
    public float lifetime;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}