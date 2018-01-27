using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEffectsManager : MonoBehaviour {
    public static ScreenEffectsManager manager = null;

    public Material material;
    public event System.Action middle;

    // Use this for initialization
    private void Awake()
    {
        material.SetFloat("_TimeBegin", -50000);
        material.SetFloat("_Selector", (float)Random.Range(0, 3));
        material.SetFloat("_Distorsion", (float)Random.Range(0, 2));
    }

    void Start () {
        manager = this;
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
        material.SetFloat("_RealTime", Time.time);
	}

    public void Launch()
    {
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
}
