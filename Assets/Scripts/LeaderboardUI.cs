using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour {

    LeaderBoard leaderboard;

	void Start ()
    {
        leaderboard = GetComponent<LeaderBoard>();
        StartCoroutine(GetScores());
	}

    IEnumerator GetScores()
    {
        WWW request = leaderboard.GetScores();

        while(!request.isDone)
        {
            yield return null;
        }

        ShowScores();
    }

    void ShowScores()
    {
        for (int i = 0; i < 3; i++)
        {
            if(leaderboard.scores.Count > i && leaderboard.scores[i] != null)
            {
                Score score = leaderboard.scores[i];
                
                transform.GetChild(i).GetChild(0).GetComponent<Text>().text = score.name;
                transform.GetChild(i).GetChild(1).GetComponent<Text>().text = score.score;
            }
            else
            {
                transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "";
                transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "";
            }
        }
    }


    void Update ()
    {
		
	}
}
