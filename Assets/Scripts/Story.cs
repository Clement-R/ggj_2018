using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Story : MonoBehaviour {

    public List<Level> levels;
    int next = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void LaunchNext()
    {
        AsyncOperation op;
        ScreenEffectsManager.manager.Launch();
        if (next >= levels.Count || levels[next] == null)
        {
            
            op = SceneManager.LoadSceneAsync("main_menu");
            op.allowSceneActivation = false;
            next = 0;
            ScreenEffectsManager.manager.middle += () =>
            {
                op.allowSceneActivation = true;
            };
            return;
        }
        op = SceneManager.LoadSceneAsync("main");
        op.allowSceneActivation = false;
        ScreenEffectsManager.manager.middle += () =>
        {
            LevelManager.Manager.type = LevelManager.Type.Normal;
            LevelManager.Manager.level = levels[next];
            LevelManager.Manager.Init();
            LevelManager.Manager.finish += LaunchNext;
            next++;
            op.allowSceneActivation = true;
        };
    }
}
