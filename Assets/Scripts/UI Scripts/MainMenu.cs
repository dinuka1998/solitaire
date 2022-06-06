using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private string GAMEPLAY_SCEAN_NAME= "PlayGame";

    public void PlayGame() {

        SceneManager.LoadScene(GAMEPLAY_SCEAN_NAME);

    }

    public void Exit() {

        Application.Quit();

    }
}
