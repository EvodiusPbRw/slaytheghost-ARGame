using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using static MonsterModel;
using static PlayerModel;
using static ItemModel;
using static QuestModel;

public class Reader : AbstractDataBattle
{
    //Needed Private Value
    private MonsterModel monsterModel;
    private PlayerModel playerModel;
    //End Needed Private Value

    //Needed Public Value
    public TextAsset textJsonMonster;
    public TextAsset textJsonPlayer;
    public TextAsset textJsonItem;

    public List<GameObject> gObj = new List<GameObject>();

    public Text hpPercentageMonster ;
    public Text hpOriginMonster ; 
    public RawImage hp_bar_monster;

    public Text energyPercentageMonster;
    public Text energyOriginMonster ;
    public RawImage energy_bar_monster; 

    public Text mob_name;
    public Text mob_desc;

    public Text player_name_text;
    public RawImage hp_bar_player;
    public RawImage charge_bar_player;

    public Text burstTimerText;
    public GameObject BurstCooldownPanel;
    public Image ChargePointProgressBurst;

    public Text skillTimerText;
    public GameObject SkillCooldownPanel;

    public GameObject statusMonsterPanel;
    public GameObject resultPanel;

    public GameObject normalHitEffect;
    public List<GameObject> skilleffects;
    public List<GameObject> bursteffects;

    GameObject skillEffect;
    GameObject burstEffect;
    //End Needed Public Value


    //Global Variable

    //Field for Monster
    float object_width; // <-- Bar HealthPoint Monster/Hantu
    float object_x; // <-- Position X Monster/Hantu

    float width_monster_energy; // <-- Bar Energy Monster/Hantu
    float x_pos_monster_energy; // <-- Position X Energy Monster/Hantu
    //End of Field of Monster


    //Field for Player Status

    float width_player_hp;
    float x_pos_player_hp;
    float width_player_charge;
    float x_pos_player_charge;

    //End of Field for Player Status

    //Fiel for Attack
    float _timeAttack = 0;
    float _timeDelayedAttack = 0.1f;

    bool isReadyAttack = true;
    float cooldowntimeAttack;
    //Ene of Field for Attack

    //Field for Burst
    float _timeBurst = 0; // <-- The time begin goes here
    float _timeDelayedBurst = 0.1f; // <--For make the loop/update just in time (not fast or slow enough)

    bool CooldownReadyBurst = true; // <-- Flag for to know Burst done or not
    float CooldownTimeBurst; // <-- How long the cooldown takes
    Button burstButton; // <-- Make it disable or enable with this var (Pusing main cooldown gak tidur 2 hari gegara cooldown)

    float ObjectHeightChargeBurst = 0; // <-- Variable for Set the height equal as charge point held by player
    //End of Field for Burst

    //Field for Skill
    float _timeSkill = 0;
    float _timeDelayedSkill = 0.1f;

    bool CooldownReadySkill = true;
    float CooldownTimeSkill;
    Button skillButton;
    //End of Field for Skill

    string location;
    float chanceFlee = 20f;
    int maxFlee = 3;
    
    Skill skill = new Skill();
    Burst burst = new Burst();

    CapsuleCollider monsterCollider;
    //End Global Variable

    void Awake(){
        AbstractDataBattle.isBattleDone = false;
        AbstractDataBattle.isEnteredCombat = false;
        AbstractDataBattle.isUltiDone = true;
        AbstractDataBattle.maxPlayerIdle = 4f;
        AbstractDataBattle.timerPlayerIdle = 0f;
        AbstractDataBattle.mob=null;
        AbstractDataBattle.monster=null;
        AbstractDataBattle.MonsterAnimate = null;
        AbstractDataBattle.clips = null;
        AbstractDataBattle.timerPlayerIdle = 0f;
    }

    void Start()
    {
        //For read monster data from monster model

        this.monsterModel = new MonsterModel(textJsonMonster);
        List<Monster> tmpList = monsterModel.ReaderData();

        //End of read monster data

        //For read player data from player model
        
        // this.playerModel = new PlayerModel(textJsonPlayer);
        // AbstractPlayerData.player = this.playerModel.ReaderData();

        //End of read player data

        location = "Bero";
        if(PlayerPrefs.HasKey("location"))
        {
            location = PlayerPrefs.GetString("location");
            PlayerPrefs.DeleteKey("location");
        } 

        List<Monster> monsterList = new List<Monster>();

        for(int i = 0 ;i < tmpList.Count; i++) {
            if(location == AbstractPlayerData.GetCurrentMainQuest().QuestLocation) {
                if(tmpList[i].location == location && AbstractPlayerData.GetCurrentMainQuest().MonsterType == tmpList[i].type) {
                    monsterList.Add(tmpList[i]);
                }
            }
            else {
                if(tmpList[i].location == location && tmpList[i].type != "Boss")
                {
                    monsterList.Add(tmpList[i]);
                }
            }
            
        }

        int tmp_rng = Random.Range(0, monsterList.Count);
        AbstractDataBattle.mob = monsterList[tmp_rng];

        for(int i = 0; i < gObj.Count; i++) {
            if(gObj[i].ToString().Substring(0, gObj[i].ToString().IndexOf(' ')) == monsterList[tmp_rng].source){
                AbstractDataBattle.monster = Instantiate(gObj[i], new Vector3(0, 0, 2), Quaternion.Euler(0, 180, 0));
                AbstractDataBattle.monster.tag = "MonsterCollider";
                AbstractDataBattle.monster.name = gObj[i].ToString().Substring(0, gObj[i].ToString().IndexOf(' '));
                AbstractDataBattle.monster.AddComponent<ARAnchor>();
                monsterCollider = AbstractDataBattle.monster.AddComponent<CapsuleCollider>() as CapsuleCollider;
                monsterCollider.height = 1.5f;
                monsterCollider.radius = 0.3f;
                monsterCollider.center = new Vector3(0f,1.25f,0.1f);          
                break;
            }
        }
        
        object_width = 250;
        object_x = 0;

        width_monster_energy = (250f/(float)AbstractDataBattle.mob.energy_cap) * ((float)AbstractDataBattle.mob.energy);
        x_pos_monster_energy = -125f;

        energy_bar_monster.rectTransform.sizeDelta = new Vector2(width_monster_energy, 25);
        energy_bar_monster.rectTransform.localPosition = new Vector3((-125) + width_monster_energy/2,0,0);

        mob_name.text = AbstractDataBattle.mob.cd_name;
        mob_desc.text = AbstractDataBattle.mob.desc;


        //Initiate Player UI Object

        player_name_text.text = AbstractPlayerData.player.Name;
        width_player_hp = (175f/(float)AbstractPlayerData.player.Status.MaxHealthPoint) * (float) AbstractPlayerData.player.Status.HealthPoint;
        width_player_charge = (175f/(float)AbstractPlayerData.player.Status.MaxChargePoint) * (float) AbstractPlayerData.player.Status.ChargePoint;

        //End of Initiate Player Object

        //Initialize Icon and Effect Skill and Burst

        skill = AbstractPlayerData.GetSkillUsed();
        burst = AbstractPlayerData.GetBurstUsed();

        foreach(GameObject obj in skilleffects)
        {
            if(skill.SourceEffect == obj.name) 
            {
                skillEffect = obj;
                break;
            }
        }

        foreach(GameObject obj in bursteffects)
        {
            if(burst.SourceEffect == obj.name) 
            {
                burstEffect = obj;
                break;
            }
        }

        GameObject.Find("Burst/Icon").GetComponent<Image>().sprite = AbstractPlayerData.GetBurstUsedSprite();
        GameObject.Find("Skill/Icon").GetComponent<Image>().sprite = AbstractPlayerData.GetSkillUsedSprite();

        //End of Initialize Icon and Effect Skill and Burst

        //Initialize Height and Position of Burst Bar
        ObjectHeightChargeBurst = (float)(125f/this.burst.ChargePoint) * (float)((AbstractPlayerData.player.Status.ChargePoint - this.burst.ChargePoint <= 0)? AbstractPlayerData.player.Status.ChargePoint : this.burst.ChargePoint) ;
        ChargePointProgressBurst.transform.Translate(0,0.5f,0);
        ChargePointProgressBurst.rectTransform.localPosition = new Vector3(0,(-62.5f)+(ObjectHeightChargeBurst/2),0);
        //End of Initiazlize Height and Position Burst Bar

        //Init Button Skill and Burst
        burstButton = GameObject.Find("Burst").GetComponent<Button>() as Button;
        skillButton = GameObject.Find("Skill").GetComponent<Button>() as Button;
        //End of Init Button Skill and Burst

        //Init Item 

        Item item = AbstractPlayerData.GetItemUsed();
        
        if (item != null)
        {
            chanceFlee += item.Nick == "Flee"? 30f : 0f;
            maxFlee += item.Nick == "Flee"? 1 : 0;
        }

        //End of Init Item
    }

    void Update()
    {
        LoadItemUsed();

        float hpToPercentMonster = (float.Parse(AbstractDataBattle.mob.hp.ToString())*100f) / float.Parse(AbstractDataBattle.mob.max_hp.ToString());
        hpPercentageMonster.text = ((int)hpToPercentMonster).ToString();

        hpOriginMonster.text = AbstractDataBattle.mob.hp.ToString() + " / " + AbstractDataBattle.mob.max_hp.ToString();

        float energyToPercentMonster = (float.Parse(AbstractDataBattle.mob.energy.ToString())*100f) / float.Parse(AbstractDataBattle.mob.energy_cap.ToString());
        energyPercentageMonster.text = ((int)energyToPercentMonster).ToString();

        energyOriginMonster.text = AbstractDataBattle.mob.energy.ToString() + "/" + AbstractDataBattle.mob.energy_cap.ToString();

        
        // Update Frame for Player Stat
        width_player_hp = (175f/(float)AbstractPlayerData.player.Status.MaxHealthPoint) * (float) AbstractPlayerData.player.Status.HealthPoint;
        width_player_charge = (175f/(float)AbstractPlayerData.player.Status.MaxChargePoint) * (float) AbstractPlayerData.player.Status.ChargePoint;

        hp_bar_player.rectTransform.sizeDelta = new Vector2(width_player_hp,15f);
        hp_bar_player.rectTransform.localPosition = new Vector3((-87.5f) + (width_player_hp/2f),0f,0f);
        charge_bar_player.rectTransform.sizeDelta = new Vector2(width_player_charge,15f);
        charge_bar_player.rectTransform.localPosition = new Vector3((-87.5f) + (width_player_charge/2f),0f,0f);
        //End of Update Frame for Player Stat

        ChargePointProgressBurst.rectTransform.sizeDelta = new Vector2(125, ObjectHeightChargeBurst);

        if (Input.touchCount == 1 && !statusMonsterPanel.activeSelf) {
            TouchToAttack(Input.touches[0]);
        }

        if(AbstractDataBattle.mob.hp > 0 && AbstractPlayerData.player.Status.HealthPoint > 0)
        {
            if(AbstractDataBattle.mob.energy == 0)
            {
                energy_bar_monster.rectTransform.sizeDelta = new Vector2(0, 25);
                energy_bar_monster.rectTransform.localPosition = new Vector3((-125) + 0/2,0,0);
            }

        } 
        else if (AbstractDataBattle.mob.hp <= 0 && AbstractDataBattle.isEnteredCombat == true)
        {
            AbstractDataBattle.mob.hp = 0;
            AbstractDataBattle.isEnteredCombat = false;
            AbstractDataBattle.isBattleDone = true;
            this.resultPanel.SetActive(true);
            StartCoroutine(result());
            StartCoroutine("ResultToUI");
            this.resultPanel.AddComponent<Button>().onClick.AddListener(() => {
                LoadingData.sceneToLoad = "InGame";
                SceneManager.LoadScene("Loading");
            });  
        } 
        else if (AbstractPlayerData.player.Status.HealthPoint <= 0 && AbstractDataBattle.isEnteredCombat == true)
        {
            AbstractDataBattle.mob.hp = 0;
            AbstractDataBattle.isEnteredCombat = false;
            AbstractDataBattle.isBattleDone = true;
            GameObject.Find("User Interface").transform.GetChild(3).gameObject.SetActive(true);

        }

        
        if(AbstractDataBattle.isUltiDone)
        {
            StartCoroutine(cooldownUniversal("Burst"));
            StartCoroutine(cooldownUniversal("Skill"));
            StartCoroutine(cooldownUniversal("Attack"));
        }
    }

    IEnumerator result(){
        yield return new WaitForSecondsRealtime(1f);
        AbstractDataBattle.MonsterAnimate.SetTrigger("Death");
    }

    public void ContinueTheGame() {
        AbstractPlayerData.checkpoint.LoadDataCheckpoint();
        LoadingData.sceneToLoad = "InGame";
        SceneManager.LoadScene("Loading");
    }


    void ResultToUI(){
        Text expgain = GameObject.Find("User Interface").transform.GetChild(2).gameObject.transform.GetChild(2).gameObject.GetComponent<Text>() as Text;
        GameObject drop = GameObject.Find("User Interface").transform.GetChild(2).gameObject.transform.GetChild(4).gameObject;

        string str = "";
        AbstractPlayerData.player.Status.ExperiencePoint += AbstractDataBattle.mob.base_exp;
        if (AbstractPlayerData.player.Status.ExperiencePoint >= AbstractPlayerData.player.Status.ExperienceCap) 
        {
            AbstractPlayerData.LevelUp();
            str = "You levelled up!";
        }
        else
        {
            str = (AbstractPlayerData.player.Status.ExperienceCap - AbstractPlayerData.player.Status.ExperiencePoint) + " again to next level";
        }
        expgain.text = "You got " + AbstractDataBattle.mob.base_exp + ", " + str;

        List<string> drop_item = new List<string>();
        ItemModel itemmodel = new ItemModel(textJsonItem);
        List<Item> items = itemmodel.ReaderData();
        for(int i = 0; i < AbstractDataBattle.mob.drop.Count;i++)
        {
            float rng = Random.Range(0f, 100f);
            if (rng <= AbstractDataBattle.mob.drop[i].chance)
            {
                foreach(Item item in items)
                {
                    if(item.ItemID == AbstractDataBattle.mob.drop[i].item_id)
                    {
                        drop_item.Add(item.Name);

                        for(int j = 0; j < AbstractPlayerData.player.Slots.Count;j++)
                        {
                            if(AbstractPlayerData.player.Slots[j].ItemID == AbstractDataBattle.mob.drop[i].item_id)
                            {
                                if (AbstractPlayerData.items[j].Type == "Consumable"){
                                    AbstractPlayerData.player.Slots[j].Quantity += 1;
                                }     
                                break;
                            }
                            if (j+1 == AbstractPlayerData.player.Slots.Count)
                            {
                                Slot slot = new Slot();
                                slot.ItemID = item.ItemID;
                                slot.IsUsed = false;
                                slot.Quantity = 1;
                                AbstractPlayerData.player.Slots.Add(slot);
                                AbstractPlayerData.items.Add(item);
                            }
                        }

                    }
                }
            }
        }

        if (drop_item.Count > 0)
        {
            drop.SetActive(true);

            GameObject child_drop = drop.transform.GetChild(0).gameObject;
            float y = 35f;
            for(int i = 0; i < drop_item.Count; i++)
            {
                GameObject grandchild = Instantiate(child_drop);
                grandchild.SetActive(true);
                grandchild.name = "[" + (i+1) + "] Item";
                grandchild.GetComponent<Text>().text = drop_item[i];
                grandchild.transform.parent = drop.transform;
                grandchild.GetComponent<RectTransform>().localPosition = new Vector3(0,y,0);
                grandchild.GetComponent<RectTransform>().sizeDelta = child_drop.GetComponent<RectTransform>().sizeDelta;
                grandchild.GetComponent<RectTransform>().localScale = new Vector3(1f,1f,1f);
                y -= 40f;
                // drop.transform.GetChild(i).gameObject.GetComponent<Text>().text = drop_item[i];
            }
        }
        else 
        {
            drop.SetActive(false);
        }

        for(int i = 0; i < AbstractPlayerData.mainQuest.Count; i++)
        {
            if (AbstractPlayerData.mainQuest[i].IsFinish == false && AbstractPlayerData.mainQuest[i].QuestLocation == location)
            {
                AbstractPlayerData.mainQuest[i].Fulfill += 1;
                if(AbstractPlayerData.mainQuest[i].Fulfill > AbstractPlayerData.mainQuest[i].Request)
                {
                    AbstractPlayerData.mainQuest[i].Fulfill = AbstractPlayerData.mainQuest[i].Request;
                }
                break;
            }
        }

        if(AbstractPlayerData.sideQuest.Count > 0 && AbstractPlayerData.GetCurrentSecretQuest() != null)
        {
            for(int i = 0; i < AbstractPlayerData.sideQuest.Count; i++)
            {
                if (AbstractPlayerData.sideQuest[i].IsFinish == false && AbstractPlayerData.sideQuest[i].QuestLocation == location && AbstractDataBattle.mob.type == AbstractPlayerData.sideQuest[i].MonsterType)
                {
                    AbstractPlayerData.sideQuest[i].Fulfill += 1;
                    if(AbstractPlayerData.sideQuest[i].Fulfill > AbstractPlayerData.sideQuest[i].Request)
                    {
                        AbstractPlayerData.sideQuest[i].Fulfill = AbstractPlayerData.sideQuest[i].Request;
                    }
                    break;
                }
            }
        }

    }

    void TouchToAttack(Touch touches){
        if(isReadyAttack == true)
        {
            Touch touch = touches;
            Vector3 pos = touch.position;
            if(touch.phase == TouchPhase.Began){
                Ray ray = Camera.main.ScreenPointToRay(pos);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit)) {
                    if (hit.collider.tag == AbstractDataBattle.monster.tag && AbstractDataBattle.isUltiDone) {
                        Attack(pos);
                    }
                }
            }
        }
    }

    public void Attack(Vector3 touchPosition){
        GameObject normalHitEffect = Instantiate(this.normalHitEffect, new Vector3(((touchPosition.x/GameObject.Find("User Interface").transform.position.x) - 1) + AbstractDataBattle.monster.transform.position.x, (touchPosition.y/GameObject.Find("User Interface").transform.position.y) + AbstractDataBattle.monster.transform.position.y, touchPosition.z+1.75f), Quaternion.Euler(0,0,0));
        normalHitEffect.name = "Attack";
        cooldowntimeAttack = (float) AbstractPlayerData.player.Status.AttackCooldown;
        isReadyAttack = false;
        DecrementHealthPointUI(normalHitEffect, AbstractPlayerData.player.Status.BaseAttack);
    }

    public void Skill(){
        GameObject skillEffect = Instantiate(this.skillEffect, new Vector3(0,0,0), Quaternion.Euler(0,0,0));
        skillEffect.name = "Skill";
        
        CooldownTimeSkill = (float) skill.Cooldown - (AbstractPlayerData.GetItemUsed() != null? (AbstractPlayerData.GetItemUsed().Nick == "Stone"? ((float)skill.Cooldown * 0.5f) : 0f) : 0f );
        DecrementHealthPointUI(skillEffect, (AbstractPlayerData.player.Status.BaseAttack * skill.DamageMultiplier));

        skillButton.interactable = false;
        CooldownReadySkill = false;
        SkillCooldownPanel.SetActive(true);        
    }

    public void Burst(){
        if (CooldownReadyBurst == true && AbstractPlayerData.player.Status.ChargePoint >= this.burst.ChargePoint)
        {
            GameObject burstEffect = Instantiate(this.burstEffect, new Vector3(0,0,0), Quaternion.Euler(0,0,0));
            burstEffect.name = "Burst";
            
            CooldownTimeBurst = (float)burst.Cooldown - (AbstractPlayerData.GetItemUsed() != null? (AbstractPlayerData.GetItemUsed().Nick == "Stone"? ((float)burst.Cooldown * 0.5f) : 0f) : 0f);
            DecrementHealthPointUI(burstEffect, (AbstractPlayerData.player.Status.BaseAttack * burst.DamageMultiplier));
            
            //Code for decrement chargepoint
            AbstractPlayerData.player.Status.ChargePoint -= burst.ChargePoint;
            ObjectHeightChargeBurst = (float)(125f/this.burst.ChargePoint) * (float)((AbstractPlayerData.player.Status.ChargePoint - this.burst.ChargePoint <= 0)? AbstractPlayerData.player.Status.ChargePoint : this.burst.ChargePoint) ;
            ChargePointProgressBurst.rectTransform.localPosition = new Vector3(0,(-62.5f)+(ObjectHeightChargeBurst/2),0);
            //End of decrement chargepoint

            

            //For button cannot be interact
            CooldownReadyBurst = false;
            burstButton.interactable = false;
            BurstCooldownPanel.SetActive(true);
        } 
        else {
            Debug.Log("Charge Point kurang anj!");
        }
    }

    public void DecrementHealthPointUI(GameObject typeOfAttack, double damage){
        if(typeOfAttack.name != "Attack"){
            typeOfAttack.transform.position = new Vector3(AbstractDataBattle.monster.transform.position.x , AbstractDataBattle.monster.transform.position.y + 1.2f, AbstractDataBattle.monster.transform.position.z-1f);
        }

        

        float rng = Random.Range(0.1f,100f);
        string chancestate = DecideStateForMonster(rng);
        AbstractDataBattle.MonsterHitState(chancestate);
        if(chancestate != "Evade") 
        {
            if(typeOfAttack.name == "Skill")
                LifeStealPlayer(skill.LifeSteal);
            else if (typeOfAttack.name == "Burst")
                LifeStealPlayer(burst.LifeSteal);

            if(typeOfAttack.name != "Burst")
                RechargePoint(typeOfAttack.name == "Attack"? 1f : float.Parse(skill.ChargeMultiplier.ToString()));

            float damage_total = (float)damage;
            if (chancestate == "Block"){
                damage_total -= (((float)AbstractDataBattle.mob.block/100) * damage_total);
            }
            AbstractDataBattle.mob.hp -= damage_total;

            float dmg_scaling_bar = (float)damage_total * (250 / (float)AbstractDataBattle.mob.max_hp);
            object_width -= dmg_scaling_bar;

            AbstractDataBattle.mob.energy += AbstractDataBattle.mob.energy_regen;
            if (AbstractDataBattle.mob.energy > AbstractDataBattle.mob.energy_cap)
                AbstractDataBattle.mob.energy = AbstractDataBattle.mob.energy_cap;

            
            
            if(AbstractDataBattle.mob.hp<AbstractDataBattle.mob.max_hp) AbstractDataBattle.timerPlayerIdle = 0;
        }
        else{
            AbstractDataBattle.mob.energy += (AbstractDataBattle.mob.energy_regen/2);
            if (AbstractDataBattle.mob.energy > AbstractDataBattle.mob.energy_cap)
                AbstractDataBattle.mob.energy = AbstractDataBattle.mob.energy_cap;
        }
        width_monster_energy = (250f/(float)AbstractDataBattle.mob.energy_cap) * ((float)AbstractDataBattle.mob.energy);

        hp_bar_monster.rectTransform.sizeDelta = new Vector2(object_width, 25);
        hp_bar_monster.rectTransform.localPosition = new Vector3((-125) + object_width/2,0,0);

        energy_bar_monster.rectTransform.sizeDelta = new Vector2(width_monster_energy, 25);
        energy_bar_monster.rectTransform.localPosition = new Vector3((-125) + width_monster_energy/2,0,0);
    }

    void LifeStealPlayer(double lifesteal){
        AbstractPlayerData.player.Status.HealthPoint += (AbstractDataBattle.mob.hp * (lifesteal/100));
        if(AbstractPlayerData.player.Status.HealthPoint > AbstractPlayerData.player.Status.MaxHealthPoint)
            AbstractPlayerData.player.Status.HealthPoint = AbstractPlayerData.player.Status.MaxHealthPoint;
    }

    public string DecideStateForMonster(float rng){
        if (rng <= AbstractDataBattle.mob.evade_chance)
            return "Evade";
        else if (rng <= AbstractDataBattle.mob.block_chance)
            return "Block";
        else 
            return "TakePunch";
    }

    public void RechargePoint(float chargemultiplier){
        if(AbstractPlayerData.player.Status.ChargePoint + AbstractPlayerData.player.Status.BaseRechargePoint * chargemultiplier > AbstractPlayerData.player.Status.MaxChargePoint)
            AbstractPlayerData.player.Status.ChargePoint = AbstractPlayerData.player.Status.MaxChargePoint;
        else
            AbstractPlayerData.player.Status.ChargePoint += AbstractPlayerData.player.Status.BaseRechargePoint * chargemultiplier;
        ObjectHeightChargeBurst = (float)(125f/this.burst.ChargePoint) * (float)((AbstractPlayerData.player.Status.ChargePoint - this.burst.ChargePoint <= 0)? AbstractPlayerData.player.Status.ChargePoint : this.burst.ChargePoint) ;
        ChargePointProgressBurst.rectTransform.localPosition = new Vector3(0,(-62.5f)+(ObjectHeightChargeBurst/2),0);
    }

    public void Reveal(){
        statusMonsterPanel.SetActive(!statusMonsterPanel.activeSelf);
    }

    public void Flee() {       
        float rng = Random.Range(0f, 100f);
        if(rng <= chanceFlee && maxFlee > 0)
        {
            StartCoroutine(FleeResult("Success", "Kamu berhasil kabur!"));
        }
        else if(maxFlee > 0){
            StartCoroutine(FleeResult("Fail","Kamu gagal kabur! sisa " + maxFlee + " lagi.."));
        }
        else {
            StartCoroutine(FleeResult("Run out","Kesempatan kamu habis!"));
        }
    }

    IEnumerator FleeResult(string state, string str)
    {
        GameObject fleeobj = GameObject.Find("ItemFlee");
        Button fleeobj_button = fleeobj.GetComponent<Button>() as Button;

        GameObject parent = GameObject.Find("User Interface");
        GameObject child = parent.transform.GetChild(4).gameObject;

        child.SetActive(true);
        fleeobj_button.interactable = false;
        if (state == "Success")
        {
            child.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Kamu berhasil kabur!";
            
            yield return new WaitForSecondsRealtime(2f);

            LoadingData.sceneToLoad = "InGame";
            SceneManager.LoadScene("Loading");
        }
        else if (state == "Fail")
        {
            maxFlee -= 1;
            child.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Kamu gagal kabur! sisa " + maxFlee + " lagi..";
            yield return new WaitForSecondsRealtime(3f);

            child.SetActive(false);
            fleeobj_button.interactable = true;

        }
        else{
            child.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Kamu gagal kabur! sisa " + maxFlee + " lagi..";
            yield return new WaitForSecondsRealtime(3f);

            child.SetActive(false);
            fleeobj_button.interactable = true;
        }

        yield return null;
    }


    IEnumerator cooldownUniversal(string type){
        if (type == "Burst" && CooldownReadyBurst == false){
            _timeBurst = _timeBurst + 1f * Time.deltaTime;
            if (_timeBurst >= _timeDelayedBurst){
                _timeBurst = 0f;
                cooldownBurst();
            }
            burstTimerText.text = (int)CooldownTimeBurst + "sec";
        }
        if (type == "Skill" && CooldownReadySkill == false){
            _timeSkill = _timeSkill + 1f * Time.deltaTime;
            if (_timeSkill >= _timeDelayedSkill){
                _timeSkill = 0f;
                cooldownSkill();
            }
            skillTimerText.text = (int)CooldownTimeSkill + "sec";
        }
        if(type == "Attack" && isReadyAttack == false)
        {
            _timeAttack = _timeAttack + 1f * Time.deltaTime;
            if(_timeAttack >= _timeDelayedAttack)
            {
                _timeAttack = 0f;
                cooldownAttack();
            }
        }
        yield return null;
    }

    void cooldownAttack()
    {
        cooldowntimeAttack -= 0.1f;
        if(cooldowntimeAttack < 0f)
        {
            isReadyAttack = true;
        }
    }

    void cooldownSkill(){
        CooldownTimeSkill -= 0.1f;
        if(CooldownTimeSkill < 0f)
        {
            CooldownReadySkill = true;
            skillButton.interactable = true;
            SkillCooldownPanel.SetActive(false);
        }
    }

    void cooldownBurst(){
        CooldownTimeBurst -= 0.1f;
        if(CooldownTimeBurst < 0f)
        {
            CooldownReadyBurst = true;
            burstButton.interactable = true;
            BurstCooldownPanel.SetActive(false);
        }
    }

    
    void LoadItemUsed()
    {
        Item item = AbstractPlayerData.GetItemUsed();
        int qtyItem = AbstractPlayerData.GetQtyItemUsed();

        GameObject parent = GameObject.Find("Action");
        GameObject child = parent.transform.GetChild(2).gameObject;
        Button child_button = child.GetComponent<Button>() as Button;
        
        child.SetActive(false);
        child_button.onClick.RemoveAllListeners();
        if (item != null && qtyItem > 0 && item.Type != "Artifact" && item.Nick != "Flee")
        {
            child.SetActive(true);
            Image child_icon = child.transform.GetChild(2).gameObject.GetComponent<Image>() as Image;
            Text child_name = child.transform.GetChild(3).gameObject.GetComponent<Text>() as Text;
            GameObject child_itemLeft = child.transform.GetChild(4).gameObject;

            child_icon.sprite = AbstractPlayerData.itemicon;
            child_name.text = item.Name;
            child_itemLeft.SetActive(false);
            if (item.Type == "Consumable")
            {
                child_itemLeft.SetActive(true);
                child_itemLeft.transform.GetChild(0).gameObject.GetComponent<Text>().text = qtyItem.ToString(); 

                child_button.onClick.AddListener(() =>
                    {
                        for(int i=0; i < AbstractPlayerData.player.Slots.Count;i++)
                        {
                            if(AbstractPlayerData.player.Slots[i].ItemID == item.ItemID)
                            {
                                AbstractPlayerData.player.Status.HealthPoint += (item.StatusModifier.hp * AbstractPlayerData.player.Status.MaxHealthPoint);
                                if (AbstractPlayerData.player.Status.HealthPoint > AbstractPlayerData.player.Status.MaxHealthPoint)
                                {
                                    AbstractPlayerData.player.Status.HealthPoint = AbstractPlayerData.player.Status.MaxHealthPoint;
                                }
                                AbstractPlayerData.player.Slots[i].Quantity -= 1;
                            }
                        }
                    }
                );

            } 
            else if(item.Nick == "Reveal")
            {
                child_button.onClick.AddListener(() =>
                    {
                        Reveal();
                    }
                );
            }

        }

    }
}