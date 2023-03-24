using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class PlayerModel : MonoBehaviour
{
    private TextAsset textJsonPlayer;
    private string strPlayer;

    public PlayerModel(TextAsset textJsonPlayer){
        this.textJsonPlayer = textJsonPlayer;
    }

    public PlayerModel(string strPlayer){
        this.strPlayer = strPlayer;
    }

    public class Burst
    {
        public string Name { get; set; }
        public double DamageMultiplier { get; set; }
        public string Description { get; set; }
        public string ElementType { get; set; }
        public int ChargePoint { get; set; }
        public double Cooldown { get; set; }
        public double LifeSteal { get; set; }
        public bool IsUsed { get; set; }
        public string SourceIcon { get; set;}
        public string SourceEffect { get; set;}
    }

    public class Player
    {
        public string Name { get; set; }
        public string Job { get; set; }
        public string Gender { get; set; }
        public string CharacterSource { get; set; }
        public int Level { get; set; }
        public Status Status { get; set; }
        public List<Slot> Slots { get; set; }
        public int CurrentCheckpoint { get; set; }
        public Talent Talent { get; set; }
    }

    public class Root
    {
        public Player Player { get; set; }
    }

    public class Skill
    {
        public string Name { get; set; }
        public double DamageMultiplier { get; set; }
        public string Description { get; set; }
        public string ElementType { get; set; }
        public double ChargeMultiplier { get; set; }
        public double Cooldown { get; set; }
        public double LifeSteal { get; set; }
        public bool IsUsed { get; set; }
        public string SourceIcon { get; set;}
        public string SourceEffect { get; set;}
    }

    public class Slot
    {
        public int ItemID { get; set; }
        public bool IsUsed { get; set; }
        public int Quantity { get; set; }
    }

    public class Status
    {
        public double HealthPoint { get; set; }
        public double MaxHealthPoint { get; set; }
        public double ChargePoint { get; set; }
        public double MaxChargePoint { get; set; }
        public double BaseRechargePoint { get; set; }
        public int ExperiencePoint { get; set; }
        public int ExperienceCap { get; set; }
        public double BaseAttack { get; set; }
        public double BaseDefense { get; set; }
        public double AttackCooldown { get; set; }
    }

    public class Talent
    {
        public List<Skill> Skill { get; set; }
        public List<Burst> Burst { get; set; }
    }

    public Player ReaderData(){
        Root datas = JsonConvert.DeserializeObject<Root>(textJsonPlayer.text);
        return datas.Player;
    }

    public Player ReaderDataByString() 
    {
        Root datas = JsonConvert.DeserializeObject<Root>(strPlayer);
        return datas.Player;
    }

    public Root ReaderDataRoot(){
        Root datas = JsonConvert.DeserializeObject<Root>(textJsonPlayer.text);
        return datas;
    }

    public Root GetRoot()
    {
        
        Root datas = new Root();
        datas.Player = AbstractPlayerData.player;
        return datas;
    }
}
