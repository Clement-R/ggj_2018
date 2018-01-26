using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recettes : ScriptableObject {

    [SerializeField]
    public List<Ingredients> ingredients = new List<Ingredients>();

    public enum Type{
        Ordered, Balec
    };

    Type type;

    public bool ConsumeIngredient(Ingredients ingredient)
    {
        if(type == Type.Ordered)
        {
            if(ingredients.Count>0 && ingredients[0].name == ingredient.name)
            {
                ingredients.RemoveAt(0);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            foreach(Ingredients i in ingredients)
            {
                if(i.name == ingredient.name)
                {
                    ingredients.Remove(i);
                    return true;
                }
            }
            return false;
        }
    }

}
