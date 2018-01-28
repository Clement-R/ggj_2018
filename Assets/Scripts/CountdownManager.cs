using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using pkm.EventManager;

public class CountdownManager : MonoBehaviour {

    public Text counter;

	void Start ()
    {
        AkSoundEngine.PostEvent("ambiance", gameObject);
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        EventManager.TriggerEvent("TogglePause", new { });

        for (int i = 0; i < 3; i++)
        {
            print(i);
            counter.text = (3 - i).ToString();
            AkSoundEngine.PostEvent("menu_back", gameObject);
            yield return new WaitForSeconds(1f);
        }

        counter.gameObject.SetActive(false);
        EventManager.TriggerEvent("TogglePause", new { });
        EventManager.TriggerEvent("LaunchGame", new { });

        AkSoundEngine.PostEvent("menu_select", gameObject);
        AkSoundEngine.PostEvent("music_switch", gameObject);
    }
}
