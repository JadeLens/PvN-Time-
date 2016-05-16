using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_Manager : MonoBehaviour
{
    public GameObject Play;
    public GameObject Instructions;
    public GameObject Quit;
    public GameObject Pick;
    public Text instructionText;

    // Use this for initialization
    void Start()
    {
        instructionText.enabled = false;
    }

    //Go to Game
    public void PickSide()
    {
        Play.SetActive(false);
        Instructions.SetActive(false);
        Quit.SetActive(false);
        Pick.SetActive(true);
    }

    public void PlayGame()
    {
        Application.LoadLevel("Game");
    }

    //Go to Options
    public void toInstructions()
    {
        Play.SetActive(false);
        Instructions.SetActive(false);
        Quit.SetActive(false);

        instructionText.enabled = true;
    }


    //Go to Main
    public void toMain()
    {
        Play.SetActive(true);
        Instructions.SetActive(true);
        Quit.SetActive(true);

        instructionText.enabled = false;
    }

    //Quit Game
    public void quitGame()
    {
        Application.Quit();
    }
}
