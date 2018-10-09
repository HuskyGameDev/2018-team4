using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("dev2_Richy");
    }
    public void OptionsMenu()
    {
        //Main_Menu.SetActive(false);
        //Options_Menu.SetActive(true);

    }
    public void GetOut()
    {
        Debug.Log("GTFO");
        Debug.Break();
        Application.Quit();
    }
}