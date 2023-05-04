using Data;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


//�������̽� ILoader => �ݵ�� Dictonary�� ��ȯ�ϴ� MakeDict()�� ���鵵�� ����
public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}




public class DataManager : MonoBehaviour
{
    //�̱��� �������� ������ ������ �Ŵ��� 
    public static DataManager Instance = null;
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
        LoadData();

    }



    //���ο� �÷��̾� ���� 
    public PlayerData nowPlayer = new PlayerData();
    //�÷��̾� ���� ScoreData�� ������ �ִ� Dictionary
    //1���� Dummy �����͸� ������ ����... ������ ������ �ȵ˴ϴ� �˼��մϴ�.
    public Dictionary<string, Data.PlayerData> playerScoreDataDict { get; private set; } = new Dictionary<string, Data.PlayerData>();
    //��δ� ������ loadpath�� ��� ����Ƽ �Լ� Resource�� ����ϱ� ���� �ٸ��� �����߽��ϴ�...
    //��������....�ϴ� �ּ��Դϴ�.
    private string loadpath = $"Data/SaveData";
    private string savepath = $"Assets/Resources/Data/SaveData.Json";


    //����� ������ �������� �Լ� �Դϴ�. => ���� ���� �� ������ �÷��̵�  ����� �����͸�
    //������ �� ���ھ� ��� �����Դϴ�.
    public void LoadData()
    {
        playerScoreDataDict = LoadJson<Data.PlayerDataLoader, string, Data.PlayerData>(loadpath).MakeDict();
    }


    //�����͸� �߰��ϴ� �����Դϴ�. 
    // 2�� ���� �����Դϴ�. ( 1. ó�� ���� ������ ���, 2. ������ ����� ��� 
    //���� ��ȿ������ �κ��� �ֱ������� 2�� ȣ��Ǵ� �� ���� ��Ź�帳�ϴ�.
    public void addData(string name, int score)
    {
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
        File.WriteAllText(savepath, json);

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
        File.WriteAllText(savepath, format);

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
        TextAsset textAsset = Resources.Load<TextAsset>(path);

        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

}
