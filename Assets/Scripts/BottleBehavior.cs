using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleBehavior : MonoBehaviour {

    public Ingredients ingredient;

    private SpriteRenderer _sr;

	void Start ()
    {
        _sr = GetComponent<SpriteRenderer>();
        _sr.sprite = ingredient.sprite;
	}

    private string GetIngredientName()
    {
        return ingredient.name;
    }
}
