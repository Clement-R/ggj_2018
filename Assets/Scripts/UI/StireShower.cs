using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StireShower : MonoBehaviour {
    SpriteRenderer rend;
    Animator animator;
    public int playerID;
	// Use this for initialization
	void Start () {
        rend = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Ingredients next = LevelManager.Manager.GetNextIngredient(playerID);
        if (next != null && next.name == "stire")
        {
            rend.enabled = true;
        }
        else
        {
            rend.enabled = false;
        }
    }
}
