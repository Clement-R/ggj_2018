using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenEffectsManager : MonoBehaviour {
    public static ScreenEffectsManager manager = null;

    public Material material;
    public event System.Action middle;
    public Scanline scanline;
    float alphaScanline = 1f;
    bool disgression = false;
    float speed = 0.5f;

    // Use this for initialization
    private void Awake()
    {
        if (manager != null && manager != this)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        manager = this;
        material.SetFloat("_TimeBegin", -50000);
        material.SetFloat("_Selector", (float)Random.Range(0, 3));
        material.SetFloat("_Distorsion", (float)Random.Range(0, 2));
    }

    void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
        /*if (Input.GetButtonDown("X_1"))
        {
            //Launch();
            UnityEngine.SceneManagement.SceneManager.LoadScene("main_menu");
            Camera[] cam = GameObject.FindObjectsOfType<Camera>();
            Camera c = GetComponent<Camera>();
            for (int i = cam.Length - 1; i >= 0; i--)
            {
                if (cam[i] != c)
                {
                    DestroyImmediate(cam[i].gameObject);
                }
            }
            scanline.SetAlpha(1);

        }*/
        material.SetFloat("_RealTime", Time.time);
        if (disgression)
        {
            alphaScanline -= speed * Time.deltaTime;
            alphaScanline = Mathf.Clamp(alphaScanline, 0, 1);
            scanline.SetAlpha(alphaScanline);
            if(alphaScanline <= 0) {
                disgression = false;
            }
        }
	}

    private void OnLevelWasLoaded(int level)
    {
        Canvas c = GameObject.FindObjectOfType<Canvas>();
        c.worldCamera = GetComponent<Camera>();
    }

    public void Launch()
    {
        disgression = true;
        StartCoroutine(Effect());
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }

    IEnumerator Effect()
    {
        material.SetFloat("_TimeBegin", Time.time);
        float duration = material.GetFloat("_Duration");

        yield return new WaitForSeconds(duration / 2f);
        if(middle != null)
        {
            middle.Invoke();
            yield return new WaitForSeconds(0.01f);
            Camera[] cam = GameObject.FindObjectsOfType<Camera>();
            Camera c = GetComponent<Camera>();
            for(int i = cam.Length-1; i >= 0; i--)
            {
                if(cam[i] != c)
                {
                    DestroyImmediate(cam[i].gameObject);
                }
            }
            middle = null;
        }
        yield return new WaitForSeconds(duration / 2f);
        material.SetFloat("_Selector", (float)Random.Range(0, 3));
        material.SetFloat("_Distorsion", (float)Random.Range(0, 2));
        //material.SetFloat("_TimeBegin", -duration / 2f);
    }

    public static void SwitchToScene(string name)
    {
        ScreenEffectsManager.manager.Launch();

        AsyncOperation op = SceneManager.LoadSceneAsync(name);
        op.allowSceneActivation = false;
        ScreenEffectsManager.manager.middle += () =>
        {
            op.allowSceneActivation = true;
        };
    }
}
