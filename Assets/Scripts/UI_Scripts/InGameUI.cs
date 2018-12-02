using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/*
 * This is the class for the UI in game
 *
 */ 
public class InGameUI : MonoBehaviour
{
    //These are GUI texts that I modify to show players stats or their scaling
    //They are serialized so I can keep them private while letting them be seen in the editor
    [SerializeField]
    private Text speed;
    [SerializeField]
    private Text Strength;
    [SerializeField]
    private Text Intelegence;
    [SerializeField]
    private Text Sanity;
    [SerializeField]
    private Text speedArray;
    [SerializeField]
    private Text strArray;
    [SerializeField]
    private Text sanArray;
    [SerializeField]
    private Text intArray;
    public GameObject Player1InList;
    public GameObject Player2InList;
    public GameObject Player3InList;
    public GameObject Player4InList; 
    public GameObject Player5InList;
    public GameObject PlayerCharacterImages;
    public Text CollapsePlayerCharacterImagesText;
    public PlayerSetup CharacterList;
    public GameObject PauseUI;
    public GameObject SaveUI;
    public GameObject OptionsUI;
    private string text; 
    //This is the player object and their scaling arrays that I use a lot
    private int[,] playerScale;
    public Player player;
    public int CurrentPlayerTurn;
    public int StartingPlayer;
    Dictionary<string, string> PlayerList;
    public int TurnNumber;
    public Text TurnText; 


    /*
     * This is what gets set as soon as the scene loads. Right now, it's only funtion calls to edit text
     * It will eventually hold a lot more due to the large UI factor of our game
     */
    void Start()
    {
        Player1InList.SetActive(false);
        Player2InList.SetActive(false);
        Player3InList.SetActive(false);
        Player4InList.SetActive(false);
        Player5InList.SetActive(false);
        PlayerList = CharacterList.CharacterList();
        GUIStyle style = new GUIStyle();
        style.richText = true;
        GeneratePlayerListUI();
        StartingPlayer = PlayerTurn();
        CurrentPlayerTurn = StartingPlayer;
        TurnNumber = 1;
        StartTurn();
    }

    public void StartTurn(){
        text = ("Player " + CurrentPlayerTurn);
        Debug.Log(text);
        CharacterList.Setimage(player.GetComponent<Image>(), PlayerList[text]);
        playerScale = player.StatScaling();
        UItext(5, speedArray);
        UItext(6, strArray);
        UItext(7, sanArray);
        UItext(8, intArray);
        UpdateStats();
        TurnText.text = ("Turn " + TurnNumber.ToString());
    }

    public void EndTurn(){
        if(CurrentPlayerTurn == PreviousPlayer(StartingPlayer)){
            TurnNumber += 1;
        }
        CurrentPlayerTurn += 1;
        if (CurrentPlayerTurn > CharacterList.PlayerAmount()){
            CurrentPlayerTurn = 1; 
        }
        StartTurn();
        this.PostNotification("Gameplay->EndOfTurnSwitch");
    }

    public int PreviousPlayer(int Current){
        if(Current == 1){
            return CharacterList.PlayerAmount();
        }
        return Current - 1; 
    }

    /*
     * This updates things every frame. I have the players stats here so they are always up to date. 
     * May be subject to change later in the future. 
     */ 
    public void UpdateStats()
    {
        UItext(1, speed);
        UItext(2, Strength);
        UItext(3, Intelegence);
        UItext(4, Sanity);

    }

    public void GeneratePlayerListUI(){
        int PlayerNumber = CharacterList.PlayerAmount();
        switch(PlayerNumber){
            case 2:
                Player1InList.SetActive(true);
                Player2InList.SetActive(true);
                CharacterList.Setimage(Player1InList.GetComponent<Image>(), PlayerList["Player 1"]);
                CharacterList.Setimage(Player2InList.GetComponent<Image>(), PlayerList["Player 2"]);
                break;
            case 3:
                Player1InList.SetActive(true);
                Player2InList.SetActive(true);
                Player3InList.SetActive(true);
                CharacterList.Setimage(Player1InList.GetComponent<Image>(), PlayerList["Player 1"]);
                CharacterList.Setimage(Player2InList.GetComponent<Image>(), PlayerList["Player 2"]);
                CharacterList.Setimage(Player3InList.GetComponent<Image>(), PlayerList["Player 3"]);
                break; 
            case 4:
                Player1InList.SetActive(true);
                Player2InList.SetActive(true);
                Player3InList.SetActive(true);
                Player4InList.SetActive(true);
                CharacterList.Setimage(Player1InList.GetComponent<Image>(), PlayerList["Player 1"]);
                CharacterList.Setimage(Player2InList.GetComponent<Image>(), PlayerList["Player 2"]);
                CharacterList.Setimage(Player3InList.GetComponent<Image>(), PlayerList["Player 3"]);
                CharacterList.Setimage(Player4InList.GetComponent<Image>(), PlayerList["Player 4"]);
                break;
            case 5:
                Player1InList.SetActive(true);
                Player2InList.SetActive(true);
                Player3InList.SetActive(true); 
                Player4InList.SetActive(true);
                Player5InList.SetActive(true);
                CharacterList.Setimage(Player1InList.GetComponent<Image>(), PlayerList["Player 1"]);
                CharacterList.Setimage(Player2InList.GetComponent<Image>(), PlayerList["Player 2"]);
                CharacterList.Setimage(Player3InList.GetComponent<Image>(), PlayerList["Player 3"]);
                CharacterList.Setimage(Player4InList.GetComponent<Image>(), PlayerList["Player 4"]);
                CharacterList.Setimage(Player5InList.GetComponent<Image>(), PlayerList["Player 5"]);
                break;

        }
    }



    /*
     * This is where all the UI text gets set to their values
     * It's one big switch statement, that I'm sure will only get bigger as time continues. 
     * Right now it changes all the stats text and the stat array texts 
     */ 
    public void UItext(int option, Text text)
    {
        switch (option)
        {
            //1-4 is just the stat value. Gets updated each frame
            case 1:
                text.text = "Spd = " + player.GetStatDiceTotal(Player.StatType.Spd);
                break;

            case 2:
                text.text = "Str = " + player.GetStatDiceTotal(Player.StatType.Str);
                break;
            case 3:
                text.text = "Int = " + player.GetStatDiceTotal(Player.StatType.Int);
                break;
            case 4:
                text.text = "San = " + player.GetStatDiceTotal(Player.StatType.San);
                break;
            //5-8 is the stat array text. This gets called when the scene starts
            //Each has an if, and that if checks to see if the number its about to print, has the same index value  
            //the current stat it is at. If so, highlight the color so the player can see where they are at. 
            case 5:
                text.text = "Speed\n ";
                for (int i = 0; i <= 7; i++)
                {
                    if (i == player.GetStatTotal(Player.StatType.Spd))
                    {
                        string currentValue = playerScale[0, i].ToString();
                        text.text += "<color=Green>" + currentValue + "</color>";
                    }
                    else
                    {
                        text.text += playerScale[0, i];
                    }
                    text.text += " ";
                }

                break;
            case 6:
                text.text = "Strength\n ";
                for (int i = 0; i <= 7; i++)
                {
                    if (i == player.GetStatTotal(Player.StatType.Str))
                    {
                        string currentValue = playerScale[1, i].ToString();
                        text.text += "<color=Green>" + currentValue + "</color>";
                    }
                    else
                    {
                        text.text += playerScale[1, i];
                    }
                    text.text += " ";
                }

                break;
            case 7:
                text.text = "Sanity\n ";
                for (int i = 0; i <= 7; i++)
                {
                    if (i == player.GetStatTotal(Player.StatType.San))
                    {
                        string currentValue = playerScale[3, i].ToString();
                        text.text += "<color=Green>" + currentValue + "</color>";
                    }
                    else
                    {
                        text.text += playerScale[3, i];
                    }
                    text.text += " ";
                }

                break;
            case 8:
                text.text = "Intelligence\n ";
                for (int i = 0; i <= 7; i++)
                {
                    if (i == player.GetStatTotal(Player.StatType.Int))
                    {
                        string currentValue = playerScale[2, i].ToString();
                        text.text += "<color=Green>" + currentValue + "</color>";
                    }
                    else
                    {
                        text.text += playerScale[2, i];
                    }
                    text.text += " ";
                }

                break;
        }

    }

    public void PauseGame()
    {

        this.PostNotification("GameSystem->PauseGame");
        PauseUI.SetActive(true);

    }

    public void Resume()
    {
        this.PostNotification("GameplayButtonPressed");
        PauseUI.SetActive(false);
    }
    public void SaveGame()
    {
        this.PostNotification("SaveGameButtonPressed");
        SaveUI.SetActive(true);
        PauseUI.SetActive(false);
    }
    public void LoadGame()
    {
        SaveUI.SetActive(true);
        this.PostNotification("LoadGameButtonPressed");
        PauseUI.SetActive(false);
    }
    public void Options()
    {
        OptionsUI.SetActive(true);
        this.PostNotification("OptionsButtonPressed");
        PauseUI.SetActive(false);
    }
    public void Leave()
    {
        this.PostNotification("MainMenuButtonPressed");
        PauseUI.SetActive(false);
        SceneManager.LoadScene("Dev_Richy");
    }
    public void LeaveSaveMenu()
    {
        this.PostNotification("SaveFileBackButton");
        PauseUI.SetActive(true);
        SaveUI.SetActive(false);

    }
    public void LeaveOptionsMenu()
    {
        this.PostNotification("OptionsMenu -> MainMenu");
        PauseUI.SetActive(true);
        OptionsUI.SetActive(false);
    }

    public int PlayerTurn(){
        Random rnd = new Random();
        int StartingTurn = Random.Range(1, CharacterList.PlayerAmount());
        return StartingTurn;
    }

    public void HideOrUnHidePlayersUI(){
        if(PlayerCharacterImages.activeSelf){
            CollapsePlayerCharacterImagesText.text = "<";
            PlayerCharacterImages.SetActive(false);
            //Hide UI
        }else{
            CollapsePlayerCharacterImagesText.text = ">";
            PlayerCharacterImages.SetActive(true);
        }
    }
    /// <summary>
    /// returns the hfexcoordinate the mouse is at
    /// </summary>
    /// <returns></returns>
    public HexCoordinate MousePostion()
    {
        Vector2 pos = Input.mousePosition;
		Ray ray = Camera.main.ScreenPointToRay(pos);

		//Create a plane to represent our game grid
		Plane plane = new Plane(Vector3.forward, 0.0f);

		//Do a raycast to find on the ray where we intersect with our game board
		float d;
		plane.Raycast(ray, out d);

		//Check if we collided
		if (d > 0) {
			//Get the position along the ray that we collided
			Vector3 hitPoint = ray.GetPoint(d);
			//Return the HexCoordinate Conversion
			HexCoordinate hex = HexCoordinate.GetHexPositionFromWorld(hitPoint);
			Debug.Log(hex);
			return hex;
		}
		else {
			Debug.LogError("Unable to decect mouse click on game board.");
			return new HexCoordinate(0, 0);
		}
    }  

    public void PlayerMove()
    {
        HexCoordinate hex = MousePostion();
        HexCoordinate Phex = (HexCoordinate)player.GetLocation();
        while (player.MoveRemaining() > 0) {
            if (Input.GetMouseButtonDown(0))//Waits for mouse click
            {
                if (!GameManager._instance.gameState.gameBoard.CanCreateRoom(hex))//Checks if room is there
                {
                    if (GameManager._instance.gameState.gameBoard.canMove(Phex, hex))//Room is there, check if there is a valid door
                    {
                        player.MovePlayer(hex);
                    }
                }
                else//create tile 
                {
                    if (GameManager._instance.gameState.gameBoard.hasNeighbor(hex))//Checks if tile has valid neighbor 
                    {
                        GameManager._instance.gameState.gameBoard.CreateRoom(hex);
                        player.MoveToZero();
                    }
                }
            }
        }
    }


}
