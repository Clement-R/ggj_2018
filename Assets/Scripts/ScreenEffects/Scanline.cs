using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanline : MonoBehaviour {

    public Material material;
    public float baseSize;
	// Use this for initialization
	void Start () {
        material = Instantiate<Material>(material);
        //baseSize = material.GetFloat("_ScanLineSize");

    }
	
	// Update is called once per frame
	void Update () {
        material.SetFloat("_Move", Random.Range(0,1.5f));
        //material.SetFloat("_ScanLineSize", baseSize+Mathf.PerlinNoise(Time.time/5f,0)* 0.05f-0.025f);

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }

    public void SetAlpha(float alpha)
    {
        Debug.Log(alpha);
        material.SetFloat("_Alpha", alpha);
    }
}
