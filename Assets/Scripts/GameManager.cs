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
        if (Input.GetKeyDown(KeyCode.M))
        {
            EventManager.TriggerEvent("ToggleEndTimeAttack", new { });
        }
    }

    void DayEnd()
    {
        if(!_dayEnd)
        {
            print("Day ended !");

            _dayEnd = true;
            if(LevelManager.Manager.type == LevelManager.Type.Normal)
            {
                EventManager.TriggerEvent("ToggleEnd", new { } );
            }
            else
            {
                EventManager.TriggerEvent("ToggleEndTimeAttack", new { });
            }
        }
    }
}
