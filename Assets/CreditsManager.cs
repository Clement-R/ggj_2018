using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour {

    public float alphaScan = 0.3f;

    private void Start()
    {
        Scanline scanline = Camera.main.GetComponent<Scanline>();
        if (scanline)
        {
            scanline.SetAlpha(alphaScan);
        }
    }

    void Update ()
    {
		if(Input.GetButtonDown("A_1") || Input.GetButtonDown("X_1") || Input.GetButtonDown("B_1") || Input.GetButtonDown("Y_1"))
        {
            ScreenEffectsManager.SwitchToScene("main_menu");
        }
	}
}
