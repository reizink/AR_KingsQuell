using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeTurns : MonoBehaviour
{
    private int PlayerTurn = 1;

    DragonMovement DragonMovement;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(DragonMovement.EvenDragon == true && PlayerTurn%2 == 0)
        {
            //move Blue Dragon
            PlayerTurn++;
        }
        else if (DragonMovement.EvenDragon == false && PlayerTurn % 2 == 1)
        {
            //move Red Dragon
            PlayerTurn++;
        }
        else
        {
            Debug.Log("Turn Error has occured");
        }
    }
    
}
