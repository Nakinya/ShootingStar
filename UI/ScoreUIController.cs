using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>
public class ScoreUIController : MonoBehaviour
{
    [Header("-----Background-----")]
    [SerializeField] Image background;
    [SerializeField] Sprite[] backgroundImages;//����ͼƬ����

    [Header("-----Scoring Screen-----")]
    [SerializeField] Canvas scoringScreenCanvas;
    [SerializeField] Text playerScoreText;
    [SerializeField] Button mainMenuButton;
    [SerializeField] Transform ScoreContainer;//��Ҫ��ȡ��������ui�Ӷ�����������ΪTransform

    [Header("-----High Score Screen-----")]
    [SerializeField] Canvas newHighScoreScreenCanvas;
    [SerializeField] Button cancelButton;
    [SerializeField] Button submitButton;
    [SerializeField] InputField playerNameInputField;

    private void Start()
    {
        Cursor.visible = true;//��ʾ���
        Cursor.lockState = CursorLockMode.None;
        ShowRandomBackground();
        if (ScoreManager.Instance.HasNewHighScore)//�Ƿ����¸߷�
        {
            ShowNewHighScoreScreen();
        }
        else
        {
            ShowScroingScreen();
        }
        GameManager.GameState = GameState.Scoring;//�л���Ϸ״̬������״̬����Ȼ��һֱ��gameover״̬��

        ButtonPressedBehaviour.buttonFunctionTable.Add(mainMenuButton.gameObject.name, OnMainMenuButtonClicked);
        ButtonPressedBehaviour.buttonFunctionTable.Add(submitButton.gameObject.name, OnSubmitButtonClicked);
        ButtonPressedBehaviour.buttonFunctionTable.Add(cancelButton.gameObject.name, HideNewHighScoreScreen);
    }

    private void OnDisable()
    {
        ButtonPressedBehaviour.buttonFunctionTable.Clear();
    }
    void ShowRandomBackground()//�����ʾ����
    {
        background.sprite = backgroundImages[Random.Range(0, backgroundImages.Length - 1)];
    }

    void ShowNewHighScoreScreen()//��ʾ�¸߷ֻ���
    {
        newHighScoreScreenCanvas.enabled = true;
        UIInput.Instance.SelectUI(cancelButton);


    }
    void HideNewHighScoreScreen()//�����¸߷ֻ���
    {
        newHighScoreScreenCanvas.enabled = false;
        ScoreManager.Instance.SavePlayerScoreData();
        ShowRandomBackground();
        ShowScroingScreen();
    }

    void ShowScroingScreen()//�÷ֽ��㻭��
    {
        scoringScreenCanvas.enabled = true;
        playerScoreText.text = ScoreManager.Instance.Score.ToString();//���·���
        UIInput.Instance.SelectUI(mainMenuButton);//ѡ��Button

        //���¸߷����а�
        UpdateHighScoreLeaderBoard();
    }
    void UpdateHighScoreLeaderBoard()
    {
        var playerScoreList = ScoreManager.Instance.LoadPlayerScoreData().list;//��ȡ����list

        for (int i = 0; i < ScoreContainer.childCount; i++)
        {
            var child = ScoreContainer.GetChild(i);
            child.Find("Rank").GetComponent<Text>().text = (i + 1).ToString();
            child.Find("Score").GetComponent<Text>().text = playerScoreList[i].score.ToString();
            child.Find("Name").GetComponent<Text>().text = playerScoreList[i].playerName.ToString();
        }
    }

    void OnMainMenuButtonClicked()
    {
        scoringScreenCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }

    void OnSubmitButtonClicked()
    {
        if (!string.IsNullOrEmpty(playerNameInputField.text))//�ж���������Ƿ�Ϊ�գ���Ϊ�����޸ĵ�ǰ�������
        {
            ScoreManager.Instance.SetPlayerName(playerNameInputField.text);
        }
        HideNewHighScoreScreen();
    }
}
