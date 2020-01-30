using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerifyPlayerNum : MonoBehaviour
{
    public Button PlayButton;
    public Toggle C1, C2, C3, C4; //4 characters

    private float PlayerSliderGet;
    private int CharactersSelected;
    private bool selected;

    void Start()
    {
        PlayerSliderGet = GameObject.Find("PlayersSlider").GetComponent<Slider>().value;

        PlayButton.interactable = false;

        C1.isOn = true;
        C2.isOn = true;
        C3.isOn = false;
        C4.isOn = false;
    }

    void Update()
    {
        PlayerSliderGet = GameObject.Find("PlayersSlider").GetComponent<Slider>().value;

        if (Convert.ToInt32(PlayerSliderGet) == CharactersSelected)
        {
            PlayButton.interactable = true;
        }
        else
        {
            PlayButton.interactable = false;
        }

        if(Convert.ToInt32(PlayerSliderGet) == 4)
        {
            C1.interactable = false;
            C2.interactable = false;
            C3.interactable = false;
            C4.interactable = false;

            C1.isOn = true;
            C2.isOn = true;
            //C3.isOn = true;
            //C4.isOn = true;
            //CharactersSelected = 4;
            Debug.Log("All 4 selected");
        }
        else
        {
            C1.interactable = true;
            C2.interactable = true;
            C3.interactable = true;
            C4.interactable = true;
        }
    }

    public void IfPressed()
    {
        CharactersSelected = 0;
        if (C1.isOn) { CharactersSelected += 1; }
        if (C2.isOn) { CharactersSelected += 1; }
        if (C3.isOn) { CharactersSelected += 1; }
        if (C4.isOn) { CharactersSelected += 1; }
        Debug.Log("Characters selected: " + CharactersSelected);
    }
    public void OnClick()
    {

    }
}
