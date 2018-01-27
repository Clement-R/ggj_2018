using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private bool _dayEnd = false;

	void Start ()
    {
        LevelManager.Manager.finish += DayEnd;
	}

    void DayEnd()
    {
        if(!_dayEnd)
        {
            _dayEnd = true;
            print("Day eneded !");
        }
    }
}
