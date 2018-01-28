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

        AkSoundEngine.PostEvent("menu", gameObject);
    }

    public void LaunchStory()
    {
        AkSoundEngine.PostEvent("menu_back", gameObject);
        AkSoundEngine.PostEvent("menu_validation_game", gameObject);

        Story.story.LaunchNext();
    }

    public void LaunchTimeAttack()
    {
        AkSoundEngine.PostEvent("menu_back", gameObject);
        AkSoundEngine.PostEvent("menu_validation_game", gameObject);

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
        AkSoundEngine.PostEvent("menu_back", gameObject);
        ScreenEffectsManager.SwitchToScene("credits");
    }

    public void Quit()
    {
        AkSoundEngine.PostEvent("menu_back", gameObject);
        Application.Quit();
    }
}
