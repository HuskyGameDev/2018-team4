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
    public PlayerSetup CharacterList;
    //This is the player object and their scaling arrays that I use a lot
    private int[,] playerScale;
    public Player player;

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
        GUIStyle style = new GUIStyle();
        style.richText = true;
        playerScale = player.StatScaling();
        UItext(5, speedArray);
        UItext(6, strArray);
        UItext(7, sanArray);
        UItext(8, intArray);
        GeneratePlayerListUI();
    }

    /*
     * This updates things every frame. I have the players stats here so they are always up to date. 
     * May be subject to change later in the future. 
     */ 
    void Update()
    {
        UItext(1, speed);
        UItext(2, Strength);
        UItext(3, Intelegence);
        UItext(4, Sanity);

    }

    public void GeneratePlayerListUI(){
        int PlayerNumber = CharacterList.PlayerAmount();
        Dictionary<string,string> PlayerList = CharacterList.CharacterList();
        switch(PlayerNumber){
            case 2:
                Player1InList.SetActive(true);
                Player2InList.SetActive(true);
                CharacterList.Setimage(Player1InList.GetComponent<Image>(),PlayerList["Player 1"]);
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
                text.text = "Spd = " + player.GetStatDice(Player.StatType.Spd);
                break;

            case 2:
                text.text = "Str = " + player.GetStatDice(Player.StatType.Str);
                break;
            case 3:
                text.text = "Int = " + player.GetStatDice(Player.StatType.Int);
                break;
            case 4:
                text.text = "San = " + player.GetStatDice(Player.StatType.San);
                break;
            //5-8 is the stat array text. This gets called when the scene starts
            //Each has an if, and that if checks to see if the number its about to print, has the same index value  
            //the current stat it is at. If so, highlight the color so the player can see where they are at. 
            case 5:
                text.text = "Speed\n ";
                for (int i = 0; i <= 7; i++)
                {
                    if (i == player.GetStat(Player.StatType.Spd))
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
                    if (i == player.GetStat(Player.StatType.Str))
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
                    if (i == player.GetStat(Player.StatType.San))
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
                    if (i == player.GetStat(Player.StatType.Int))
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

}
