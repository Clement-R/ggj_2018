using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RecipeShower : MonoBehaviour {
    public int playerIndex = 0;
    Recettes recette;
    Image image;
    public Text title;
    public List<Image> progression;
    int progressionIndex = 0;
	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        LevelManager.Manager.ingredientAdded += added;
        LevelManager.Manager.ingredientMissed += missed;

        if (playerIndex == 0)
        {
            LevelManager.Manager.joueur1Ended += (r) =>
            {
                missed(playerIndex);
            };
        }
        else
        {
            LevelManager.Manager.joueur2Ended += (r) =>
            {
                missed(playerIndex);
            };
        }
        missed(playerIndex);
	}

    private void Update()
    {
        if (playerIndex == 0 && LevelManager.Manager.currentJoueur1)
        {
            image.sprite = LevelManager.Manager.currentJoueur1.sprite;
            title.text = LevelManager.Manager.currentJoueur1.recipeName;
        }
        else if(playerIndex == 1 && LevelManager.Manager.currentJoueur2)
        {
            image.sprite = LevelManager.Manager.currentJoueur2.sprite;
            title.text = LevelManager.Manager.currentJoueur2.recipeName;
        }
    }

    void added(int p)
    {
        if(p == playerIndex)
        {
            progression[progressionIndex].enabled = true;
            progressionIndex++;
        }
    }

    void missed(int p)
    {
        if(p == playerIndex)
        {
            foreach(Image i in progression)
            {
                i.enabled = false;
            }
            progressionIndex = 0;
        }
    }
}
