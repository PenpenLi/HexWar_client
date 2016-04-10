using System;
using System.IO;
using System.Collections.Generic;

public class Map
{
    public static Dictionary<int, MapData> mapDataDic;

    public static void Init()
    {
        mapDataDic = new Dictionary<int, MapData>();

        Dictionary<int, MapSDS> mapDic = StaticData.GetDic<MapSDS>();

        foreach (MapSDS mapSDS in mapDic.Values)
        {
            MapData mapData = new MapData();

            mapDataDic.Add(mapSDS.ID, mapData);

            using (FileStream fs = new FileStream(ConfigDictionary.Instance.map_path + mapSDS.name, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    mapData.GetData(br);
                }
            }
        }
    }
}

