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
        if(next >= levels.Count || levels[next] == null)
        {
            SceneManager.LoadScene("main_menu");
            next = 0;
            return;
        }
        AsyncOperation op = SceneManager.LoadSceneAsync("main");
        op.completed += (o) =>
        {
            LevelManager.Manager.type = LevelManager.Type.Normal;
            LevelManager.Manager.level = levels[next];
            LevelManager.Manager.Init();
            LevelManager.Manager.finish += LaunchNext;
            next++;
        };
    }
}
