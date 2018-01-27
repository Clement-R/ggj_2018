using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    private void Start()
    {
        Scanline scanline = Camera.main.GetComponent<Scanline>();
        if (scanline)
        {
            scanline.SetAlpha(1);
        }
    }

    public void LaunchStory()
    {
        Story.story.LaunchNext();
    }

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
