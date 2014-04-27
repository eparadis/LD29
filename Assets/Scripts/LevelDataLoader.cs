using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelDataLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ReadLevelFromFile( "pond");
	}

	void ReadLevelFromFile( string textResourceName)
	{
		TextAsset textData=(TextAsset)Resources.Load( textResourceName);
		if(textData == null)
		{
			Debug.LogError( "Could not load level from " + textResourceName);
			return;
		}

		List<char[]> output = new List<char[]>();
		foreach( string line in textData.text.Split ('\n') )
		{
			if( line.StartsWith("#") )
				continue;	// ignore lines that start with #
			if( line.Length < 10 )
				continue;	// ignore lines that aren't 10 characters
			output.Add( line.ToCharArray() );
		}
		Debug.Log ( output.Count.ToString() + " rows loaded from " + textResourceName);

		Board board = GetComponent<Board>();
		board.InitalizeBoard( output);
	
	}
	

}
