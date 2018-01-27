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
    }

    void Start () {
        manager = this;
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("A_1"))
        {
            Debug.Log("Reset");
            Launch();
        }
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
        material.SetFloat("_Selector", (float)Random.Range(0, 3));
        material.SetFloat("_Distorsion", (float)Random.Range(0, 2));
        yield return new WaitForSeconds(duration / 2f);
        if(middle != null)
        {
            middle.Invoke();
            Camera[] c = FindObjectsOfType<Camera>();
            foreach(Camera cam in c)
            {
                if(cam != GetComponent<Camera>())
                {
                    Destroy(cam);
                }
            }
            middle = null;
        }
        //material.SetFloat("_TimeBegin", -duration / 2f);
    }
}
