using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using pkm.EventManager;

public class EndMenuBehavior : MonoBehaviour {

	void Start ()
    {
        EventManager.StartListening("TogglePause", OnEnd);
	}

    void OnEnd(object obj)
    {
        transform.GetChild(0).gameObject.SetActive(true);

        transform.GetChild(0).GetChild(1).GetComponent<Text>().text = LevelManager.Manager.Score.ToString();
    }
	
	void Update ()
    {
		// TODO : Manager players inputs
	}
}
