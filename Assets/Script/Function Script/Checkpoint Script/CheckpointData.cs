using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerModel;
using static QuestModel;
using static ItemModel;
using UnityEditor;
using System.IO;

public class CheckpointData : MonoBehaviour
{
    public string serializedplayer {get; set;}
    public string serializedquest {get; set;}

    public CheckpointData() {

    }


    public void SaveDataCheckpoint(){
        string path = Application.persistentDataPath + "/DataCheckpointPlayer.txt";
        File.WriteAllText(path, string.Empty);
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(serializedplayer);
        writer.Close();

        string path2 = Application.persistentDataPath + "/DataCheckpointQuest.txt";
        File.WriteAllText(path2, string.Empty);
        StreamWriter writer2 = new StreamWriter(path2, true);
        writer2.WriteLine(serializedquest);
        writer2.Close();
    }

    public void LoadDataCheckpoint() {
        StreamReader readCheckpointPlayer = new StreamReader(Application.persistentDataPath + "/DataCheckpointPlayer.txt");
        string strPlayer = readCheckpointPlayer.ReadLine();
        readCheckpointPlayer.Close();

        StreamReader readCheckpointQuest = new StreamReader(Application.persistentDataPath + "/DataCheckpointQuest.txt");
        string strQuest = readCheckpointQuest.ReadLine();
        readCheckpointQuest.Close();


        PlayerModel playermodel = new PlayerModel(strPlayer);
        QuestModel questmodel = new QuestModel(strQuest);

        AbstractPlayerData.player = playermodel.ReaderDataByString();
        AbstractPlayerData.mainQuest.Clear();
        AbstractPlayerData.mainQuest = questmodel.ReaderDataByString();
        AbstractPlayerData.sideQuest.Clear();
        AbstractPlayerData.sideQuest = questmodel.ReaderDataByStringSQ();
        AbstractPlayerData.notification.Clear();
        AbstractPlayerData.items.Clear();
        AbstractPlayerData.skillicons.Clear();
        AbstractPlayerData.bursticons.Clear();
    }

    public void DeleteDataCheckpoint(){
        string PathPlayerData = Application.persistentDataPath + "/DataCheckpointPlayer.txt";
        string PathQuestData = Application.persistentDataPath + "/DataCheckpointQuest.txt";
        if(File.Exists(PathPlayerData) || File.Exists(PathQuestData))
        {
            File.Delete(PathPlayerData);
            File.Delete(PathQuestData);
        }
    }
}
