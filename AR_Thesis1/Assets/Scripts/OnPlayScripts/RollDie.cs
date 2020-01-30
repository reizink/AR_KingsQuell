using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollDie : MonoBehaviour
{
    public int EndRoll;
    public Text RollNum;


    public void RollTheDie()
    {
        EndRoll = Random.Range(1, 4);
        RollNum.text = EndRoll.ToString();

        //gameObject.GetComponentInChildren<Image>().color = Color.red;
    }
}
