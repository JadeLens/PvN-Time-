using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerUI : MonoBehaviour
{
    public float currHP;
    public float maxHP;
    public Image healthBar;

    public float currPower;
    public float maxPower;
    public Image powerBar;

    public float currDash;
    public float maxDash;
    public Image dashBar;
    public float cd = 0;

    public Text Coinsnum;
    public Text ScoreNum;
    static public int Score = 0;
    static public int Coins = 0;
    // Use this for initialization
    void Start()
    {
        currHP = maxHP;
        currPower = 0.0f;
        maxPower = 100.0f;

        currDash = 100.0f;
        maxDash = 100.0f;
    }

    // Update is called once per frame
    void Update()
    {
        ScoreNum.text = "" + Score;
        Coinsnum.text = "" + Coins;
        //HP
        healthBar.fillAmount = (currHP / maxHP);
        dashBar.fillAmount = (currDash / maxDash);
        powerBar.fillAmount = (currPower / maxPower);

        //Passive Regen
        //Dash
        if (currDash < maxDash)
        {
            currDash += 1.5f;
        }
        if (currDash > maxDash)
        {
            currDash = maxDash;
        }

        //Power
        if (currPower < maxPower)
        {
            currPower += .3f;
        }
        if (currPower > maxPower)
        {
            currPower = maxPower;
        }

        //Health Decrease test
        if (Input.GetKey(KeyCode.H))
        {
            currHP -= 10;
        }
    }

   
}
