using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelSelect : MonoBehaviour 
{
    public Text display;
    public string world;
    public string level;

	// Use this for initialization
	void Start () 
    {
        display = GameObject.Find("current").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () 
    {

	}

    void OnMouseOver()
    {
        display.text = "World: " + world.ToString() + " - " + level.ToString();
    }

    void OnMouseDown()
    {
        Application.LoadLevel(level);
    }
}
