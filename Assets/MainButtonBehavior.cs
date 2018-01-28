using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainButtonBehavior : EventTrigger
{
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        AkSoundEngine.PostEvent("menu_select", gameObject);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
    }
}
