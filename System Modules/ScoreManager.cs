using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class ScoreManager : PersistanSingleton<ScoreManager>
{
    #region Score Display
    public int Score => score;
    int score;//实际显示分数
    int currenScore;//玩家总分数
    [SerializeField] Vector3 scoreTextSale = new Vector3(1.2f, 1.2f, 1f);
    
    public void ResetScore()//重置分数
    {
        score = 0;
        ScoreDisplay.UpdateScoreText(score);
    }
    public void AddScore(int scorePoint)
    {
        currenScore += scorePoint;
        StartCoroutine(AddScoreCoroutine());
    }
    IEnumerator AddScoreCoroutine()//用于动态显示增加的分数
    {
        ScoreDisplay.ScaleScoreText(scoreTextSale);//缩放文本大小
        while (score < currenScore)
        {
            score++;
            ScoreDisplay.UpdateScoreText(score);//每一帧增加分数

            yield return null;
        }
        ScoreDisplay.ScaleScoreText(Vector3.one);
    }
    #endregion
    #region High Score System
    [System.Serializable]
    public class PlayerScore//玩家得分数据类
    {
        public int score;
        public string playerName;

        public PlayerScore(int score,string playerName)
        {
            this.score = score;
            this.playerName = playerName;
        }
    }
    [System.Serializable]
    public class PlayerScoreData//最终存储的json文本文件里的对象
    {
        public List<PlayerScore> list = new List<PlayerScore>();//用来保存玩家数据
    }

    readonly string saveFileName = "player_score.json";
    string playerName = "No Name";
    public bool HasNewHighScore => score > LoadPlayerScoreData().list[9].score;//判断分数是否超过存档中第10名的分数

    public void SetPlayerName(string newName)
    {
        this.playerName = newName;
    }

    public void SavePlayerScoreData()
    {
        var playerScoreData = LoadPlayerScoreData();
        playerScoreData.list.Add(new PlayerScore(score, playerName));//储存当前玩家数据
        playerScoreData.list.Sort((x, y) => y.score.CompareTo(x.score));//降序排列

        SaveSystem.SaveByJson(saveFileName, playerScoreData);
    }
    public PlayerScoreData LoadPlayerScoreData()//读取玩家得分
    {
        var playerScoreData = new PlayerScoreData();
        if (SaveSystem.SaveFileExists(saveFileName))
        {
            playerScoreData = SaveSystem.LoadFromJson<PlayerScoreData>(saveFileName);
        }
        else
        {
            while (playerScoreData.list.Count < 10)//不存在则初始化10个数据
            {
                playerScoreData.list.Add(new PlayerScore(0, playerName));

                SaveSystem.SaveByJson(saveFileName, playerScoreData);
            }
        }
        return playerScoreData;
    }

    #endregion
}
