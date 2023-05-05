
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInformationCanvas : MonoBehaviour
{
    [Header("��ư�̺�Ʈ�� �߰����ֱ� ���� ��")]
    [SerializeField]
    private GameObject userInformationInput;
    [SerializeField]
    private GameObject gameStartButton;

    [Header("�÷��̾� ��ǲ �� �������� ���� ��")]
    //�ʱ� ���ھ� �� �÷��̾��� �Է��� �޾Ƽ� �÷��̾� �̸� ����
    //���� ������ ���� �� ��ŷ �ý��������� ��
    //������ ABC 999 �� �����Դϴ�.
    private string playerInputName;
    [SerializeField]
    private TMP_InputField playerInputField;
    [SerializeField]
    private int initScore = 0;
    [SerializeField]
    private TextMeshProUGUI warningText;

    private void Start()
    {
        //ù ���� �� ��ǲ �ʵ尡 ���� �ִ� ���� �����ϱ� ���� ��Ȱ��ȭ
        userInformationInput.SetActive(false);
        warningText.gameObject.SetActive(false);
        //GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
    }

    public void ShowInputUserInfomationActive()
    {
        //������ ���� ������ ������ ��ǲ �ʵ尡 ��Ÿ��
        //���� �̸��� �Է��� �ް� ������ �� �ֽ��ϴ�.
        userInformationInput.SetActive(true);
        gameStartButton.SetActive(false);

    }
    public void CancelInputUserInfomationActive()
    {
        //������ �Է��� �����ϸ�, ��ǲ �ʵ尡 �������,
        //�ٽ� ���� ���� ��ư�� ��Ÿ���ϴ�.
        userInformationInput.SetActive(false);
        gameStartButton.SetActive(true);

    }
    public void confirmUserInformation()
    {
        //������ ������ �Է��� ��� DataManager�� ���� �÷��̾ �����ϴ� Ŭ���� 
        //PlayerData �� ���ο� ��ü nowPlayer�� ���� ��, �����մϴ�.
        if (DataManager.Instance.playerScoreDataDict.ContainsKey(playerInputField.text) || playerInputField.text == string.Empty)
        {
            //�̹� �����ϴ� ���̵� Ȥ�� ������ �Է��� ��� ��� �޼����� ���� Courtine�� �����մϴ�.
            StartCoroutine(nameof(warningtext_co));
        }
        else
        {
            DataManager.Instance.addData(playerInputField.text, initScore);

            //TODO ���Ӿ� �ε� 
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
