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
    [SerializeField] Sprite[] backgroundImages;//背景图片数组

    [Header("-----Scoring Screen-----")]
    [SerializeField] Canvas scoringScreenCanvas;
    [SerializeField] Text playerScoreText;
    [SerializeField] Button mainMenuButton;
    [SerializeField] Transform ScoreContainer;//需要获取的是他的ui子对象，所以声明为Transform

    [Header("-----High Score Screen-----")]
    [SerializeField] Canvas newHighScoreScreenCanvas;
    [SerializeField] Button cancelButton;
    [SerializeField] Button submitButton;
    [SerializeField] InputField playerNameInputField;

    private void Start()
    {
        Cursor.visible = true;//显示光标
        Cursor.lockState = CursorLockMode.None;
        ShowRandomBackground();
        if (ScoreManager.Instance.HasNewHighScore)//是否获得新高分
        {
            ShowNewHighScoreScreen();
        }
        else
        {
            ShowScroingScreen();
        }
        GameManager.GameState = GameState.Scoring;//切换游戏状态到积分状态，不然会一直在gameover状态；

        ButtonPressedBehaviour.buttonFunctionTable.Add(mainMenuButton.gameObject.name, OnMainMenuButtonClicked);
        ButtonPressedBehaviour.buttonFunctionTable.Add(submitButton.gameObject.name, OnSubmitButtonClicked);
        ButtonPressedBehaviour.buttonFunctionTable.Add(cancelButton.gameObject.name, HideNewHighScoreScreen);
    }

    private void OnDisable()
    {
        ButtonPressedBehaviour.buttonFunctionTable.Clear();
    }
    void ShowRandomBackground()//随机显示背景
    {
        background.sprite = backgroundImages[Random.Range(0, backgroundImages.Length - 1)];
    }

    void ShowNewHighScoreScreen()//显示新高分画面
    {
        newHighScoreScreenCanvas.enabled = true;
        UIInput.Instance.SelectUI(cancelButton);


    }
    void HideNewHighScoreScreen()//隐藏新高分画面
    {
        newHighScoreScreenCanvas.enabled = false;
        ScoreManager.Instance.SavePlayerScoreData();
        ShowRandomBackground();
        ShowScroingScreen();
    }

    void ShowScroingScreen()//得分结算画面
    {
        scoringScreenCanvas.enabled = true;
        playerScoreText.text = ScoreManager.Instance.Score.ToString();//更新分数
        UIInput.Instance.SelectUI(mainMenuButton);//选中Button

        //更新高分排行榜
        UpdateHighScoreLeaderBoard();
    }
    void UpdateHighScoreLeaderBoard()
    {
        var playerScoreList = ScoreManager.Instance.LoadPlayerScoreData().list;//读取到的list

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
        if (!string.IsNullOrEmpty(playerNameInputField.text))//判断玩家输入是否为空，不为空则修改当前玩家名字
        {
            ScoreManager.Instance.SetPlayerName(playerNameInputField.text);
        }
        HideNewHighScoreScreen();
    }
}
