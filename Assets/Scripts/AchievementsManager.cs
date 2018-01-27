using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;
using UnityEngine.UI;

public class AchievementsManager : MonoBehaviour {

    public GameObject achievementPanel;
    Text panelText;

    private int _numberOfDrinksServed;
    private int _numberOfDrinksShaken;
    private int _numberOfDrinksStirred;
    private int _numberOfDrinksLitted;

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

        // panelText = achievementPanel.transform.GetChild(0).GetComponent<Text>();
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
        CheckAchievements();
    }

    void OnStireDrink(dynamic obj)
    {
        _numberOfDrinksStirred++;
        CheckAchievements();
    }

    void OnShakeDrink(dynamic obj)
    {
        _numberOfDrinksShaken++;
        CheckAchievements();
    }

    void OnServeDrink(dynamic obj)
    {
        _numberOfDrinksServed++;
        CheckAchievements();
    }

    void OnDisable()
    {
        EventManager.StopListening("LitDrink", OnLitDrink);
        EventManager.StopListening("StireDrink", OnStireDrink);
        EventManager.StopListening("ShakeDrink", OnShakeDrink);
        EventManager.StopListening("ServeDrink", OnServeDrink);
    }

    void CheckAchievements()
    {
        switch(_numberOfDrinksServed)
        {
            case 1:
                UnlockAchievement("Trainees", "First drink made");
                break;

            case 20:
                UnlockAchievement("Canned Heat Connoisseurs", "20 drinks made");  
                break;

            case 40:
                UnlockAchievement("Confirmed Mixologists", "40 drinks made");
                break;

            case 60:
                UnlockAchievement("Booze Bishops", "60 drinks made");
                break;
        }

        switch (_numberOfDrinksShaken)
        {
            case 10:
                UnlockAchievement("Shaken, not stirred", "Shaken 10 drinks");
                break;
        }

        switch (_numberOfDrinksStirred)
        {
            case 10:
                UnlockAchievement("Twirlers", "Stirred 10 drinks");
                break;
        }

        switch (_numberOfDrinksLitted)
        {
            case 10:
                UnlockAchievement("Arsonists", "Lit 10 drinks on fire");
                break;
        }
    }

    void UnlockAchievement(string title, string description)
    {
        achievementPanel.SetActive(true);
        // obj.text
        // obj.description

        // panelText.text = obj.text;
        StartCoroutine("hidePanel");
    }

    IEnumerator hidePanel()
    {
        yield return new WaitForSeconds(2.0f);
        achievementPanel.SetActive(false);
    }
}
