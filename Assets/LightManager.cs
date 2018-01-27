using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

public class LightManager : MonoBehaviour {

    // Déclaration dans l'UI
    [Header("Interactive Lights")]
    public GameObject LJ1;
    public GameObject LJ2;

    [Header("Colors")]
    public Color FailColor;
    public Color WinColor;
    public Color ReactColor;

    private Color oJ1;
    private Color oJ2;
    
    // Use this for initialization
    void Start () {
        oJ1 = LJ1.GetComponent<SpriteRenderer>().color;
        oJ2 = LJ2.GetComponent<SpriteRenderer>().color;
        EventManager.StartListening("WrongBottle", FailLights);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void FailLights(dynamic obj) 
    {
        if (obj.type == 0)
        {
            print("J Gauche Fail");
            LJ1.GetComponent<SpriteRenderer>().color = FailColor;
            StartCoroutine(GoToColor());
        }
        else
        {
            print("J Droite Fail");
            LJ2.GetComponent<SpriteRenderer>().color = FailColor;
            StartCoroutine(GoToColor());
        }
    }

    IEnumerator GoToColor()
    {
        yield return new WaitForSeconds(1f);
        LJ1.GetComponent<SpriteRenderer>().color = oJ1;
        LJ2.GetComponent<SpriteRenderer>().color = oJ2;
    }
}
