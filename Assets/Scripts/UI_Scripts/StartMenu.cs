using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void StartGame()
    {
        SceneManager.LoadScene("DemoLevel");
    }
    void OptionsMenu()
    {
        //Main_Menu.SetActive(false);
       //Options_Menu.SetActive(true);

    }
    void GetOut()
    {
        Application.Quit();
    }

}
