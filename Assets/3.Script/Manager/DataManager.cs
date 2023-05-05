using Data;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


//�������̽� ILoader => �ݵ�� Dictonary�� ��ȯ�ϴ� MakeDict()�� ���鵵�� ����
public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}




public class DataManager : MonoBehaviour
{
    //�̱��� �������� ������ ������ �Ŵ��� 
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

    //��ó���� �Դϴ�. ����Ƽ ������ Ȥ�� ���ĵ� ��� 
    //�� �� ���� ���� ��ο� ����� ȯ�濡�� ����ϴ� ��θ� �и��߽��ϴ�./
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


    //���ο� �÷��̾� ���� 

    //�÷��̾� ���� ScoreData�� ������ �ִ� Dictionary
    //1���� Dummy �����͸� ������ ����... ������ ������ �ȵ˴ϴ� �˼��մϴ�.
    public Dictionary<string, Data.PlayerData> playerScoreDataDict { get; private set; } = new Dictionary<string, Data.PlayerData>();
    //����� ������ �������� �Լ� �Դϴ�. => ���� ���� �� ������ �÷��̵�  ����� �����͸�
    //������ �� ���ھ� ��� �����Դϴ�.
    //�����Ǿ����ϴ�. �߰��� �κ��� ��..Chatgpt�� ����Ͽ��� �ϱ� ���ؼ��� ������ ������ ���ٰ� �ϳ׿�..

    public void LoadData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, filepath);

        string json;

        if (filePath.Contains("://")) // ����Ͽ����� ���� ��� �տ� "file://" �� �پ�� �մϴ�.
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
        // Json �����͸� �Ľ��Ͽ� �ʿ��� �����͸� �ҷ��ɴϴ�.
        // ...
    }

    //�����͸� �߰��ϴ� �����Դϴ�. 
    // 2�� ���� �����Դϴ�. ( 1. ó�� ���� ������ ���, 2. ������ ����� ��� 
    //���� ��ȿ������ �κ��� �ֱ������� 2�� ȣ��Ǵ� �� ���� ��Ź�帳�ϴ�.
    public void addData(string name, int score)
    {
        nowPlayer = new PlayerData(); // ������ ���� ĳ���� ���� �ʱ�ȭ

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
    //ù ������ �ƴϱ⿡, Dictonary���� �ش� ���� ã�Ƽ� score �κи� �������� �� �ֵ��� �Ͽ����ϴ�. ==> ĳ���Ͱ� ���� ��쿡�� 
    //addData �� �ƴ϶� ������ ������ renewScroe �� ȣ���մϴ�.
    public void afterGameOverRenewData(int totalscore)
    {
        addData(nowPlayer.name, totalscore);
        SavePlayerData();
    }

    //�����͸� �����ϴ� �����Դϴ�..
    //�켱 �˼��մϴ�... Json ������ ���� �ʳ׿� .. ��¿ �� ���� ���� ��ü�� �����ͼ� �ش� ������ �̸��� �߰��� �ֵ��� �����Ͽ����ϴ�..
    //�� �Լ��� ȣ��Ǵ� ���� ������ ������ ����� ���Դϴ�. 
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
            json = json.TrimEnd(',', '\r', '\n');  // ������ ��ǥ ����
        }
        json += "\r\n  ]\r\n}";

        // JSON ���ڿ��� ���Ͽ� ����
        File.WriteAllText(filepath, json);

        Debug.Log("Player data saved.");
    }


    public void clearData()
    {
        playerScoreDataDict.Clear();

        string format = "{\"playerInformationList\":[" +
    "{\"name\":\"�ʱ� ��ǻ��\",\"score\":5}," +
    "{\"name\":\"�߱� ��ǻ��\",\"score\":10}," +
    "{\"name\":\"��� ��ǻ��\",\"score\":20}," +
    "{\"name\":\"���İ�\",\"score\":500}" +
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
