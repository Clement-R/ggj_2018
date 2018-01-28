using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using pkm.EventManager;

public class CountdownManager : MonoBehaviour {

    public Text counter;

	void Start ()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        EventManager.TriggerEvent("TogglePause", new { });

        for (int i = 0; i < 3; i++)
        {
            print(i);
            counter.text = (3 - i).ToString();
            yield return new WaitForSeconds(1f);
        }

        counter.gameObject.SetActive(false);
        EventManager.TriggerEvent("TogglePause", new { });
    }
}
