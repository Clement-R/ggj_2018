using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPanelBehavior : MonoBehaviour {

    private SpriteRenderer _icon;
    private Text _title;
    private Text _description;

    void Start()
    {
        _icon = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _title = transform.GetChild(1).GetComponent<Text>();
        _description = transform.GetChild(2).GetComponent<Text>();
    }

    public void Setup(Achievement achievement)
    {
        _icon.sprite = achievement.sprite;
        _title.text = achievement.title;
        _description.text = achievement.description;
    }
}
