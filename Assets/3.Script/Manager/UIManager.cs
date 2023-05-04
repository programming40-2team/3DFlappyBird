using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region 싱글톤
    //UIManager 입니다. 5.3에 교수님께서 수업하신 좀비 서바이버 UIManager와 동일하게 구현했습니다.
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
    //점수 점수를 획득할 경우 addScore 메서드를 호출 시 자동 갱신 
    private int playerScore = 0;
    //점수 추가하는 클래스 점수를 추가하면 자동으로 UI 갱신
    public void addScore(int score)
    {
        playerScore += score;
        update_Score_Text(playerScore);
    }
    //라이프를 추가하는 함수 자동으로 UI 갱신

    [SerializeField]
    private TextMeshProUGUI Score_Text;

    [SerializeField]
    private GameObject gameResultUI;

    [SerializeField]
    private GameObject Ranking;

    [SerializeField]
    private Image[] images;




    public void isPlayerLifeIncrease(bool isAdd)
    {
        int currentLife = 0;
        foreach (Image image in images)
        {
            if (image.enabled.Equals(true))
            {
                currentLife++;
            }
        }
        if (currentLife.Equals(0)) return;

        if (isAdd)
        {
            if (currentLife.Equals(3)) return;
            for (int i = 0; i < images.Length; i++)
            {
                if (images[i].enabled.Equals(false))
                {
                    StartCoroutine(nameof(fadeEffect), images[i]);
                    break;
                }
            }

        }
        else
        {
            for (int i = images.Length - 1; i >= 0; i--)
            {
                if (images[i].enabled.Equals(true))
                {
                    StartCoroutine(nameof(fadeEffect), images[i]);
                    break;
                }
            }
        }
    }


    //ScoreText를 갱신하는 함수 -> 점수를 더해질 경우 자동갱신
    private void update_Score_Text(int newScore)
    {
        Score_Text.text = $"Score : {newScore}";
    }

    //게임 오버 시 나타나는 함수 게임 종료 시 '한번만' 호출!
    public void gameOver()
    {
        //게임 종료 시 점수를 갱신하고, 게임 멈춤 및 스코어 창 비활성 + 게임 결과창 활성
        DataManager.Instance.afterGameOverRenewData(playerScore);
        Time.timeScale = 0;
        Score_Text.gameObject.SetActive(false);
        gameResultUI.gameObject.SetActive(true);

        setRankingUi();



    }

    public void setRankingUi()
    {

        //우선 순위 큐에 기존 랭킹들 하나씩 넣어주기 
        PriorityQueue<string> priorityQueue = new PriorityQueue<string>();
        foreach (var i in DataManager.Instance.playerScoreDataDict.Keys)
        {
            if (DataManager.Instance.playerScoreDataDict[i].score < 0) continue;
            priorityQueue.Enqueue(-DataManager.Instance.playerScoreDataDict[i].score, i);
        }

        //하드 코딩 들어간 점 양해 부탁드립니다.
        // 우아하게 할 수 있지만.. 규모가 작은 관계로 ^....^'';

        //나 + 순위권 유저들 랭킹 보여주기
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
        //나의 랭킹 보여주기
        //하드 코딩입니다. 이해하지 않으셔도 무관합니다.
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



    //게임 다시 시작할 경우 닉네임 부터 재설정 해야 합니다.
    //그래야 랭킹에 반영될 수 있습니다. 아직 연동 X 
    public void gameRestart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Intro");
    }
    enum RankUI
    {
        Icon_MedalSliver = 0,
        Text_NickName = 1,
        Text_Score = 2,

    }


    private IEnumerator fadeEffect(Image image)
    {
        float fadeSpeed = 0.5f;
        float alpha = Mathf.Round(image.color.a);
        if (alpha == 1)
        {
            //페이드 아웃? 점점 희려지는 기능
            while (alpha > 0)
            {
                alpha -= Time.deltaTime * fadeSpeed;
                Color Acolor = image.color;
                Acolor.a = alpha;
                image.color = Acolor;
                //꺼질 때 자연스럽게 꺼지게 하기 위해서 0.2 정도 남으면 바로 끄고 생성
                if (alpha < 0.2f)
                {
                    image.enabled = false;
                    image.transform.GetChild(0).GetComponent<Image>().enabled = !image.enabled;
                    alpha = 0;
                    image.color = Acolor;
                }
                yield return null;
            }

        }
        else
        {
            //페이드 인 점점 밝아지는 기능
            image.enabled = true;
            while (alpha < 1)
            {
                alpha += Time.deltaTime * fadeSpeed;
                Color Bcolor = image.color;
                Bcolor.a = alpha;
                image.color = Bcolor;

                //켜질 떄 자연스럽게 켜지게 하기 위해서
                if (alpha > 0.3f)
                {
                    image.transform.GetChild(0).GetComponent<Image>().enabled = !image.enabled;

                }

                yield return null;
            }



        }


    }
}
