using System;
using System.Collections.Generic;

namespace Data
{
    //데이터 포멧 입니다. 
    //JSON 다룰주 모릅니다..ㅈㅅㅈㅅ

    [Serializable]
    public class PlayerData
    {
        public string name;
        public int score;
    }

    [Serializable]
    public class PlayerDataLoader : ILoader<string, PlayerData>
    {
        public List<PlayerData> playerInformationList = new List<PlayerData>();

        public Dictionary<string, PlayerData> MakeDict()
        {
            Dictionary<string, PlayerData> dict = new Dictionary<string, PlayerData>();
            foreach (PlayerData data in playerInformationList)
            {
                dict.Add(data.name, data);
            }

            return dict;
        }
    }


}
