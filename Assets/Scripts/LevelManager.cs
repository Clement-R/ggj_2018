﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    static LevelManager manager;

    public static LevelManager Manager{
        get {
            if (manager == null)
            {
                GameObject g = new GameObject();
                manager = g.AddComponent<LevelManager>();
            }
            return manager;
        }
    }

    public Level level;

    
    float elapsedTime = 0f;

    public Recettes currentJoueur1 = null;
    public Recettes currentJoueur2 = null;

    public delegate void RecetteEnded(Recettes r);
    public event RecetteEnded joueur1Ended;
    public event RecetteEnded joueur2Ended;

    public event System.Action finish;

    int score = 0;
    public int Score
    {
        get { return score; }
    }

    Recettes GetNext(int player)
    {
        if(player == 0 && level.recetteJoueur1 != null && level.recetteJoueur1.Count > 0)
        {
            Recettes r = Instantiate<Recettes>(level.recetteJoueur1[0]);
            level.recetteJoueur1.RemoveAt(0);
            return r;
        }
        if (player == 1 && level.recetteJoueur2 != null && level.recetteJoueur2.Count > 0)
        {
            Recettes r = Instantiate<Recettes>(level.recetteJoueur2[0]);
            level.recetteJoueur2.RemoveAt(0);
            return r;
        }
        return null;
    }

    public bool AddIngredient(int player, Ingredients ingredient)
    {
        bool achieved = false;
        if (player == 0)
        {
            if(currentJoueur1 == null)
            {
                currentJoueur1 = GetNext(0);
            }
            if(currentJoueur1 != null)
            {
                achieved = currentJoueur1.ConsumeIngredient(ingredient);
                if (currentJoueur1.IsFinished())
                {
                    if (currentJoueur1.IsGood())
                    {
                        Debug.Log("J1 validé!");
                        score += currentJoueur1.score;
                    }
                    else
                    {
                        Debug.Log("J1 Raté");
                    }
                    currentJoueur1 = GetNext(0);
                    if (joueur1Ended != null)
                    {
                        joueur1Ended.Invoke(currentJoueur1);
                    }
                }
            }
        }
        if (player == 1)
        {
            if (currentJoueur2 == null)
            {
                currentJoueur2 = GetNext(1);
            }
            if (currentJoueur2 != null)
            {
                achieved = currentJoueur2.ConsumeIngredient(ingredient);
                if (currentJoueur2.IsFinished())
                {
                    if (currentJoueur2.IsGood())
                    {
                        Debug.Log("J2 Validé!");
                        score += currentJoueur2.score;
                    }
                    else
                    {
                        Debug.Log("J2 Raté");
                    }
                    currentJoueur2 = GetNext(1);
                    if (joueur2Ended != null)
                    {
                       joueur2Ended.Invoke(currentJoueur2);
                    }
                }
            }
        }
        return achieved;
    }

    private void Awake()
    {
        manager = this;
        level = Instantiate<Level>(level);
        currentJoueur1 = GetNext(0);
        currentJoueur2 = GetNext(1);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > level.time && finish != null)
        {
            finish.Invoke();
        }
    }

    public Ingredients GetNextIngredient(int joueur)
    {
        if(joueur == 0)
        {
            return currentJoueur1.GetNextIngredient();
        }
        else
        {
            return currentJoueur2.GetNextIngredient();
        }
    }
}
