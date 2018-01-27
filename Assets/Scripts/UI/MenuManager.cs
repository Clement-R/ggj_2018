using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	public void LaunchTimeAttack()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("main");
        op.completed += (o) =>
        {
            LevelManager.Manager.type = LevelManager.Type.Random;
            LevelManager.Manager.Init();
        };
    }

    public void Quit()
    {
        Application.Quit();
    }
}
