using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class ItemModel : MonoBehaviour
{
    private TextAsset textJsonItem;

    public ItemModel(TextAsset textJsonItem)
    {
        this.textJsonItem = textJsonItem;
    }

    public class Item
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public string Nick { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public StatusModifier StatusModifier { get; set; }
        public string Source { get; set; }
    }

    public class Root
    {
        public List<Item> Items { get; set; }
    }

    public class StatusModifier
    {
        public double hp { get; set; }
        public double stamina { get; set; }
        public double exp { get; set; }
        public double attack { get; set; }
    }

    public List<Item> ReaderData(){
        Root datas = JsonConvert.DeserializeObject<Root>(textJsonItem.text);
        return datas.Items;
    }
}
