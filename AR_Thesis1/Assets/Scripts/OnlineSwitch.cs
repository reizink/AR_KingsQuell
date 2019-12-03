using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineSwitch : MonoBehaviour
{
    public GameObject ActiveLobby, PassPlay;

    private float SliderGet;

    // Start is called before the first frame update
    void Start()
    {
        SliderGet = GameObject.Find("OnlineSlider").GetComponent<Slider>().value;
    }

    // Update is called once per frame
    public void ChangeLobbyMenu()
    {
        SliderGet = GameObject.Find("OnlineSlider").GetComponent<Slider>().value;

        if(SliderGet > 1)
        {
            ActiveLobby.SetActive(true);
            PassPlay.SetActive(false);
        }
        else
        {
            ActiveLobby.SetActive(false);
            PassPlay.SetActive(true);
        }
    }
}
