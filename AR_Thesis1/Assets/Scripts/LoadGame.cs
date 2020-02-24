using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{

    public void ButtonClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void BackToMenuButton()
    {
        SceneManager.LoadScene(0);
    }
}

