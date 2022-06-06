using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    private string MIAN_MENU_SCEAN_NAME= "MainMenu";
    public void ResetScene() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void ToMainMenu() {

         SceneManager.LoadScene(MIAN_MENU_SCEAN_NAME);

    }
}
