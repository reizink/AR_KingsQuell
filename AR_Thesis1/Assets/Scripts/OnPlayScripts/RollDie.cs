using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollDie : MonoBehaviour
{
    public int EndRoll;
    public Text RollNum;
    public bool Rolled = false;
    public Text PlayerRoll;
    public GameObject RollButton;

    public void Start()
    {
        Rolled = false;
        PlayerRoll.text = "";
        EndRoll = 0;
        RollNum.text = "";
    }

    public void RollTheDie()
    {
        EndRoll = Random.Range(1, 4);
        RollNum.text = EndRoll.ToString();
        Rolled = true;
        PlayerRoll.text = "";
        RollButton.SetActive(false);

        //gameObject.GetComponentInChildren<Image>().color = Color.red;
    }
}
