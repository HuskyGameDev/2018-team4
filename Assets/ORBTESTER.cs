using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OuroborosScripting;
using OuroborosScripting.GeneratedLanguages;
using System.IO;

public class ORBTESTER : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("qqq");

		string contents = File.ReadAllText(@"C:\temp\testing2.bgls");
		//OuroborosLanguage b = new OuroborosLanguage();
		//StartCoroutine(b.BuildParseTable());

		StartCoroutine(OuroborosInterpreter.Execute<OuroborosLanguage>(contents));
	}
}
