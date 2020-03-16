using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashedPlayers : MonoBehaviour
{
    //re-tag sides based on rotation to fix movement bug
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Tile")
        {
            if (gameObject.name == "Side1Col")
                other.transform.parent.parent.tag = "Side1";
            else if (gameObject.name == "Side2Col")
                other.transform.parent.parent.tag = "Side2";
            else if (gameObject.name == "Side3Col")
                other.transform.parent.parent.tag = "Side3";
            else if (gameObject.name == "Side4Col")
                other.transform.parent.parent.tag = "Side4";
            else if (gameObject.name == "Side5Col")
                other.transform.parent.parent.tag = "Side5";
            else if (gameObject.name == "Side6Col")
                other.transform.parent.parent.tag = "Side6";
        }

        //Debug.Log("GameObject: " + other.gameObject);

    }
}