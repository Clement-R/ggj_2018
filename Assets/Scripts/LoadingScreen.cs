using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Launch());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator Launch()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("main_menu");
    }
}
