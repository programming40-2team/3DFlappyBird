
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserInformationCanvas : MonoBehaviour
{
    [Header("버튼이벤트에 추가해주기 위한 것")]
    [SerializeField]
    private GameObject userInformationInput;
    [SerializeField]
    private GameObject gameStartButton;

    [Header("플레이어 인풋 을 가져오기 위한 것")]
    //초기 스코어 및 플레이어의 입력을 받아서 플레이어 이름 설정
    //추후 데이터 저장 및 랭킹 시스템을위한 것
    //오락실 ABC 999 점 느낌입니다.
    private string playerInputName;
    [SerializeField]
    private TMP_InputField playerInputField;
    [SerializeField]
    private int initScore = 0;
    [SerializeField]
    private TextMeshProUGUI warningText;

    private void Start()
    {
        //첫 시작 시 인풋 필드가 나와 있는 것을 방지하기 위해 비활성화
        userInformationInput.SetActive(false);
        warningText.gameObject.SetActive(false);
        //GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
    }

    public void ShowInputUserInfomationActive()
    {
        //유저가 게임 시작을 누르면 인풋 필드가 나타나
        //유저 이름의 입력을 받고 시작할 수 있습니다.
        userInformationInput.SetActive(true);
        gameStartButton.SetActive(false);

    }
    public void CancelInputUserInfomationActive()
    {
        //유저가 입력을 포기하면, 인풋 필드가 사라지고,
        //다시 게임 시작 버튼이 나타납니다.
        userInformationInput.SetActive(false);
        gameStartButton.SetActive(true);

    }
    public void confirmUserInformation()
    {
        //유저가 정보를 입력할 경우 DataManager에 현재 플레이어를 저장하는 클래스 
        //PlayerData 의 새로운 객체 nowPlayer를 만든 후, 저장합니다.
        if (DataManager.Instance.playerScoreDataDict.ContainsKey(playerInputField.text) || playerInputField.text == string.Empty)
        {
            //이미 존재하는 아이디 혹은 공백을 입력할 경우 경고 메세지를 띄우는 Courtine을 실행합니다.
            StartCoroutine(nameof(warningtext_co));
        }
        else
        {
            DataManager.Instance.addData(playerInputField.text, initScore);

            //TODO 게임씬 로딩 
            SceneManager.LoadScene("Game");
        }

    }

    private IEnumerator warningtext_co()
    {
        warningText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        warningText.gameObject.SetActive(false);

    }
  

}
