using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using ScriptSegment = List<GrammarElement>;

namespace BoardGameScripting {
  /// <summary>
  /// A parsed Script
  /// </summary>
  public class BGSScript {

    private static string defaultSavePath = Application.persistentDataPath + "/Scripts/";
    private static string fileExtension = ".xml";

    /// <summary>
    /// Types of supported scripts
    /// </summary>
    public enum ScriptType { Room, Item, Artifact, Event, Effect }

    /// <summary>
    /// Timings for choices and effects
    /// </summary>
    public enum Timing { Immediate, Delayed, Static }

    /// <summary>
    /// The enforcement level for a choice
    /// </summary>
    public enum Enforcement { Optional, Required }

    /// <summary>
    /// The type of card this is.
    /// </summary>
    public ScriptType type;

    /// <summary>
    /// The name for this card
    /// </summary>
    public string name;

    /// <summary>
    /// The base image for this card
    /// </summary>
    public Sprite image;

    /// <summary>
    /// Aditional icons to place on the card. Will get assumed based on CSL statements
    /// </summary>
    public List<Sprite> additionalIcons;

    /// <summary>
    /// List of effects that this room applies
    /// </summary>
    public List<EffectSet> effectList;

    /// <summary>
    /// List of choices that can be made in this room
    /// </summary>
    public List<ChoiceSet> choiceList;

    /// <summary>
    /// Creates a unique UID for the filename
    /// </summary>
    /// <returns></returns>
    public bool TestForUniqueFilename(string filename, out string UID) {
      string filePath;
      int num = 0;
      do {
        num++;
        filePath = defaultSavePath + filename + ((num == 1) ? "" : "" + num) + fileExtension;
      } while (System.IO.File.Exists(filePath));
      UID = "";
      return num == 1;
    }

    /// <summary>
    /// Saves a card to the file base.
    /// </summary>
    public static void SaveCard(Script cardToSave, string filename) {
      XmlSerializer serializer = new XmlSerializer(typeof(Script));
      string filePath = defaultSavePath + filename + fileExtension;
      System.IO.TextWriter textWriter = new System.IO.StreamWriter(filePath);
      serializer.Serialize(textWriter, cardToSave);
    }

    /// <summary>
    /// Loads all cards from a using a filepath
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static Script LoadCard(string filePath) {
      XmlSerializer serializer = new XmlSerializer(typeof(Script));
      FileStream fs = new FileStream(filePath, FileMode.Open);
      Script script = (Script)serializer.Deserialize(fs);
      return script;
    }

    /// <summary>
    /// Loads all scripts from the scripts folder.
    /// </summary>
    /// <param name="dirPath"></param>
    /// <returns></returns>
    public static List<Script> LoadAllScripts(string dirPath) {
      List<string> filePaths = new List<string>(Directory.GetFiles(dirPath));
      List<Script> scripts = new List<Script>();

      for (int i = filePaths.Count - 1; i >= 0; i--) {
        string filePath = filePaths[i];
        if (filePath.Contains(fileExtension)) {
          scripts.Add(LoadCard(filePath));
          filePaths.RemoveAt(i);
        }
      }

      return scripts;
    }

    /// <summary>
    /// A player set of choices, with a level of Enforcemment.
    /// </summary>
    public struct ChoiceSet {
      /// <summary>
      /// Enfocement level, such as optional or required
      /// </summary>
      public Enforcement enforcement;

      /// <summary>
      /// The list of options to choose between
      /// </summary>
      public List<Choice> choices;

      public ChoiceSet(Enforcement enforcement, List<Choice> choices) {
        this.enforcement = enforcement;
        this.choices = choices;
      }
    }

    /// <summary>
    /// A set of effects that is optional to the player, with some requirements.
    /// </summary>
    public struct Choice {
      /// <summary>
      /// The name, or summary of this Choice. Ex: 'Run Away'
      /// </summary>
      public string name;

      /// <summary>
      /// The Statement that must result in true for this to be a valid choice for the player
      /// </summary>
      public ScriptSegment requirementTestStatement;

      /// <summary>
      /// The text displayed when this choice is chosen/completed.
      /// </summary>
      public string resultText;

      /// <summary>
      /// The effects of this choice upon selection.
      /// </summary>
      public List<EffectSet> resultEffectList;

      public Choice(string name, ScriptSegment requirementTestStatement, string resultText, List<EffectSet> resultEffectList) {
        this.name = name;
        this.requirementTestStatement = requirementTestStatement;
        this.resultText = resultText;
        this.resultEffectList = resultEffectList;
      }
    }

    /// <summary>
    /// A set of Effects, with a timing on when they happen
    /// </summary>
    public struct EffectSet {
      /// <summary>
      /// The timing on when this effect happens, such as immediate or recurring.
      /// </summary>
      public Timing timing;

      /// <summary>
      /// The list of effects to be applied.
      /// </summary>
      public List<Effect> effects;

      public EffectSet(Timing timing, List<Effect> effects) {
        this.timing = timing;
        this.effects = effects;
      }
    }

    /// <summary>
    /// A set of statements that have an effect on the game.
    /// </summary>
    public struct Effect {
      /// <summary>
      /// The tokens that generate an effect when parsed.
      /// </summary>
      public ScriptSegment tokens;
      public Effect(ScriptSegment tokens) {
        this.tokens = tokens;
      }
    }
  }
}
