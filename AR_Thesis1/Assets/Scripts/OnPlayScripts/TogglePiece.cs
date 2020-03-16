using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePiece : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TurnOffOthers(Toggle T1)
    {
        T1.isOn = false;
    }

    public void SelectPiece(GameObject piece)
    {
        //SelectedPiece = piece;
        Debug.Log("SelectedPiece: " + piece);
    }
}
