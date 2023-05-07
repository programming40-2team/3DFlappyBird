using Data;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


//인터페이스 ILoader => 반드시 Dictonary를 반환하는 MakeDict()을 만들도록 강제
public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}




public class DataManager : MonoBehaviour
{
    //싱글톤 형식으로 구현된 데이터 매니저 
    public static DataManager Instance = null;
    private string filepath;
    public PlayerData nowPlayer;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    //전처리기 입니다. 유니티 에디터 혹은 스탠드 얼론 
    //일 때 쓰는 파일 경로와 모바일 환경에서 사용하는 경로를 분리했습니다./
    private void Start()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        filepath = Path.Combine(Application.dataPath, "SaveData.json");
#else
    filepath = Path.Combine(Application.persistentDataPath, "SaveData.json");
#endif

        LoadData();
        nowPlayer = new PlayerData();
    }


    //새로운 플레이어 생성 

    //플레이어 들의 ScoreData를 가지고 있는 Dictionary
    //1개의 Dummy 데이터를 가지고 있음... 없으면 실행이 안됩니다 죄송합니다.
    public Dictionary<string, Data.PlayerData> playerScoreDataDict { get; private set; } = new Dictionary<string, Data.PlayerData>();
    //저장된 정보를 가지오는 함수 입니다. => 게임 실행 시 이전에 플레이된  저장된 데이터를
    //가져온 후 스코어 계산 예정입니다.
    //수정되었습니다. 추가된 부분은 그..Chatgpt가 모바일에서 하기 위해서는 저런게 있으면 좋다고 하네요..

    public void LoadData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, filepath);

        string json;

        if (filePath.Contains("://")) // 모바일에서는 파일 경로 앞에 "file://" 이 붙어야 합니다.
        {
            UnityWebRequest www = UnityWebRequest.Get(filePath);
            www.SendWebRequest();
            while (!www.isDone) { }

            json = www.downloadHandler.text;
        }
        else
        {
            json = File.ReadAllText(filePath);
        }
        playerScoreDataDict = LoadJson<Data.PlayerDataLoader, string, Data.PlayerData>(filepath).MakeDict();
        // Json 데이터를 파싱하여 필요한 데이터를 불러옵니다.
        // ...
    }

    //데이터를 추가하는 과정입니다. 
    // 2번 사용될 예정입니다. ( 1. 처음 게임 실행할 경우, 2. 게임이 종료될 경우 
    //조금 비효율적인 부분이 있긴하지만 2번 호출되는 점 참고 부탁드립니다.
    public void addData(string name, int score)
    {
        nowPlayer = new PlayerData(); // 이전에 사용된 캐릭터 정보 초기화

        nowPlayer.name = name;
        nowPlayer.score = score;

        if (playerScoreDataDict.ContainsKey(name))
        {
            playerScoreDataDict[name] = nowPlayer;
        }
        else
        {
            playerScoreDataDict.Add(name, nowPlayer);
        }
    }
    //첫 시작이 아니기에, Dictonary에서 해당 값을 찾아서 score 부분만 변경해줄 수 있도록 하였습니다. ==> 캐릭터가 죽을 경우에는 
    //addData 가 아니라 개량된 버전인 renewScroe 를 호출합니다.
    public void afterGameOverRenewData(int totalscore)
    {
        addData(nowPlayer.name, totalscore);
        SavePlayerData();
    }

    //데이터를 저장하는 과정입니다..
    //우선 죄송합니다... Json 저장이 쉽지 않네요 .. 어쩔 수 없이 형식 자체를 가져와서 해당 형식의 이름을 추가해 주도록 설정하였습니다..
    //이 함수가 호출되는 경우는 오로지 게임이 종료된 후입니다. 
    public void SavePlayerData()
    {
        string json = "{\r\n  \"playerInformationList\": [\r\n";
        foreach (var pair in playerScoreDataDict)
        {
            string playerJson = JsonFormat(pair.Key);
            json += playerJson + ",\r\n";
        }
        if (playerScoreDataDict.Count > 0)
        {
            json = json.TrimEnd(',', '\r', '\n');  // 마지막 쉼표 제거
        }
        json += "\r\n  ]\r\n}";

        // JSON 문자열을 파일에 저장
        File.WriteAllText(filepath, json);

        Debug.Log("Player data saved.");
    }


    public void clearData()
    {
        playerScoreDataDict.Clear();

        string format = "{\"playerInformationList\":[" +
    "{\"name\":\"초급 컴퓨터\",\"score\":5}," +
    "{\"name\":\"중급 컴퓨터\",\"score\":10}," +
    "{\"name\":\"고급 컴퓨터\",\"score\":20}," +
    "{\"name\":\"알파고\",\"score\":500}" +
    "]}";
        File.WriteAllText(filepath, format);

        LoadData();

    }

    private string JsonFormat(string key)
    {
        PlayerData playerData = playerScoreDataDict[key];
        string str =
            $"{{\r\n  \"name\": \"{playerData.name}\",\r\n  \"score\": {playerData.score}\r\n}}";
        return str;
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        string data = File.ReadAllText(path);

        return JsonUtility.FromJson<Loader>(data);
    }

}
