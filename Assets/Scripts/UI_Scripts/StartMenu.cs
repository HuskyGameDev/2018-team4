using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartMenu : MonoBehaviour {
    [SerializeField]
    private Text speed;
    [SerializeField]
    private Text Strength;
    [SerializeField]
    private Text Intelegence;
    [SerializeField]
    private Text Sanity;

	// Use this for initialization
	void Start () {
		
	}
    public Player player;

	// Update is called once per frame
	void Update () {
		UItext(1,speed);
        UItext(2, Strength);
        UItext(3, Intelegence);
        UItext(4, Sanity);
	}

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

    public void UItext (int option,Text text)
    {    
        switch (option)
        {
            case 1:
                text.text = "Spd = " + player.GetStat(Player.StatType.Spd);
                break;
                
            case 2:
                text.text = "Str = " + player.GetStat(Player.StatType.Str);
                break;
            case 3:
                text.text = "Int = " + player.GetStat(Player.StatType.Int);
                break;
            case 4:
                text.text = "San = " + player.GetStat(Player.StatType.San);
                break;
        }
        
    }

}
