using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePiece : MonoBehaviour
{
    public Toggle T1, T2, T3, T4;
    public GameObject Piece;

    private void Update()
    {
        if(gameObject.activeInHierarchy == false)
        {
            T1.isOn = false;
            T2.isOn = false;
            T3.isOn = false;
            T4.isOn = false;
        }
    }

    public void TurnOffOthers(Toggle t1)
    {
        t1.isOn = false;
        //gameObject.GetComponent<Toggle>().isOn = true;
    }

    public void SelectPiece(GameObject piece)
    {
        //gameObject.GetComponent<Toggle>().isOn = true;
        Piece = piece;

        Debug.Log("SelectedPiece: " + piece);
    }
}
