using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public void Start()
    {
        MainMenu.SetActive(false);
        Options.SetActive(false);
        Graphics.SetActive(false);
        Audio.SetActive(false);
        Gameplay.SetActive(false);
        Controls.SetActive(false);
        Pause.SetActive(false);
        this.AddObserver((object sender, object args) => { Options.SetActive(false); }, "Options Menu Disable");
        this.AddObserver((object sender, object args) => { Options.SetActive(true); }, "Options Menu Enable");
        this.AddObserver((object sender, object args) => { MainMenu.SetActive(true); }, "Main Menu Enable");
        this.AddObserver((object sender, object args) => { MainMenu.SetActive(false); }, "Main Menu Disable");
        this.AddObserver((object sender, object args) => { Graphics.SetActive(false); }, "Graphics Menu Disable");
        this.AddObserver((object sender, object args) => { Graphics.SetActive(true); }, "Graphics Menu Enable");
        this.AddObserver((object sender, object args) => { Audio.SetActive(true); }, "Audio Menu Enable");
        this.AddObserver((object sender, object args) => { Audio.SetActive(false); }, "Audio Menu Disable");
        this.AddObserver((object sender, object args) => { Gameplay.SetActive(false); }, "Gameplay Menu Disable");
        this.AddObserver((object sender, object args) => { Gameplay.SetActive(true); }, "Gameplay Menu Enable");
        this.AddObserver((object sender, object args) => { Controls.SetActive(true); }, "Controls Menu Enable");
        this.AddObserver((object sender, object args) => { Controls.SetActive(false); }, "Controls Menu Disable");
        this.AddObserver((object sender, object args) => { Pause.SetActive(false); }, "Pause Menu Disable");
        this.AddObserver((object sender, object args) => { Pause.SetActive(true); }, "Pause Menu Enable");
        //this.AddObserver((object sender, object args) => { Dummy.SetActive(false); }, "Dummy Menu Disable");
        //this.AddObserver((object sender, object args) => { Dummy.SetActive(true); }, "Dummy Menu Enable");
    }
    // Use this for initialization
    public GameObject MainMenu;
    public GameObject Options;
    public GameObject Graphics;
    public GameObject Audio;
    public GameObject Gameplay;
    public GameObject Controls;
    public GameObject Pause;
    //public GameObject Dummy;

    public void OnOptionsClick()
    {
        this.PostNotification("Options Menu Pressed");
    }
    public void OnGraphcisClick()
    {
        this.PostNotification("Graphics Menu Pressed");
    }
    public void OnGameplayClick()
    {
        this.PostNotification("Gameplay Menu Pressed");
    }
    public void OnAudioClick()
    {
        this.PostNotification("Audio Menu Pressed");
    }
    public void OnControlsClick()
    {
        this.PostNotification("Controls Menu Pressed");
    }
    public void OnResumeClick()
    {
        this.PostNotification("GameplayButtonPressed");
    }
    public void OnSaveClick()
    {
        this.PostNotification("SaveGameButtonPressed");
    }
    public void OnLoadClick()
    {
        this.PostNotification("LoadGameButtonPressed");
    }
    public void OnMainClick()
    {
        this.PostNotification("MainMenuButtonPressed");
    }
    public void OnPauseOptionsClick()
    {
        this.PostNotification("OptionsButtonPressed");
    }
    public void OnStartGameClick()
    {
        this.PostNotification("Start Game Pressed");
    }
    public void OnAbandonClick()
    {
        this.PostNotification("Abandon Pressed");
    }


}
