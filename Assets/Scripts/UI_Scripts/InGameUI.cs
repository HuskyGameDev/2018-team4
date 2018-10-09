using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InGameUI : MonoBehaviour
{
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

    private int[,] playerScale;
    public Player player;

    void Start()
    {
        GUIStyle style = new GUIStyle();
        style.richText = true;
        playerScale = player.StatScaling();
        UItext(5, speedArray);
        UItext(6, strArray);
        UItext(7, sanArray);
        UItext(8, intArray);
    }

    void Update()
    {
        UItext(1, speed);
        UItext(2, Strength);
        UItext(3, Intelegence);
        UItext(4, Sanity);

    }

    public void UItext(int option, Text text)
    {
        switch (option)
        {
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
