using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Score {

    public string name;
    public string score;
    public string date;
}

[System.Serializable]
public class ScoreList
{
    public List<Score> Score;
}