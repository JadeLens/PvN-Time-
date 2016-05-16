using UnityEngine;
using System.Collections;

public class destroyObjectTag : MonoBehaviour 
{
    public static void DestroyGameObjectsWithTag(string tag)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject target in gameObjects)
        {
            GameObject.Destroy(target);
        }
    }
}
