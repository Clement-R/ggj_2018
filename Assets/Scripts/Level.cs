using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : ScriptableObject {
    public float time = 50f;
    [SerializeField]
    public List<Recettes> recetteJoueur1;
    [SerializeField]
    public List<Recettes> recetteJoueur2;


}
