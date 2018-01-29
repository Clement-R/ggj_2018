using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

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

    Recettes currentJ1Original = null;
    Recettes currentJ2Original = null;
    public Recettes currentJoueur1 = null;
    public Recettes currentJoueur2 = null;

    public delegate void RecetteEnded(Recettes r);
    public event RecetteEnded joueur1Ended;
    public event RecetteEnded joueur2Ended;

    public delegate void AddedIngredient(int player);
    public event AddedIngredient ingredientAdded;
    public event AddedIngredient ingredientMissed;

    public event System.Action finish;

    Recettes[] allRecipes;

    public enum Type
    {
        Normal, Random
    }
    public Type type = Type.Normal;

    int score = 0;
    public int Score
    {
        get { return score; }
    }

    int recipesDone = 0;
    public int RecipesDone
    {
        get { return recipesDone; }
    }

    private void Start()
    {
        EventManager.StartListening("LaunchGame", OnGameStart);
    }

    void OnGameStart(dynamic obj)
    {
        AkSoundEngine.PostEvent("ambiance", gameObject);
        AkSoundEngine.PostEvent("ambiance", gameObject);
    }

    Recettes GetNext(int player)
    {
        if(player == 0)
        {
            Recettes r = null;
            if(type == Type.Normal && level.recetteJoueur1 != null && level.recetteJoueur1.Count > 0)
            {
                r = Instantiate<Recettes>(level.recetteJoueur1[0]);
                currentJ1Original = level.recetteJoueur1[0];
                level.recetteJoueur1.RemoveAt(0);
            }
            else if(type == Type.Random)
            {
                r = allRecipes[Random.Range(0, allRecipes.Length)];
                currentJ1Original = r;
                r = Instantiate<Recettes>(r);
                Debug.Log("New Recipe: " + r.name);
            }
            return r;
        }
        if (player == 1)
        {
            Recettes r = null;
            if (type == Type.Normal && level.recetteJoueur2 != null && level.recetteJoueur2.Count > 0)
            {
                r = Instantiate<Recettes>(level.recetteJoueur2[0]);
                currentJ2Original = level.recetteJoueur2[0];
                level.recetteJoueur2.RemoveAt(0);
            }
            else if (type == Type.Random)
            {
                r = allRecipes[Random.Range(0, allRecipes.Length)];
                currentJ2Original = r;
                r = Instantiate<Recettes>(r);
                Debug.Log("New Recipe: " + r.name);
            }
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
                if (achieved && ingredientAdded != null)
                {
                    ingredientAdded.Invoke(0);
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
                if (achieved && ingredientAdded != null)
                {
                    ingredientAdded.Invoke(1);
                }
            }
        }
        if (!achieved)
        {
            if(player == 0 && currentJoueur1 != null)
            {
                currentJoueur1 = Instantiate<Recettes>(currentJ1Original);
                if(ingredientMissed != null)
                {
                    ingredientMissed.Invoke(0);
                }
            }
            else if(player == 1 && currentJoueur2 != null)
            {
                currentJoueur2 = Instantiate<Recettes>(currentJ2Original);
                if (ingredientMissed != null)
                {
                    ingredientMissed.Invoke(1);
                }
            }
        }
        return achieved;
    }

    private void Awake()
    {
        manager = this;

        Debug.Log("Init LevelManager");
        
        Resources.LoadAll<Recettes>("Assets/");
        allRecipes = Resources.FindObjectsOfTypeAll<Recettes>();
        Init();
    }

    public void Ended()
    {

    }

    public void Init()
    {
        
        if (type == Type.Random || level == null)
        {
            level = ScriptableObject.CreateInstance<Level>();
            level.time = 10f;
        }
        else
        {
            level = Instantiate<Level>(level);
        }
        currentJoueur1 = GetNext(0);
        currentJoueur2 = GetNext(1);
    }

    public bool DayFinished()
    {
        if(type == Type.Normal && currentJoueur1 == null && currentJoueur2 == null && level.recetteJoueur1.Count == 0 && level.recetteJoueur2.Count == 0)
        {
            return true;
        }
        return false;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if((elapsedTime > level.time || DayFinished()) && finish != null)
        {
            finish.Invoke();
        }
    }

    public Ingredients GetNextIngredient(int joueur)
    {
        if(joueur == 0)
        {
            if(currentJoueur1 != null)
            {
                return currentJoueur1.GetNextIngredient();
            }

            return null;
        }
        else
        {
            if (currentJoueur2 != null)
            {
                return currentJoueur2.GetNextIngredient();
            }

            return null;
        }
    }

    public bool Valider(int player)
    {
        if (player == 0 && currentJoueur1.IsGood())
        {
            EventManager.TriggerEvent("ReciepeWin", new { type = player });
            recipesDone++;
            score += currentJoueur1.score;
            if (type == Type.Random)
            {
                level.time += 1.5f;
            }
            currentJoueur1 = GetNext(0);
            if (joueur1Ended != null)
            {
                joueur1Ended.Invoke(currentJoueur1);
            }
            return true;
        }
        if (player == 1 && currentJoueur2.IsGood())
        {
            EventManager.TriggerEvent("ReciepeWin", new { type = player });
            Debug.Log("J2 Validé!");
            recipesDone++;
            score += currentJoueur2.score;
            if (type == Type.Random)
            {
                level.time += 1.5f;
            }
            currentJoueur2 = GetNext(1);
            if (joueur2Ended != null)
            {
                joueur2Ended.Invoke(currentJoueur2);
            }
            return true;
        }
        
        return false;
    }
}
