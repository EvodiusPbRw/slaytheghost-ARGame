using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using static PlayerModel;

public class QuestModel : MonoBehaviour
{
    private TextAsset textJsonQuest;
    private string strQuest;

    public QuestModel(TextAsset textJsonQuest)
    {
        this.textJsonQuest = textJsonQuest;
    }

    public QuestModel(string strQuest)
    {
        this.strQuest = strQuest;
    }

    public class Data
    {
        public List<MainQuest> MainQuest { get; set; }
        public List<SideQuest> SideQuest { get; set; }
    }

    public class MainQuest
    {
        public int QuestID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string QuestLocation { get; set; }
        public int Fulfill { get; set; }
        public int Request { get; set; }
        public string MonsterType { get; set; }
        public Reward Reward { get; set; }
        public bool IsFinish { get; set; }
        public bool IsCheckpoint { get; set; }
    }

    public class Reward
    {
        public int exp { get; set; }
        public PlayerModel.Skill Skill { get; set; }
        public PlayerModel.Burst Burst { get; set; }
    }

    public class Root
    {
        public Data Data { get; set; }
    }

    public class SideQuest
    {
        public int QuestID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string QuestLocation { get; set; }
        public int Fulfill { get; set; }
        public int Request { get; set; }
        public string MonsterType { get; set; }
        public Reward Reward { get; set; }
        public bool IsFinish { get; set; }
        public bool IsCheckpoint { get; set; }
    }

    public List<MainQuest> ReaderData(){
        Root datas = JsonConvert.DeserializeObject<Root>(textJsonQuest.text);
        return datas.Data.MainQuest;
    }

    public List<SideQuest> ReaderDataSQ(){
        Root datas = JsonConvert.DeserializeObject<Root>(textJsonQuest.text);
        return datas.Data.SideQuest;
    }

    public List<MainQuest> ReaderDataByString()
    {
        Root datas = JsonConvert.DeserializeObject<Root>(strQuest);
        return datas.Data.MainQuest;
    }


    public List<SideQuest> ReaderDataByStringSQ(){
        Root datas = JsonConvert.DeserializeObject<Root>(strQuest);
        return datas.Data.SideQuest;
    }

    public Root ReaderDataRoot(){
        Root datas = JsonConvert.DeserializeObject<Root>(textJsonQuest.text);
        return datas;
    }

    public Root GetRoot(){
        Root datas = new Root();
        Data data = new Data();
        data.MainQuest = AbstractPlayerData.mainQuest;
        data.SideQuest = AbstractPlayerData.sideQuest;
        datas.Data = data;
        return datas;
    }
}
