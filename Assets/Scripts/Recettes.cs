﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recettes : ScriptableObject {


    public enum Type{
        Ordered, Balec
    };

    public string recipeName;
    public Type type;
    bool good = true;

    public Sprite sprite;
    public int score = 10;
    [SerializeField]
    public List<Ingredients> ingredients = new List<Ingredients>();

    public bool ConsumeIngredient(Ingredients ingredient)
    {
        if(type == Type.Ordered)
        {
            if(ingredients.Count>0 && ingredient != null && ingredients[0].name == ingredient.name)
            {
                ingredients.RemoveAt(0);
                return true;
            }
            else
            {
                good = false;
                return false;
            }
        }
        else
        {
            foreach(Ingredients i in ingredients)
            {
                if(ingredient != null && i.name == ingredient.name)
                {
                    ingredients.Remove(i);
                    return true;
                }
            }
            good = false;
            return false;
        }
    }

    public bool IsGood()
    {
        return ingredients.Count == 0 && good;
    }

    public bool IsFinished()
    {
        return ingredients.Count == 0 || !good;
    }

    public Ingredients GetNextIngredient()
    {
        if(ingredients.Count>0 && type == Type.Ordered)
        {
            return ingredients[0];
        }
        return null;
    }

}
