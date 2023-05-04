using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    #region �̱���
    //UIManager �Դϴ�. 5.3�� �����Բ��� �����Ͻ� ���� �����̹� UIManager�� �����ϰ� �����߽��ϴ�.
    public static UIManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    //���� ������ ȹ���� ��� addScore �޼��带 ȣ�� �� �ڵ� ���� 
    private int playerScore = 0;

    //���� �߰��ϴ� Ŭ���� ������ �߰��ϸ� �ڵ����� UI ����
    public void addScore(int score)
    {
        playerScore += score;
        update_Score_Text(playerScore);
    }


    [SerializeField]
    private TextMeshProUGUI Score_Text;

    [SerializeField]
    private GameObject gameResultUI;

    [SerializeField]
    private GameObject Ranking;



    //ScoreText�� �����ϴ� �Լ� -> ������ ������ ��� �ڵ�����
    private void update_Score_Text(int newScore)
    {
        Score_Text.text = $"Score : {newScore}";
    }

    //���� ���� �� ��Ÿ���� �Լ� ���� ���� �� '�ѹ���' ȣ��!
    public void gameOver()
    {
        //���� ���� �� ������ �����ϰ�, ���� ���� �� ���ھ� â ��Ȱ�� + ���� ���â Ȱ��
        DataManager.Instance.afterGameOverRenewData(playerScore);
        Time.timeScale = 0;
        Score_Text.gameObject.SetActive(false);
        gameResultUI.gameObject.SetActive(true);

        setRankingUi();



    }

    public void setRankingUi()
    {

        //�켱 ���� ť�� ���� ��ŷ�� �ϳ��� �־��ֱ� 
        PriorityQueue<string> priorityQueue = new PriorityQueue<string>();
        foreach (var i in DataManager.Instance.playerScoreDataDict.Keys)
        {
            if (DataManager.Instance.playerScoreDataDict[i].score < 0) continue;
            priorityQueue.Enqueue(-DataManager.Instance.playerScoreDataDict[i].score, i);
        }

        //�ϵ� �ڵ� �� �� ���� ��Ź�帳�ϴ�.
        // ����ϰ� �� �� ������.. �Ը� ���� ����� ^....^'';

        //�� + ������ ������ ��ŷ �����ֱ�
        int rank = 0;
        int myrank = 0;
        while (priorityQueue.Count() > 0)
        {
            rank++;
            string rankerKey = priorityQueue.Dequeue();
            if (rankerKey == DataManager.Instance.nowPlayer.name)
            {
                myrank = rank;

            }
            if (rank <= 3)
            {
                Ranking.transform.GetChild(rank).GetChild((int)RankUI.Text_NickName).GetComponent<TextMeshProUGUI>().text
                    = $"{rankerKey}";

                Ranking.transform.GetChild(rank).GetChild((int)RankUI.Text_Score).GetComponent<TextMeshProUGUI>().text
                    = $"{DataManager.Instance.playerScoreDataDict[rankerKey].score}";


            }
            else if (rank <= 5)
            {
                Ranking.transform.GetChild(rank).GetChild((int)RankUI.Text_NickName - 1).GetComponent<TextMeshProUGUI>().text
                    = $"{rankerKey}";

                Ranking.transform.GetChild(rank).GetChild((int)RankUI.Text_Score - 1).GetComponent<TextMeshProUGUI>().text
                    = $"{DataManager.Instance.playerScoreDataDict[rankerKey].score}";

            }

            else
            {
                if (myrank != 0) break;

            }  

        }
        //���� ��ŷ �����ֱ�
        //�ϵ� �ڵ��Դϴ�. �������� �����ŵ� �����մϴ�.
        Ranking.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{DataManager.Instance.nowPlayer.name}";
        Ranking.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{DataManager.Instance.nowPlayer.score}";
        Ranking.transform.GetChild(0).GetChild(6).GetComponent<TextMeshProUGUI>().text = $"{myrank}";
        switch (myrank)
        {
            case 1:
                Ranking.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
                Ranking.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
                Ranking.transform.GetChild(0).GetChild(5).gameObject.SetActive(false);
                break;
            case 2:
                Ranking.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
                Ranking.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
                Ranking.transform.GetChild(0).GetChild(5).gameObject.SetActive(false);
                break;
            case 3:
                Ranking.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
                Ranking.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
                Ranking.transform.GetChild(0).GetChild(5).gameObject.SetActive(true);
                break;
            default:
                Ranking.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
                Ranking.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
                Ranking.transform.GetChild(0).GetChild(5).gameObject.SetActive(false);
                break;
        }


    }



    //���� �ٽ� ������ ��� �г��� ���� �缳�� �ؾ� �մϴ�.
    //�׷��� ��ŷ�� �ݿ��� �� �ֽ��ϴ�. ���� ���� X 
    public void gameRestart()
    {
        SceneManager.LoadScene("Intro");

    }
    enum RankUI
    {
        Icon_MedalSliver=0,
        Text_NickName=1,
        Text_Score=2,

    }

}
