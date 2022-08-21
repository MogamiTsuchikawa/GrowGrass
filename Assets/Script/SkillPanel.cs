using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class SkillPanel : MonoBehaviour
{
    private bool isOpenSkillPanel = false;
    [SerializeField] Sprite skillButtonOnSprite, skillButtonOffSprite;
    [SerializeField] Image skillButtonImage;
    [SerializeField] GameObject skillPanel;
    [SerializeField] List<SkillBuyButton> skillBuyButtons = new();
    public List<SkillItem> skillItems = new()
    {
        new SkillItem()
        {
            index = 0,
            name = "Ž©‘RŠg‘å",
            needPoint = 500,
            action = () => { GrassManager.instance.hasNaturalSpreadSkill = true; },
            onece = true
        },
        new SkillItem()
        {
            index = 1,
            name = "Ž©‘R¬’·",
            needPoint = 500,
            action = () => { GrassManager.instance.hasAutoLevelUpSkill = true; },
            onece = true
        },
        new SkillItem()
        {
            index = 2,
            name = "Ží‚ÅŠgŽU",
            needPoint = 1000,
            action = () => { GrassManager.instance.hasSpreadSeedSkill = true; },
            onece = true
        },
        new SkillItem()
        {
            index = 3,
            name = "Šg‘å‘£i",
            needPoint = 1500,
            action = () => { GrassManager.instance.naturalSpreadIntervalTime -= 2; },
            onece = false
        },
        new SkillItem()
        {
            index = 4,
            name = "¬’·‘£i",
            needPoint = 1500,
            action = () => { GrassManager.instance.autoLevelUpIntervalTime -= 2; },
            onece = false
        },
    };
    public static SkillPanel instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClickSkillButton()
    {
        isOpenSkillPanel = !isOpenSkillPanel;
        if (isOpenSkillPanel) skillButtonImage.sprite = skillButtonOnSprite;
        else skillButtonImage.sprite = skillButtonOffSprite;
        skillPanel.SetActive(isOpenSkillPanel);
        if (!isOpenSkillPanel) return;
        RefreshSkillPanel();
    }
    public void RefreshSkillPanel()
    {
        var ableSkills = skillItems.Where(sk => sk.buyable).ToList();
        foreach (var ableSkill in ableSkills)Debug.Log(ableSkill.name);
        for (int i = 0; i < skillBuyButtons.Count; i++)
        {
            if (i >= ableSkills.Count())
            {
                skillBuyButtons[i].Disable = true;
                skillBuyButtons[i].gameObject.SetActive(false);
            }
            else
            {
                skillBuyButtons[i].Disable = false;
                skillBuyButtons[i].buttonLabelText.text = $"{ableSkills[i].name}\n-{ableSkills[i].needPoint}pt";
                skillBuyButtons[i].skillItemIndex = ableSkills[i].index;
            }
        }
    }
}
public class SkillItem
{
    public int index;
    public string name;
    public int needPoint;
    public System.Action action;
    public bool onece;
    public bool buyable = true;
    public SkillItem()
    {

    }
    public SkillItem(SkillItem skill)
    {
        index = skill.index;
        name = skill.name;
        needPoint = skill.needPoint;
        action = skill.action;
        onece = skill.onece;
        buyable = skill.buyable;
    }
}
