using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public float alphaScan = 1f;

    private void Start()
    {
        Scanline scanline = Camera.main.GetComponent<Scanline>();
        if (scanline)
        {
            scanline.SetAlpha(alphaScan);
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
        ScreenEffectsManager.SwitchToScene("credits");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
