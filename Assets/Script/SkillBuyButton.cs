using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SkillBuyButton : MonoBehaviour
{
    public TextMeshProUGUI buttonLabelText;
    private bool _disable = false;
    public bool Disable 
    { 
        get => _disable; 
        set
        {
            _disable = value;
        }
    }
    public int skillItemIndex;
    public void OnClickButton()
    {
        var skillItem = SkillPanel.instance.skillItems[skillItemIndex];
        if (Disable) return;
        if (skillItem == null) return;
        skillItem.action();
        Debug.Log($"RUN {skillItem.name}");
        skillItem.buyable = !skillItem.onece;
        GameManager.instance.GrassPoint -= skillItem.needPoint;
        SkillPanel.instance.RefreshSkillPanel();
    }
}
