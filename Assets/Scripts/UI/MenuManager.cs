using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	public void LaunchTimeAttack()
    {
        ScreenEffectsManager.manager.Launch();
        AsyncOperation op = SceneManager.LoadSceneAsync("main");
        op.allowSceneActivation = false;
        ScreenEffectsManager.manager.middle += () =>
        {
            LevelManager.Manager.type = LevelManager.Type.Random;
            LevelManager.Manager.Init();
            op.allowSceneActivation = true;
        };
    }

    void TimeAttackSceneSwitch()
    {

    }

    public void LaunchCredits()
    {
        ScreenEffectsManager.manager.Launch();
        AsyncOperation op = SceneManager.LoadSceneAsync("credits");
        op.allowSceneActivation = false;
        ScreenEffectsManager.manager.middle += () =>
        {
            op.allowSceneActivation = true;
        };
    }

    public void Quit()
    {
        Application.Quit();
    }
}
