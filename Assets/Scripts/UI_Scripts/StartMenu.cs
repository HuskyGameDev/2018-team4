using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using StateMachineSystem;

//This script is only for the starting menu. It doesn't need to be combined with anything else.
public class StartMenu : MonoBehaviour
{
    //This is a funtion for the start button. Loads the scene to start the game
    public void StartGame()
    {
        this.PostNotification("MainMenuStartGameButton");
    }
    //This is the funtion for the options button on the starting menu. Will eventually pull up another menu.
    public void OptionsMenu()
    {
        //Main_Menu.SetActive(false);
        //Options_Menu.SetActive(true);

    }

    //This is the function that goes on the abandon button. It exits the game, or pauses the editor when pressed.
    public void GetOut()
    {
        Debug.Log("GTFO");
        Debug.Break();
        Application.Quit();
    }
}