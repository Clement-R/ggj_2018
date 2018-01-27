using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RecipeShower : MonoBehaviour {
    public int playerIndex = 0;

	// Use this for initialization
	void Start () {
		if(playerIndex == 0)
        {
            LevelManager.Manager.joueur1Ended += SetRecette;
        }
        else
        {
            LevelManager.Manager.joueur2Ended += SetRecette;
        }
	}
	
	void SetRecette(Recettes r)
    {
        if (r != null)
        {
            GetComponent<Image>().sprite = r.sprite;
        }
    }
}
