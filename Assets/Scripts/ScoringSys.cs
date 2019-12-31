using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringSys : MonoBehaviour {

    public int scoreNum = 0;
    public Text publicscoreNum;

	// Use this for initialization
	void Start () 
    {
        
	}
	
    public void AddPoints()
    {
        scoreNum += 1;
        publicscoreNum.text = "Score: " + scoreNum;
    }

	// Update is called once per frame
	void Update () 
    {
        
	}
}
