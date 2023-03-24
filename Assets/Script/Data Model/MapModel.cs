using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class MapModel : MonoBehaviour
{
    private TextAsset textJsonMap;
    public MapModel(TextAsset mapData)
    {
        this.textJsonMap = mapData;
    }

    public class Map
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public string location { get; set; }
    }

    public class Root
    {
        public List<Map> Map { get; set; }
    }

    public List<Map> ReaderData()
    {
        Root datas = JsonConvert.DeserializeObject<Root>(textJsonMap.text);
        return datas.Map;
    }

}
