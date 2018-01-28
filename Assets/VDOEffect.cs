using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

public class VDOEffect : MonoBehaviour {

    public Sprite idle;
    public Sprite flash;

    private SpriteRenderer _sr;

    void Start ()
    {
        _sr = GetComponent<SpriteRenderer>();
        EventManager.StartListening("Beat", FlashEffect);
	}

    void FlashEffect(dynamic obj)
    {
        StartCoroutine(Effect());
    }

    IEnumerator Effect()
    {
        _sr.sprite = flash;
        yield return new WaitForSeconds(0.25f);
        _sr.sprite = idle;
    }
}
