using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ConversationModel : MonoBehaviour
{
    private TextAsset textJsonConversation;

    public ConversationModel(TextAsset textJsonConversation)
    {
        this.textJsonConversation = textJsonConversation;
    }

    public class Conversation
    {
        public List<Main> Main { get; set; }

        [JsonProperty("Ending Denial")]
        public List<EndingDenial> EndingDenial { get; set; }

        [JsonProperty("Information-Status")]
        public List<InformationStatus> InformationStatus { get; set; }

        [JsonProperty("Information-Talent")]
        public List<InformationTalent> InformationTalent { get; set; }

        [JsonProperty("Information-Bag")]
        public List<InformationBag> InformationBag { get; set; }

        [JsonProperty("Information-Map")]
        public List<InformationMap> InformationMap { get; set; }

        [JsonProperty("Information-Quest")]
        public List<InformationQuest> InformationQuest { get; set; }

        [JsonProperty("Information-Battle")]
        public List<InformationBattle> InformationBattle { get; set; }

        [JsonProperty("Ending True")]
        public List<EndingTrue> EndingTrue { get; set; }
    }

    public class EndingDenial
    {
        public string ID { get; set; }
        public string Part { get; set; }
        public string Sentence { get; set; }
        public string Type { get; set; }
        public List<object> Option { get; set; }
        public string Source { get; set; }
    }

    public class EndingTrue
    {
        public string ID { get; set; }
        public string Part { get; set; }
        public string Sentence { get; set; }
        public List<object> Option { get; set; }
        public string Source { get; set; }
    }

    public class InformationBag
    {
        public string ID { get; set; }
        public string Part { get; set; }
        public string Sentence { get; set; }
        public List<object> Option { get; set; }
        public string Source { get; set; }
    }

    public class InformationBattle
    {
        public string ID { get; set; }
        public string Part { get; set; }
        public string Sentence { get; set; }
        public List<object> Option { get; set; }
        public string Source { get; set; }
    }

    public class InformationMap
    {
        public string ID { get; set; }
        public string Part { get; set; }
        public string Sentence { get; set; }
        public List<object> Option { get; set; }
        public string Source { get; set; }
    }

    public class InformationQuest
    {
        public string ID { get; set; }
        public string Part { get; set; }
        public string Sentence { get; set; }
        public List<object> Option { get; set; }
        public string Source { get; set; }
    }

    public class InformationStatus
    {
        public string ID { get; set; }
        public string Part { get; set; }
        public string Sentence { get; set; }
        public string Type { get; set; }
        public List<object> Option { get; set; }
        public string Source { get; set; }
    }

    public class InformationTalent
    {
        public string ID { get; set; }
        public string Part { get; set; }
        public string Sentence { get; set; }
        public List<object> Option { get; set; }
        public string Source { get; set; }
    }

    public class Main
    {
        public string ID { get; set; }
        public string Part { get; set; }
        public string Sentence { get; set; }
        public string Type { get; set; }
        public List<Option> Option { get; set; }
        public string Source { get; set; }
    }

    public class Option
    {
        public string Response { get; set; }
        public string Type { get; set; }
        public bool IsEnding { get; set; }
        public string EndingType { get; set; }
    }

    public class Root
    {
        public Conversation Conversation { get; set; }
    }

    public Root ReaderData(){
        Root datas = JsonConvert.DeserializeObject<Root>(textJsonConversation.text);
        return datas;
    }
}
