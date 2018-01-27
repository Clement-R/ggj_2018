using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using pkm.EventManager;

public class EndMenuBehavior : MonoBehaviour {

    public float scoreMedium;
    public float scoreHigh;

    [Header("Result sprites")]
    public Sprite resultLow;
    public Sprite resultMedium;
    public Sprite resultHigh;

    private bool _isVisible = false;

	void Start ()
    {
        EventManager.StartListening("TogglePause", OnEnd);
	}

    void OnEnd(object obj)
    {
        transform.GetChild(0).gameObject.SetActive(true);

        transform.GetChild(0).GetChild(1).GetComponent<Text>().text = LevelManager.Manager.Score.ToString();
        
        if(LevelManager.Manager.Score < scoreMedium)
        {
            transform.GetChild(0).GetChild(3).GetComponent<Image>().sprite = null;
        }
        else if(LevelManager.Manager.Score >= scoreMedium && LevelManager.Manager.Score < scoreHigh)
        {
            transform.GetChild(0).GetChild(3).GetComponent<Image>().sprite = null;
        }
        else
        {
            transform.GetChild(0).GetChild(3).GetComponent<Image>().sprite = null;
        }
        
        _isVisible = true;
    }
	
	void Update ()
    {
        if(_isVisible)
        {
            // Manager players inputs
            if (Input.GetButtonDown("Back_1"))
            {
                SceneManager.LoadScene("main_menu");
            }

            if (Input.GetButtonDown("Start_1"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
