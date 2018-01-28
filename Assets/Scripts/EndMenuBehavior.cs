using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using pkm.EventManager;

public class EndMenuBehavior : MonoBehaviour {

    public float scoreMedium;
    public float scoreHigh;

    [Header("Result sprites")]
    public Sprite resultLow;
    public Sprite resultMedium;
    public Sprite resultHigh;

    private bool _isVisible = false;

    
    public GameObject timeAttackMenu;
    public GameObject timeAttackFirstSelected;
    public LeaderBoard leaderBoard;

	void Start ()
    {
        EventManager.StartListening("ToggleEnd", OnEnd);
        EventManager.StartListening("ToggleEndTimeAttack", OnTimeAttackEnd);
    }

    void OnTimeAttackEnd(dynamic obj)
    {
        timeAttackMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(timeAttackFirstSelected);
    }

    void OnEnd(object obj)
    {
        transform.GetChild(0).gameObject.SetActive(true);

        transform.GetChild(0).GetChild(1).GetComponent<Text>().text = LevelManager.Manager.Score.ToString();
        
        if(LevelManager.Manager.Score < scoreMedium)
        {
            transform.GetChild(0).GetChild(3).GetComponent<Image>().sprite = resultLow;
        }
        else if(LevelManager.Manager.Score >= scoreMedium && LevelManager.Manager.Score < scoreHigh)
        {
            transform.GetChild(0).GetChild(3).GetComponent<Image>().sprite = resultMedium;
        }
        else
        {
            transform.GetChild(0).GetChild(3).GetComponent<Image>().sprite = resultHigh;
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
                ScreenEffectsManager.SwitchToScene("main_menu");
            }

            if (Input.GetButtonDown("Start_1"))
            {
                ScreenEffectsManager.SwitchToScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void LeaderboardRegister(string name)
    {
        Dictionary<string, string> args = new Dictionary<string, string>();
        args.Add(name, LevelManager.Manager.Score.ToString());

        leaderBoard.POST("http://scarounet.pythonanywhere.com/scores", args, () => { });
    }

}
