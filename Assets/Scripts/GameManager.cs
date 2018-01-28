using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

public class GameManager : MonoBehaviour {
    
    private bool _dayEnd = false;

	void Start ()
    {
        LevelManager.Manager.finish += DayEnd;
	}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            EventManager.TriggerEvent("ToggleEnd", new { });
        }
    }

    void DayEnd()
    {
        if(!_dayEnd)
        {
            print("Day ended !");

            _dayEnd = true;

            EventManager.TriggerEvent("ToggleEnd", new { } );
        }
    }
}
