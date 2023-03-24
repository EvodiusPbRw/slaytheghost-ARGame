using System.Collections;
using System.Collections.Generic;
using UnityEngine.Android;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using static QuestModel;
using static ItemModel;
using static PlayerModel;

public class LoadAllData : MonoBehaviour
{
    #region Quest
    public Text TitleQuest;
    public Text ProgressQuest;
    public Text objectiveLocation;
    #endregion


    #region Items
    public TextAsset textJsonItem;
    public List<Sprite> icons;
    List<Item> items = new List<Item>();
    #endregion

    
    #region Player
    public TextAsset textJsonPlayer;
    #endregion

    #region Talent
    public List<Sprite> skillicons;
    public List<Sprite> bursticons;
    #endregion

    #region Status
    private RawImage healthbarwidth;
    private RawImage staminabarwidth;
    #endregion

    void Awake()
    {
        //LoadPlayer();
        InitLoadQuest();
        if(AbstractPlayerData.items.Count == 0)
            InitLoadItem();
        if(AbstractPlayerData.skillicons.Count == 0 && AbstractPlayerData.bursticons.Count == 0)
            InitLoadTalentSource();
        LoadMiniStatus(); 
    }
    
    void Update()
    {
        if(GameObject.Find("[Box] Bag") != null) 
            if(GameObject.Find("[Box] Bag").activeSelf == true) 
                StartCoroutine("LoadItem");
        if(GameObject.Find("[Box] Detail Status") != null)
            if(GameObject.Find("[Box] Detail Status").activeSelf == true) 
                StartCoroutine("LoadLargeStatus");
        if(GameObject.Find("[Box] Detail Mission") != null && AbstractPlayerData.mainQuest[AbstractPlayerData.mainQuest.Count-1].IsFinish == false) 
            StartCoroutine("LoadLargeQuest");
        LoadSecretQuest();
        RefreshQuest();
        ConvertStatusToWidthMiniStatus();
    }

    void LoadPlayer(){
        PlayerModel playerModel = new PlayerModel(textJsonPlayer);
        AbstractPlayerData.player = playerModel.ReaderData();
    }

    void LoadMiniStatus() {
        GameObject parent = GameObject.Find("Status");
        Text firstchild = parent.transform.GetChild(3).gameObject.GetComponent<Text>() as Text;
        healthbarwidth = parent.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.GetComponent<RawImage>() as RawImage;
        staminabarwidth = parent.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.GetComponent<RawImage>() as RawImage;
        firstchild.text = AbstractPlayerData.player.Name;

        ConvertStatusToWidthMiniStatus();
    }

    void ConvertStatusToWidthMiniStatus() {
        float maxbarwidth = 140f;
        float scalinghealthpoint = 0f;
        float scalingstaminapoint = 0f;

        scalinghealthpoint = (maxbarwidth/(float)AbstractPlayerData.player.Status.MaxHealthPoint) * (float)AbstractPlayerData.player.Status.HealthPoint;
        scalingstaminapoint = (maxbarwidth/(float)AbstractPlayerData.player.Status.MaxChargePoint) * (float)AbstractPlayerData.player.Status.ChargePoint;
        healthbarwidth.rectTransform.sizeDelta = new Vector2(scalinghealthpoint, 15f);
        healthbarwidth.rectTransform.localPosition = new Vector3(-(maxbarwidth/2)+(scalinghealthpoint/2),0,0);
        staminabarwidth.rectTransform.sizeDelta = new Vector2(scalingstaminapoint, 15f);
        staminabarwidth.rectTransform.localPosition = new Vector3(-(maxbarwidth/2)+(scalingstaminapoint/2),0,0);
    }

    void LoadLargeStatus(){
        GameObject parent = GameObject.Find("[Box] Detail Status");
        GameObject childeldest = parent.transform.GetChild(0).gameObject;
        GameObject childyoungest = parent.transform.GetChild(1).gameObject;

        Text childeldest_name = childeldest.transform.GetChild(1).gameObject.GetComponent<Text>() as Text;
        Text childeldest_job = childeldest.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;
        Text childeldest_level = childeldest.transform.GetChild(3).gameObject.GetComponent<Text>() as Text;
        GameObject childeldest_bar = childeldest.transform.GetChild(5).gameObject;
        GameObject childeldest_status = childeldest.transform.GetChild(6).gameObject;

        GameObject grandchildeldest_hpwrapper = childeldest_bar.transform.GetChild(0).gameObject;
        RawImage grandchildeldest_hpwrapper_hp = grandchildeldest_hpwrapper.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<RawImage>() as RawImage;
        Text grandchildeldest_hpwrapper_text = grandchildeldest_hpwrapper.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;
        GameObject grandchildeldest_spwrapper = childeldest_bar.transform.GetChild(1).gameObject;
        RawImage grandchildeldest_spwrapper_sp = grandchildeldest_spwrapper.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<RawImage>() as RawImage;
        Text grandchildeldest_spwrapper_text = grandchildeldest_spwrapper.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;
        GameObject grandchildeldest_expwrapper = childeldest_bar.transform.GetChild(2).gameObject;
        RawImage grandchildeldest_expwrapper_exp = grandchildeldest_expwrapper.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<RawImage>() as RawImage;
        Text grandchildeldest_expwrapper_text = grandchildeldest_expwrapper.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;

        Text childeldest_status_maxhp = childeldest_status.transform.GetChild(3).gameObject.GetComponent<Text>() as Text;
        Text childeldest_status_maxsp = childeldest_status.transform.GetChild(4).gameObject.GetComponent<Text>() as Text;
        Text childeldest_status_atk = childeldest_status.transform.GetChild(5).gameObject.GetComponent<Text>() as Text;
        Text childeldest_status_atkinv = childeldest_status.transform.GetChild(9).gameObject.GetComponent<Text>() as Text;
        Text childeldest_status_def = childeldest_status.transform.GetChild(10).gameObject.GetComponent<Text>() as Text;
        Text childeldest_status_er = childeldest_status.transform.GetChild(11).gameObject.GetComponent<Text>() as Text;

        GameObject grandchildyoungest_skillwrapper = childyoungest.transform.GetChild(1).gameObject;
        GameObject grandchildyoungest_burstwrapper = childyoungest.transform.GetChild(2).gameObject;

        GameObject grandchildyoungest_skillwrapper_skillslots = grandchildyoungest_skillwrapper.transform.GetChild(2).gameObject;
        GameObject grandchildyoungest_burstwrapper_burstslots = grandchildyoungest_burstwrapper.transform.GetChild(2).gameObject;

        childeldest_name.text = AbstractPlayerData.player.Name;
        childeldest_job.text = AbstractPlayerData.player.Job;
        childeldest_level.text = "Level " + AbstractPlayerData.player.Level;

        grandchildeldest_hpwrapper_text.text = AbstractPlayerData.player.Status.HealthPoint + "/" + AbstractPlayerData.player.Status.MaxHealthPoint;
        grandchildeldest_spwrapper_text.text = AbstractPlayerData.player.Status.ChargePoint + "/" + AbstractPlayerData.player.Status.MaxChargePoint;
        grandchildeldest_expwrapper_text.text = AbstractPlayerData.player.Status.ExperiencePoint + "/" + AbstractPlayerData.player.Status.ExperienceCap;

        childeldest_status_maxhp.text = AbstractPlayerData.player.Status.MaxHealthPoint.ToString();
        childeldest_status_maxsp.text = AbstractPlayerData.player.Status.MaxChargePoint.ToString();
        childeldest_status_atk.text = AbstractPlayerData.player.Status.BaseAttack.ToString();
        childeldest_status_atkinv.text = AbstractPlayerData.player.Status.AttackCooldown.ToString();
        childeldest_status_def.text = AbstractPlayerData.player.Status.BaseDefense.ToString();
        childeldest_status_er.text = AbstractPlayerData.player.Status.BaseRechargePoint.ToString();

        float maxbarwidth = 200f;
        float scalingbarhp = (maxbarwidth/(float)AbstractPlayerData.player.Status.MaxHealthPoint) * (float)AbstractPlayerData.player.Status.HealthPoint;
        float scalingbarsp = (maxbarwidth/(float)AbstractPlayerData.player.Status.MaxChargePoint) * (float)AbstractPlayerData.player.Status.ChargePoint;
        float scalingbarexp = (maxbarwidth/(float)AbstractPlayerData.player.Status.ExperienceCap) * (float)AbstractPlayerData.player.Status.ExperiencePoint;

        grandchildeldest_hpwrapper_hp.rectTransform.sizeDelta = new Vector2(scalingbarhp,15f);
        grandchildeldest_hpwrapper_hp.rectTransform.localPosition = new Vector3(-(maxbarwidth/2) + (scalingbarhp/2),0,0);
        grandchildeldest_spwrapper_sp.rectTransform.sizeDelta = new Vector2(scalingbarsp,15f);
        grandchildeldest_spwrapper_sp.rectTransform.localPosition = new Vector3(-(maxbarwidth/2) + (scalingbarsp/2),0,0);
        grandchildeldest_expwrapper_exp.rectTransform.sizeDelta = new Vector2(scalingbarexp,15f);
        grandchildeldest_expwrapper_exp.rectTransform.localPosition = new Vector3(-(maxbarwidth/2) + (scalingbarexp/2),0,0);

        AddTalentToSlot("Skill",AbstractPlayerData.skillicons.Count);
        AddTalentToSlot("Burst", AbstractPlayerData.bursticons.Count);
    }

    void AddTalentToSlot(string name, int cnt){
        for(int i = 0; i < cnt; i++) {
            GameObject children = GameObject.Find(name + " Slots/[" + (i+1) + "] Slot");
            GameObject children_item = children.transform.GetChild(1).gameObject;
            children_item.SetActive(true);
            Image children_item_icon = children_item.transform.GetChild(1).gameObject.GetComponent<Image>() as Image;
            Text children_item_name = children_item.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;

            if(children_item.GetComponent<Button>() == null)
            {
                Button children_button = children_item.AddComponent<Button>() as Button;

                if(name == "Skill") {
                    children_item_icon.sprite = AbstractPlayerData.skillicons[i];
                    children_item_name.text = AbstractPlayerData.player.Talent.Skill[i].Name;
                }
                
                else  {
                    children_item_icon.sprite = AbstractPlayerData.bursticons[i];
                    children_item_name.text = AbstractPlayerData.player.Talent.Burst[i].Name;
                }

                children_button.onClick.AddListener(delegate()
                    {
                        GameObject parent = GameObject.Find("[Box] Detail Status");
                        GameObject child = parent.transform.GetChild(3).gameObject;

                        GameObject panel = child.transform.GetChild(0).gameObject;
                        if (panel.GetComponent<Button>() == null) {
                            Button panel_button = panel.AddComponent<Button>() as  Button;
                            panel_button.onClick.AddListener(delegate() { child.SetActive(false); });
                        }


                        int num = int.Parse(children.name.Substring(1,1))-1;
                        child.SetActive(true);

                        GameObject child_infowrapper = child.transform.GetChild(1).gameObject;
                        GameObject grandchild = child_infowrapper.transform.GetChild(1).gameObject;

                        Image grandchild_icon = grandchild.transform.GetChild(0).gameObject.GetComponent<Image>() as Image;
                        Text grandchild_name = grandchild.transform.GetChild(1).gameObject.GetComponent<Text>() as Text;
                        Text grandchild_type = grandchild.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;
                        Text grandchild_element = grandchild.transform.GetChild(3).gameObject.GetComponent<Text>() as Text;
                        Text grandchild_description = grandchild.transform.GetChild(6).gameObject.GetComponent<Text>() as Text;
                        GameObject grandchild_modifier = grandchild.transform.GetChild(7).gameObject;
                        GameObject grandchild_isused = grandchild.transform.GetChild(8).gameObject;

                        Text grandchild_modifier_charge = grandchild_modifier.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;
                        Text grandchild_modifier_dmg = grandchild_modifier.transform.GetChild(5).gameObject.GetComponent<Text>() as Text;
                        Text grandchild_modifier_chargevalue = grandchild_modifier.transform.GetChild(6).gameObject.GetComponent<Text>() as Text;
                        Text grandchild_modifier_cd = grandchild_modifier.transform.GetChild(7).gameObject.GetComponent<Text>() as Text;
                        Text grandchild_modifier_ls = grandchild_modifier.transform.GetChild(8).gameObject.GetComponent<Text>() as Text;

                        if (name == "Skill") {
                            grandchild_icon.sprite = AbstractPlayerData.skillicons[num];
                            grandchild_name.text = AbstractPlayerData.player.Talent.Skill[num].Name;
                            grandchild_type.text = "Skill";
                            grandchild_element.text = AbstractPlayerData.player.Talent.Skill[num].ElementType;
                            grandchild_description.text = AbstractPlayerData.player.Talent.Skill[num].Description;
                            grandchild_modifier_dmg.text = AbstractPlayerData.player.Talent.Skill[num].DamageMultiplier.ToString();
                            grandchild_modifier_charge.text = "Charge Multiplier";
                            grandchild_modifier_chargevalue.text = AbstractPlayerData.player.Talent.Skill[num].ChargeMultiplier.ToString();
                            grandchild_modifier_cd.text = AbstractPlayerData.player.Talent.Skill[num].Cooldown.ToString();
                            grandchild_modifier_ls.text = AbstractPlayerData.player.Talent.Skill[num].LifeSteal.ToString();
                            Button grandchild_isused_button = grandchild_isused.GetComponent<Button>() as Button;
                            Text grandchild_isused_text = grandchild_isused.transform.GetChild(0).GetComponent<Text>() as Text;
                            grandchild_isused_text.text = AbstractPlayerData.player.Talent.Skill[num].IsUsed? "Used" : "Use";
                            grandchild_isused_button.onClick.RemoveAllListeners();
                            grandchild_isused_button.onClick.AddListener(delegate() {
                                if(name == "Skill") {
                                    for(int i = 0; i<AbstractPlayerData.player.Talent.Skill.Count; i++)
                                    {
                                        AbstractPlayerData.player.Talent.Skill[i].IsUsed = false;
                                    }
                                    AbstractPlayerData.player.Talent.Skill[num].IsUsed = true;
                                    grandchild_isused_text.text = AbstractPlayerData.player.Talent.Skill[num].IsUsed? "Used" : "Use";
                                }

                                else if(name == "Burst") {
                                    for(int i = 0; i<AbstractPlayerData.player.Talent.Burst.Count; i++)
                                    {
                                        AbstractPlayerData.player.Talent.Burst[i].IsUsed = false;
                                    }
                                    AbstractPlayerData.player.Talent.Burst[num].IsUsed = true;
                                    grandchild_isused_text.text = AbstractPlayerData.player.Talent.Burst[num].IsUsed? "Used" : "Use";

                                }
                            });
                        }
                        else {
                            grandchild_icon.sprite = AbstractPlayerData.bursticons[num];
                            grandchild_name.text = AbstractPlayerData.player.Talent.Burst[num].Name;
                            grandchild_type.text = "Burst";
                            grandchild_element.text = AbstractPlayerData.player.Talent.Burst[num].ElementType;
                            grandchild_description.text = AbstractPlayerData.player.Talent.Burst[num].Description;
                            grandchild_modifier_dmg.text = AbstractPlayerData.player.Talent.Burst[num].DamageMultiplier.ToString();
                            grandchild_modifier_charge.text = "Charge Point";
                            grandchild_modifier_chargevalue.text = AbstractPlayerData.player.Talent.Burst[num].ChargePoint.ToString();
                            grandchild_modifier_cd.text = AbstractPlayerData.player.Talent.Burst[num].Cooldown.ToString();
                            grandchild_modifier_ls.text = AbstractPlayerData.player.Talent.Burst[num].LifeSteal.ToString();
                            Button grandchild_isused_button = grandchild_isused.GetComponent<Button>() as Button;
                            Text grandchild_isused_text = grandchild_isused.transform.GetChild(0).GetComponent<Text>() as Text;
                            grandchild_isused_text.text = AbstractPlayerData.player.Talent.Burst[num].IsUsed? "Used" : "Use";
                            grandchild_isused_button.onClick.RemoveAllListeners();
                            grandchild_isused_button.onClick.AddListener(delegate() {
                                if(name == "Burst") {
                                    for(int i = 0; i<AbstractPlayerData.player.Talent.Burst.Count; i++)
                                    {
                                        AbstractPlayerData.player.Talent.Burst[i].IsUsed = false;
                                    }
                                    AbstractPlayerData.player.Talent.Burst[num].IsUsed = true;
                                    grandchild_isused_text.text = AbstractPlayerData.player.Talent.Burst[num].IsUsed? "Used" : "Use";
                                }
                            });
                        }

                        
                    }
                );
            }
        }
    }

    void InitLoadQuest()
    {
        foreach(MainQuest mq in AbstractPlayerData.mainQuest) {
            if(mq.IsFinish == false) 
            {
                PlayerPrefs.SetString("currentObjectiveLocation", mq.QuestLocation);
                TitleQuest.text = mq.Title;
                ProgressQuest.text = mq.Fulfill + "/" + mq.Request;
                break;        
            } 
            else if (mq == null)
            {
                PlayerPrefs.SetString("currentObjectiveLocation", "Agape");
                TitleQuest.text = "Tamat";
                ProgressQuest.text = "Done";
                break; 
            }
        }

    }

    void LoadLargeQuest(){
        MainQuest quest = AbstractPlayerData.GetCurrentMainQuest();
        GameObject parent = GameObject.Find("[Box] Detail Mission");
        Text child_title = parent.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;
        Text child_location = parent.transform.GetChild(3).gameObject.GetComponent<Text>() as Text;
        Text child_description = parent.transform.GetChild(4).gameObject.GetComponent<Text>() as Text;
        GameObject child_typewrapper = parent.transform.GetChild(5).gameObject;
        GameObject child_progresswrapper = parent.transform.GetChild(6).gameObject;
        GameObject child_talentwrapper = parent.transform.GetChild(7).gameObject;
        GameObject child_buttonobj = parent.transform.GetChild(9).gameObject;
        Button child_button = child_buttonobj.GetComponent<Button>() as Button;
        Text child_typewrapper_value = child_typewrapper.transform.GetChild(1).gameObject.GetComponent<Text>() as Text;
        Text child_progresswrapper_value = child_progresswrapper.transform.GetChild(1).gameObject.GetComponent<Text>() as Text;
        Text grandchild_talentwrapper_expText = child_talentwrapper.transform.GetChild(1).gameObject.GetComponent<Text>() as Text;
        GameObject grandchild_talentwrapper = child_talentwrapper.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
        Image grandchild_talentwrapper_icon = grandchild_talentwrapper.transform.GetChild(0).gameObject.GetComponent<Image>() as Image;
        Text grandchild_talentwrapper_name = grandchild_talentwrapper.transform.GetChild(1).gameObject.GetComponent<Text>() as Text;
        Text grandchild_talentwrapper_type = grandchild_talentwrapper.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;
        Text grandchild_talentwrapper_description = grandchild_talentwrapper.transform.GetChild(3).gameObject.GetComponent<Text>() as Text;        

        child_title.text = quest.Title;
        child_location.text = quest.QuestLocation;
        child_description.text = quest.Description;

        child_typewrapper_value.text = quest.MonsterType;
        child_progresswrapper_value.text = quest.Fulfill + " / " + quest.Request;

        grandchild_talentwrapper_expText.text = "EXP : " + quest.Reward.exp;

        grandchild_talentwrapper.SetActive(false);
        if (quest.Reward.Skill != null || quest.Reward.Burst != null)
        {
            grandchild_talentwrapper.SetActive(true);
            if(quest.Reward.Skill != null)
            {
                foreach(Sprite sprite in skillicons)
                {
                    if(quest.Reward.Skill.SourceIcon == sprite.name)
                    {
                        grandchild_talentwrapper_icon.sprite = sprite;
                        break;
                    }
                }
                grandchild_talentwrapper_name.text = quest.Reward.Skill.Name;
                grandchild_talentwrapper_type.text = "Skill";
                grandchild_talentwrapper_description.text = quest.Reward.Skill.Description;
            }
            else
            {
                foreach(Sprite sprite in bursticons)
                {
                    if(quest.Reward.Burst.SourceIcon == sprite.name)
                    {
                        grandchild_talentwrapper_icon.sprite = sprite;
                        break;
                    }
                }
                
                grandchild_talentwrapper_name.text = quest.Reward.Burst.Name;
                grandchild_talentwrapper_type.text = "Burst";
                grandchild_talentwrapper_description.text = quest.Reward.Burst.Description;
            }
        }

        child_buttonobj.SetActive(false);
        if(quest.Fulfill == quest.Request)
        {
            child_buttonobj.SetActive(true);
            child_button.onClick.RemoveAllListeners();
            child_button.onClick.AddListener(() => {
                for(int i = 0; i < AbstractPlayerData.mainQuest.Count;i++)
                {
                    if (quest.QuestID == AbstractPlayerData.mainQuest[i].QuestID)
                    {
                        AbstractPlayerData.userSession.setUserQuestTime(quest.QuestID);
                        AbstractPlayerData.mainQuest[i].IsFinish = true;
                        AbstractPlayerData.player.Status.ExperiencePoint += AbstractPlayerData.mainQuest[i].Reward.exp;
                        AbstractPlayerData.notification.Add(new Notification("Anda mendapatkan " +  AbstractPlayerData.mainQuest[i].Reward.exp + " EXP dari Main Quest", false));                        
                        if (AbstractPlayerData.player.Status.ExperiencePoint >= AbstractPlayerData.player.Status.ExperienceCap)
                        {
                            AbstractPlayerData.notification.Add(new Notification("Anda naik level!", false));
                            AbstractPlayerData.LevelUp();
                        }

                        if (quest.Reward.Skill != null || quest.Reward.Burst != null)
                        {
                            if (quest.Reward.Skill != null)
                            {   
                                AbstractPlayerData.player.Talent.Skill.Add(quest.Reward.Skill);
                                foreach(Sprite sprite in skillicons) {
                                    if (sprite.name ==quest.Reward.Skill.SourceIcon) {
                                        AbstractPlayerData.notification.Add(new Notification("You got new Talent " + quest.Reward.Skill.Name + "from (?)", false));
                                        AbstractPlayerData.skillicons.Add(sprite);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                AbstractPlayerData.player.Talent.Burst.Add(quest.Reward.Burst);
                                foreach(Sprite sprite in bursticons) {
                                    if (sprite.name ==quest.Reward.Burst.SourceIcon) {
                                        AbstractPlayerData.notification.Add(new Notification("You got new Talent " + quest.Reward.Burst.Name + "from (?)", false));
                                        AbstractPlayerData.bursticons.Add(sprite);
                                        break;
                                    }
                                }
                            }
                        }

                        if (AbstractPlayerData.mainQuest[AbstractPlayerData.mainQuest.Count - 1].IsFinish == true){
                            LoadingData.sceneToLoad = "Story";
                            SceneManager.LoadScene("Loading");
                        }

                        if (AbstractPlayerData.mainQuest[i].IsCheckpoint == true)
                        {
                            AbstractPlayerData.checkpoint.serializedplayer = JsonConvert.SerializeObject(AbstractPlayerData.playermodel.GetRoot());
                            AbstractPlayerData.checkpoint.serializedquest = JsonConvert.SerializeObject(AbstractPlayerData.questmodel.GetRoot());
                            AbstractPlayerData.checkpoint.SaveDataCheckpoint();
                        }
                        break;
                    }
                }
                
            });
        }
    }

    void RefreshQuest(){
        foreach(MainQuest mq in AbstractPlayerData.mainQuest) {
            if(mq.IsFinish == false) 
            {
                TitleQuest.text = mq.Title;
                ProgressQuest.text = mq.Fulfill + "/" + mq.Request;      
                break;  
            }
            
        }
    }

    void InitLoadTalentSource(){
        for(int i = 0 ; i < AbstractPlayerData.player.Talent.Skill.Count; i++) {
            foreach(Sprite sprite in skillicons) {
                if (sprite.name == AbstractPlayerData.player.Talent.Skill[i].SourceIcon) {
                    AbstractPlayerData.skillicons.Add(sprite);
                }
            }
        }
        for(int i = 0 ; i < AbstractPlayerData.player.Talent.Burst.Count; i++){
            foreach(Sprite sprite in bursticons) {
                if (sprite.name == AbstractPlayerData.player.Talent.Burst[i].SourceIcon) {
                    AbstractPlayerData.bursticons.Add(sprite);
                }
            }
        }
        
    }

    void LoadSecretQuest()
    {
        SideQuest quest = AbstractPlayerData.GetCurrentSecretQuest();   
        GameObject parent = GameObject.Find("Mail");
        GameObject child = parent.transform.GetChild(2).gameObject;
        Text child_text = child.transform.GetChild(1).gameObject.GetComponent<Text>() as Text;

        child.SetActive(false);

        int cnt = 0;
        for(int i = 0; i < AbstractPlayerData.notification.Count; i++)
        {
            if (AbstractPlayerData.notification[i].IsRead == false)
            {
                cnt += 1;
            }
        }

        if(cnt > 0)
        {
            child.SetActive(true);
            child_text.text = cnt.ToString();
        } 
        else 
        {
            child.SetActive(false);
        }

        parent.GetComponent<Button>().onClick.RemoveAllListeners();
        parent.GetComponent<Button>().onClick.AddListener(() => {
            for(int i = 0; i < AbstractPlayerData.notification.Count;i++)
            {
                AbstractPlayerData.notification[i].IsRead = true;
            }
            GameObject.Find("User Interface").transform.GetChild(13).gameObject.SetActive(true);
            LoadNotification();
        });

        if (quest != null){
            if (quest.Fulfill == quest.Request && quest.IsFinish == false)
            {
                for(int i = 0; i < AbstractPlayerData.sideQuest.Count;i++)
                {
                    if (AbstractPlayerData.sideQuest[i].QuestID == quest.QuestID)
                    {
                        AbstractPlayerData.player.Status.ExperiencePoint += AbstractPlayerData.sideQuest[i].Reward.exp;
                        AbstractPlayerData.notification.Add(new Notification("Anda mendapatkan " +  AbstractPlayerData.sideQuest[i].Reward.exp + " EXP dari (?)", false));
                        if (AbstractPlayerData.player.Status.ExperiencePoint >= AbstractPlayerData.player.Status.ExperienceCap) 
                        {
                            AbstractPlayerData.notification.Add(new Notification("Anda naik Level!", false));
                            AbstractPlayerData.LevelUp();
                        }
                        AbstractPlayerData.sideQuest[i].IsFinish = true;
                    }
                }


                if (quest.Reward.Skill != null || quest.Reward.Burst != null){
                    if (quest.Reward.Skill != null)
                    {   
                        AbstractPlayerData.player.Talent.Skill.Add(quest.Reward.Skill);
                        AbstractPlayerData.notification.Add(new Notification("You got new Talent " + quest.Reward.Skill.Name + "from (?)", false));
                        foreach(Sprite sprite in skillicons) {
                            if (sprite.name ==quest.Reward.Skill.SourceIcon) {
                                AbstractPlayerData.skillicons.Add(sprite);
                                break;
                            }
                        }
                    }
                    else
                    {
                        AbstractPlayerData.player.Talent.Burst.Add(quest.Reward.Burst);
                        AbstractPlayerData.notification.Add(new Notification("You got new Talent " + quest.Reward.Burst.Name + "from (?)", false));
                        foreach(Sprite sprite in bursticons) {
                            if (sprite.name ==quest.Reward.Burst.SourceIcon) {
                                AbstractPlayerData.bursticons.Add(sprite);
                                break;
                            }
                        }
                    }
                }
                
            }
        }

        
        
    }

    void InitLoadItem(){
        ItemModel itemModel = new ItemModel(textJsonItem);
        this.items = itemModel.ReaderData();
        
        for(int i = 0; i < AbstractPlayerData.player.Slots.Count;i++)
        {
            foreach(Item item in items) 
            {
                if(item.ItemID == AbstractPlayerData.player.Slots[i].ItemID)
                {
                    AbstractPlayerData.items.Add(item);
                }    
                if(AbstractPlayerData.player.Slots[i].IsUsed == true && item.ItemID == AbstractPlayerData.player.Slots[i].ItemID)
                {
                    foreach(Sprite sprite in icons){
                        if (sprite.name == item.Source)
                        {
                            AbstractPlayerData.itemicon = sprite;
                        }
                    }
                }
            }
        } 
    }

    void LoadItem(){
        for(int i = 0; i < AbstractPlayerData.items.Count; i++) 
        {
            foreach(Sprite sprite in icons)
            {
                if(AbstractPlayerData.items[i].Source == sprite.name) {
                    
                    GameObject parent = GameObject.Find("[" + (i+1) + "] Slot");
                    GameObject childeldest = parent.transform.GetChild(1).gameObject;

                    childeldest.SetActive(true);

                    GameObject grandchild_background = childeldest.transform.GetChild(0).gameObject;
                    GameObject grandchild_icon = childeldest.transform.GetChild(1).gameObject;
                    GameObject grandchild_name = childeldest.transform.GetChild(2).gameObject;

                    Image grandchild_background_image = grandchild_background.GetComponent<Image>() as Image;
                    Image grandchild_icon_image = grandchild_icon.GetComponent<Image>() as Image;
                    Text grandchild_name_text = grandchild_name.GetComponent<Text>() as Text;

                    if(childeldest.GetComponent<Button>()==null) {
                        Button childeldest_button = childeldest.AddComponent<Button>() as Button;
                        childeldest_button.onClick.AddListener(delegate() 
                            {
                                int num = int.Parse(parent.name.Substring(1,1))-1;

                                GameObject.Find("[Box] Bag").transform.GetChild(0).gameObject.transform.localPosition = new Vector3(0,-250f,0);
                                GameObject.Find("[Box] Bag").transform.GetChild(1).gameObject.SetActive(true);

                                GameObject itemIdentity = GameObject.Find("[Box] Bag").transform.GetChild(1).gameObject.transform.GetChild(1).gameObject;

                                Image itemIdentity_icon = itemIdentity.transform.GetChild(0).gameObject.GetComponent<Image>() as Image;
                                Text itemIdentity_name = itemIdentity.transform.GetChild(1).gameObject.GetComponent<Text>() as Text;
                                Text itemIdentity_nick = itemIdentity.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;
                                Text itemIdentity_type = itemIdentity.transform.GetChild(3).gameObject.GetComponent<Text>() as Text;
                                Text itemIdentity_desc = itemIdentity.transform.GetChild(6).gameObject.GetComponent<Text>() as Text;
                                GameObject itemIdentity_consume = itemIdentity.transform.GetChild(7).gameObject;
                                Button itemIdentity_consume_button = itemIdentity_consume.GetComponent<Button>() as Button;
                                GameObject itemIdentity_stat = itemIdentity.transform.GetChild(8).gameObject;
                                Button itemIdentity_button = itemIdentity.transform.GetChild(9).gameObject.GetComponent<Button>() as Button;
                                Text itemIdentity_button_name = itemIdentity.transform.GetChild(9).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>() as Text;
                                Text itemIdentity_qty = itemIdentity.transform.GetChild(10).gameObject.GetComponent<Text>() as Text;
                                Text itemIdentity_hp = itemIdentity_stat.transform.GetChild(1).gameObject.GetComponent<Text>() as Text;
                                Text itemIdentity_sp = itemIdentity_stat.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;
                                Text itemIdentity_xp = itemIdentity_stat.transform.GetChild(3).gameObject.GetComponent<Text>() as Text;
                                Text itemIdentity_at = itemIdentity_stat.transform.GetChild(4).gameObject.GetComponent<Text>() as Text;

                                itemIdentity_icon.sprite = sprite;
                                itemIdentity_name.text = AbstractPlayerData.items[num].Name;
                                itemIdentity_nick.text = AbstractPlayerData.items[num].Nick;
                                itemIdentity_type.text = AbstractPlayerData.items[num].Type;
                                itemIdentity_desc.text = AbstractPlayerData.items[num].Description;
                                itemIdentity_qty.text = "Qty : " + AbstractPlayerData.player.Slots[num].Quantity;

                                itemIdentity_hp.text = "HP : " + AbstractPlayerData.items[num].StatusModifier.hp.ToString();
                                itemIdentity_sp.text = "SP : " + AbstractPlayerData.items[num].StatusModifier.stamina.ToString();
                                itemIdentity_xp.text = "XP : " + AbstractPlayerData.items[num].StatusModifier.exp.ToString();
                                itemIdentity_at.text = "AT : " + AbstractPlayerData.items[num].StatusModifier.attack.ToString();

                                itemIdentity_button.onClick.AddListener(delegate()
                                    {
                                        for(int i = 0; i<AbstractPlayerData.player.Slots.Count; i++)
                                        {
                                            AbstractPlayerData.player.Slots[i].IsUsed = false;
                                        }
                                        AbstractPlayerData.itemicon = sprite;
                                        AbstractPlayerData.player.Slots[num].IsUsed = true;
                                        itemIdentity_button_name.text = AbstractPlayerData.player.Slots[num].IsUsed? "Used" : "Use";
                                    }
                                );
                                itemIdentity_button_name.text = AbstractPlayerData.player.Slots[num].IsUsed? "Used" : "Use";

                                
                                itemIdentity_consume_button.onClick.RemoveAllListeners();

                                if (AbstractPlayerData.items[num].Type == "Consumable") {                                    
                                    itemIdentity_consume.SetActive(true);
                                    if(AbstractPlayerData.player.Slots[num].Quantity > 0){
                                        itemIdentity_consume_button.interactable = true;
                                    }
                                            
                                    itemIdentity_consume_button.onClick.AddListener(() => {
                                        PotionUse(num, AbstractPlayerData.items[num].Nick);        
                                        if(AbstractPlayerData.player.Slots[num].Quantity <= 0){
                                            AbstractPlayerData.player.Slots[num].Quantity = 0;
                                            itemIdentity_consume_button.interactable = false;
                                        }
                                        itemIdentity_qty.text = "Qty : " + AbstractPlayerData.player.Slots[num].Quantity;
                                    });
                                } 
                                else 
                                {
                                    itemIdentity_consume.SetActive(false); 
                                }  
                            }
                        );
                    }
                    

                    if (AbstractPlayerData.player.Slots[i].IsUsed == true){
                        var tmp = grandchild_background_image.color;
                        tmp.a = 255f;
                        grandchild_background_image.color = tmp;
                    }
                    else {
                        var tmp = grandchild_background_image.color;
                        tmp.a = 140f;
                        grandchild_background_image.color = tmp;
                    }

                    grandchild_icon_image.sprite = sprite;
                    grandchild_name_text.text =  AbstractPlayerData.items[i].Nick;
                }
            }
        }

    }

    void PotionUse(int num, string name){ 
        if (name == "HP Potion")
        {
            AbstractPlayerData.player.Status.HealthPoint += (AbstractPlayerData.player.Status.MaxHealthPoint * AbstractPlayerData.items[num].StatusModifier.hp);
            if (AbstractPlayerData.player.Status.HealthPoint > AbstractPlayerData.player.Status.MaxHealthPoint) 
                AbstractPlayerData.player.Status.HealthPoint = AbstractPlayerData.player.Status.MaxHealthPoint; 
            AbstractPlayerData.player.Slots[num].Quantity -= 1;
        } 
        else 
        {
            AbstractPlayerData.player.Status.ChargePoint += (AbstractPlayerData.player.Status.MaxChargePoint * AbstractPlayerData.items[num].StatusModifier.stamina);
            if (AbstractPlayerData.player.Status.ChargePoint > AbstractPlayerData.player.Status.MaxChargePoint) 
                AbstractPlayerData.player.Status.ChargePoint = AbstractPlayerData.player.Status.MaxChargePoint;
            AbstractPlayerData.player.Slots[num].Quantity -= 1;
        }   
    }

    void LoadNotification()
    {
        GameObject parent = GameObject.Find("[Box] Mail");
        GameObject parent_wrapper = parent.transform.GetChild(2).gameObject;

        if(parent_wrapper.transform.childCount > 1 && parent_wrapper.transform.childCount-1 < AbstractPlayerData.notification.Count)
        {
            for(int i = 0; i < parent_wrapper.transform.childCount - 1; i++){
                Destroy(GameObject.Find("[" + (i+1) + "] Notif"));
            }
        }

        float y = 285f;
        for(int i = AbstractPlayerData.notification.Count; i > 0; i--)
        {
            GameObject child = Instantiate(parent_wrapper.transform.GetChild(0).gameObject);
            child.name = "[" + (i) + "] Notif";
            Text child_text = child.transform.GetChild(1).GetComponent<Text>() as Text;
            child_text.text = AbstractPlayerData.notification[i-1].NotifText;
            child.SetActive(true);
            child.transform.parent = parent_wrapper.transform;  
            child.GetComponent<RectTransform>().localPosition = new Vector3(0, y, 0);
            child.GetComponent<RectTransform>().sizeDelta = parent_wrapper.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta;
            child.GetComponent<RectTransform>().localScale = new Vector3(1f,1f,1f);
            y -= 45f;
        }
    }

    public void CloseBag(){
        GameObject.Find("[Box] Bag").transform.GetChild(0).gameObject.transform.localPosition = new Vector3(0,0,0);
        GameObject.Find("[Box] Bag").transform.GetChild(1).gameObject.SetActive(false);
        GameObject.Find("[Box] Bag").SetActive(false);
    }
}
