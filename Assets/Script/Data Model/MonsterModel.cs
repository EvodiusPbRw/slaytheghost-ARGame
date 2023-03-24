using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class MonsterModel : MonoBehaviour
{
    private TextAsset textJsonMonster;

    public MonsterModel(TextAsset textJsonMonster)
    {
        this.textJsonMonster = textJsonMonster;
    }

    public class Coordinate
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Data
    {
        public List<Monster> monster { get; set; }
    }

    public class Drop
    {
        public int item_id { get; set; }
        public int chance { get; set; }
    }

    public class Monster
    {
        public int id { get; set; }
        public string cd_name { get; set; }
        public string desc { get; set; }
        public string type { get; set; }
        public int level { get; set; }
        public double max_hp { get; set; }
        public double attack { get; set; }
        public double ultimate { get; set; }
        public double hp { get; set; }
        public double energy { get; set; }
        public double energy_cap { get; set; }
        public double energy_regen { get; set; }
        public double evade_chance { get; set; }
        public double block { get; set; }
        public double block_chance { get; set; }
        public int base_exp { get; set; }
        public List<Drop> drop { get; set; }
        public string location { get; set; }
        public Coordinate coordinate { get; set; }
        public string source { get; set; }
    }

    public class Root
    {
        public Data Data { get; set; }
    }

    public List<Monster> ReaderData(){
        Root datas = JsonConvert.DeserializeObject<Root>(textJsonMonster.text);
        return datas.Data.monster;
    }
}
