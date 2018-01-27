using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;
using UnityEngine.UI;

public class AchievementsManager : MonoBehaviour {

    [Header("Drinks")]
    public Achievement Drinks_1;
    public Achievement Drinks_20;
    public Achievement Drinks_40;
    public Achievement Drinks_60;

    [Header("Shake")]
    public Achievement Shakes_10;
    [Header("Stire")]
    public Achievement Stires_10;
    [Header("Lit")]
    public Achievement Lits_10;

    public GameObject achievementPanel;
    public Achievement[] achievements;
    Text panelText;

    private int _numberOfDrinksServed;
    private int _numberOfDrinksShaken;
    private int _numberOfDrinksStirred;
    private int _numberOfDrinksLitted;

    private bool _panelVisible = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.SetInt("NumberOfDrinks", 0);
            PlayerPrefs.SetInt("NumberOfDrinksStirred", 0);
            PlayerPrefs.SetInt("NumberOfDrinksLitted", 0);
            PlayerPrefs.SetInt("NumberOfDrinksShaken", 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _numberOfDrinksServed = 20;
            CheckAchievements("Drink");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _numberOfDrinksServed = 40;
            CheckAchievements("Drink");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _numberOfDrinksServed = 60;
            CheckAchievements("Drink");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _numberOfDrinksShaken = 10;
            CheckAchievements("Shake");
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _numberOfDrinksStirred = 10;
            CheckAchievements("Stire");
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            _numberOfDrinksLitted = 10;
            CheckAchievements("Lit");
        }
    }

    void Start ()
    {
        // Setup player prefs
        if(PlayerPrefs.HasKey("NumberOfDrinks"))
        {
            _numberOfDrinksServed = PlayerPrefs.GetInt("NumberOfDrinks");
        }
        else
        {
            _numberOfDrinksServed = 0;
            PlayerPrefs.SetInt("NumberOfDrinks", 0);
        }

        if (PlayerPrefs.HasKey("NumberOfDrinksShaken"))
        {
            _numberOfDrinksShaken = PlayerPrefs.GetInt("NumberOfDrinksShaken");
        }
        else
        {
            _numberOfDrinksShaken = 0;
            PlayerPrefs.SetInt("NumberOfDrinksShaken", 0);
        }

        if (PlayerPrefs.HasKey("NumberOfDrinksStirred"))
        {
            _numberOfDrinksStirred = PlayerPrefs.GetInt("NumberOfDrinksStirred");
        }
        else
        {
            _numberOfDrinksStirred = 0;
            PlayerPrefs.SetInt("NumberOfDrinksStirred", 0);
        }

        if (PlayerPrefs.HasKey("NumberOfDrinksLitted"))
        {
            _numberOfDrinksLitted = PlayerPrefs.GetInt("NumberOfDrinksLitted");
        }
        else
        {
            _numberOfDrinksLitted = 0;
            PlayerPrefs.SetInt("NumberOfDrinksLitted", 0);
        }

        print(PlayerPrefs.GetInt("NumberOfDrinks"));

        // panelText = achievementPanel.transform.GetChild(0).GetComponent<Text>();
    }

    void LoadAchievements()
    {
        foreach (var achievement in achievements)
        {
            /*
            if (PlayerPrefs.HasKey(achievement.Key))
            {
                _numberOfDrinksLitted = PlayerPrefs.GetInt(achievement.Key);
            }
            else
            {
                _numberOfDrinksLitted = 0;
                PlayerPrefs.SetInt("NumberOfDrinksLitted", 0);
            }
            */
        }
    }

    void OnEnable()
    {
        EventManager.StartListening("LitDrink", OnLitDrink);
        EventManager.StartListening("StireDrink", OnStireDrink);
        EventManager.StartListening("ShakeDrink", OnShakeDrink);
        EventManager.StartListening("ServeDrink", OnServeDrink);
    }

    void OnLitDrink(dynamic obj)
    {
        _numberOfDrinksLitted++;
        PlayerPrefs.SetInt("NumberOfDrinksLitted", _numberOfDrinksLitted);
        CheckAchievements(obj.type);
    }

    void OnStireDrink(dynamic obj)
    {
        _numberOfDrinksStirred++;
        PlayerPrefs.SetInt("NumberOfDrinksStirred", _numberOfDrinksStirred);
        CheckAchievements(obj.type);
    }

    void OnShakeDrink(dynamic obj)
    {
        _numberOfDrinksShaken++;
        PlayerPrefs.SetInt("NumberOfDrinksShaken", _numberOfDrinksShaken);
        CheckAchievements(obj.type);
    }

    void OnServeDrink(dynamic obj)
    {
        _numberOfDrinksServed++;
        PlayerPrefs.SetInt("NumberOfDrinks", _numberOfDrinksServed);
        print(PlayerPrefs.GetInt("NumberOfDrinks"));
        CheckAchievements(obj.type);
    }

    void OnDisable()
    {
        EventManager.StopListening("LitDrink", OnLitDrink);
        EventManager.StopListening("StireDrink", OnStireDrink);
        EventManager.StopListening("ShakeDrink", OnShakeDrink);
        EventManager.StopListening("ServeDrink", OnServeDrink);
    }

    void CheckAchievements(string type)
    {
        switch (type)
        {
            case "Stire":
                switch (_numberOfDrinksStirred)
                {
                    case 10:
                        UnlockAchievement(Stires_10);
                        break;
                }
                break;

            case "Lit":
                switch (_numberOfDrinksLitted)
                {
                    case 10:
                        UnlockAchievement(Lits_10);
                        break;
                }
                break;

            case "Shake":
                switch (_numberOfDrinksShaken)
                {
                    case 10:
                        UnlockAchievement(Shakes_10);
                        break;
                }
                break;

            case "Drink":
                switch (_numberOfDrinksServed)
                {
                    case 1:
                        UnlockAchievement(Drinks_1);
                        break;

                    case 20:
                        UnlockAchievement(Drinks_20);
                        break;

                    case 40:
                        UnlockAchievement(Drinks_40);
                        break;

                    case 60:
                        UnlockAchievement(Drinks_60);
                        break;
                }
                break;
        }
    }

    void UnlockAchievement(Achievement achievement)
    {
        if (!_panelVisible)
        {
            achievementPanel.GetComponent<AchievementPanelBehavior>().Setup(achievement);
            achievementPanel.GetComponent<Animator>().SetTrigger("Show");
            StartCoroutine("hidePanel");
        }
    }

    IEnumerator hidePanel()
    {
        _panelVisible = true;

        yield return new WaitForSeconds(2.0f);

        achievementPanel.GetComponent<Animator>().SetTrigger("Hide");
        _panelVisible = false;
    }
}
