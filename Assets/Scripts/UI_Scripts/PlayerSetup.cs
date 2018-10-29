using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StateMachineSystem;
using System.Linq;

public class PlayerSetup : MonoBehaviour {
    public GameInformation script; 
    public Dropdown dropdown1;
    public Dropdown dropdown2;
    public GameObject PlayerSelectionCanvas;
    public Text text;
    public GameObject PlayerNumberCanvas;
    public GameObject MainGameUI;
    public GameObject SummaryCanvas;
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    public GameObject player5;
    public Image player1Image;
    public Image player2Image;
    public Image player3Image;
    public Image player4Image;
    public Image player5Image;
    private List<string> availableCharacters,removedCharacters;
    private List<string> AllCharacters = new List<string> { "Select Character","White", "Green", "Yellow", "Purple", "Red", "Blue" };
    private Dictionary<string,string> Characters;
    private Sprite Green, Red, Blue, Yellow, Purple, White; 
    // Use this for initialization
    void Start () {
        PlayerSelectionCanvas.SetActive(false);
        PlayerNumberCanvas.SetActive(true);
        MainGameUI.SetActive(false);
        SummaryCanvas.SetActive(false);
        player1.SetActive(false);
        player2.SetActive(false);
        player3.SetActive(false);
        player4.SetActive(false);
        player5.SetActive(false);
        Green = Resources.Load<Sprite>("GreenHeasdshot");
        Debug.Log(Resources.Load<Sprite>("GreenHeasdshot"));
        removedCharacters = new List<string>{};
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AdvanceCharacterScreen(){
        this.PostNotification("CharacterAdvancedment");
    }


    private IEnumerator PlayerInfo(int number){
        availableCharacters = AllCharacters;
        dropdown2.ClearOptions();
        dropdown2.AddOptions(AllCharacters);
        for (int i = 0; i < number; i++)
        {
            text.text = ("Player " + (i + 1));
            yield return NotificationCenter.instance.WaitForMessage("CharacterAdvancedment");
            if (dropdown2.value == 0) i = i - 1;
            else
            {
                script.AddPlayer(("Player " + (i + 1)), dropdown2.options[dropdown2.value].text);
                dropdown2.options.RemoveAt(dropdown2.value);
                dropdown2.value = 0;
            }
        }
        Characters = script.ListPlayers();
        PlayerSelectionCanvas.SetActive(false);
        SummaryCanvas.SetActive(true);
        SetSummary();

    }
        public void SetSummary(){
       

        switch (script.PlayerCount())
        {
            case 2:
                player1.SetActive(true);
                player2.SetActive(true);

                Setimage(player1Image, Characters["Player 1"]);
                Setimage(player2Image, Characters["Player 2"]);
                break;
            case 3:
                player1.SetActive(true);
                player2.SetActive(true);
                player3.SetActive(true);
               
                Setimage(player1Image, Characters["Player 1"]);
                Setimage(player2Image, Characters["Player 2"]);
                Setimage(player3Image, Characters["Player 3"]);
                break;
            case 4:
                player1.SetActive(true);
                player2.SetActive(true);
                player3.SetActive(true);
                player4.SetActive(true);
               
                Setimage(player1Image, Characters["Player 1"]);
                Setimage(player2Image, Characters["Player 2"]);
                Setimage(player3Image, Characters["Player 3"]);
                Setimage(player4Image, Characters["Player 4"]);
                break;
            case 5:
                player1.SetActive(true);
                player2.SetActive(true);
                player3.SetActive(true);
                player4.SetActive(true);
                player5.SetActive(true);
              
                Setimage(player1Image, Characters["Player 1"]);
                Setimage(player2Image, Characters["Player 2"]);
                Setimage(player3Image, Characters["Player 3"]);
                Setimage(player4Image, Characters["Player 4"]);
                Setimage(player5Image, Characters["Player 5"]);
                break;
        }

    }

    public void Setimage(Image pic, string name){
        switch(name){
            case "Green":
                Debug.Log("assinging Green");
                pic.GetComponent<Image>().sprite = Green;
                //pic.GetComponent<Image>().color = Color.green;
                
                break;
            case "White":
                Debug.Log("assinging White");
                pic.GetComponent<Image>().sprite = Resources.Load<Sprite>("WhiteHeasdshot");

                break;
            case "Red":
                Debug.Log("assinging Red");
                pic.GetComponent<Image>().sprite = Resources.Load<Sprite>("RedHeasdshot");

                break;
            case "Blue":
                Debug.Log("assinging Blue");
                pic.GetComponent<Image>().sprite = Resources.Load<Sprite>("BlueHeadshot");

                break;
            case "Purple":
                Debug.Log("assinging Purple");
                pic.GetComponent<Image>().sprite = Resources.Load<Sprite>("PurpleHeasdshot");

                break;
            case "Yellow":
                Debug.Log("assinging Yellow");
                pic.GetComponent<Image>().sprite = Resources.Load<Sprite>("YellowHeasdshot");

                break;

        }
    }


    public void CharacterNumber(){
        string value = dropdown1.options[dropdown1.value].text;
        Debug.Log(value);
        switch(value){
            case "2 Players":
                StartCoroutine(PlayerInfo(2));
                PlayerSelectionCanvas.SetActive(true);
                PlayerNumberCanvas.gameObject.SetActive(false);
                break;
            case "3 Players":
                StartCoroutine(PlayerInfo(3));
                PlayerSelectionCanvas.SetActive(true);
                PlayerNumberCanvas.gameObject.SetActive(false);
                break;
            case "4 Players":
                StartCoroutine(PlayerInfo(4));
                PlayerSelectionCanvas.SetActive(true);
                PlayerNumberCanvas.gameObject.SetActive(false);
                break;
            case "5 Players":
                StartCoroutine(PlayerInfo(5));
                PlayerSelectionCanvas.SetActive(true);
                PlayerNumberCanvas.gameObject.SetActive(false);
                break;
        }
    }
    public void BeginTheGame()
    {
        this.PostNotification("GameSystem->GamePlay");
        SummaryCanvas.SetActive(false);
        MainGameUI.SetActive(true);
    }
}
