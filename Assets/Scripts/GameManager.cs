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
        if(LevelManager.Manager.RecipesDone >= 8)
        {
            AkSoundEngine.SetState("Public_Chaleur", "Chaud");
        }
        else if (LevelManager.Manager.RecipesDone >= 3)
        {
            AkSoundEngine.SetState("Public_Chaleur", "Bof");
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
