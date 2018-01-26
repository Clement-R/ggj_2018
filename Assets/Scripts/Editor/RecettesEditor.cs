﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Recettes))]
public class RecettesEditor : Editor {

    [MenuItem("Assets/Create/Recette")]
    public static void CreateRecette()
    {
        Utility.CreateAsset<Recettes>();
    }

    [MenuItem("Assets/Create/Ingrédient")]
    public static void CreateIngredient()
    {
        Utility.CreateAsset<Ingredients>();
    }
}
