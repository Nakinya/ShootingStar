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
    int score;//ʵ����ʾ����
    int currenScore;//����ܷ���
    [SerializeField] Vector3 scoreTextSale = new Vector3(1.2f, 1.2f, 1f);
    
    public void ResetScore()//���÷���
    {
        score = 0;
        ScoreDisplay.UpdateScoreText(score);
    }
    public void AddScore(int scorePoint)
    {
        currenScore += scorePoint;
        StartCoroutine(AddScoreCoroutine());
    }
    IEnumerator AddScoreCoroutine()//���ڶ�̬��ʾ���ӵķ���
    {
        ScoreDisplay.ScaleScoreText(scoreTextSale);//�����ı���С
        while (score < currenScore)
        {
            score++;
            ScoreDisplay.UpdateScoreText(score);//ÿһ֡���ӷ���

            yield return null;
        }
        ScoreDisplay.ScaleScoreText(Vector3.one);
    }
    #endregion
    #region High Score System
    [System.Serializable]
    public class PlayerScore//��ҵ÷�������
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
    public class PlayerScoreData//���մ洢��json�ı��ļ���Ķ���
    {
        public List<PlayerScore> list = new List<PlayerScore>();//���������������
    }

    readonly string saveFileName = "player_score.json";
    string playerName = "No Name";
    public bool HasNewHighScore => score > LoadPlayerScoreData().list[9].score;//�жϷ����Ƿ񳬹��浵�е�10���ķ���

    public void SetPlayerName(string newName)
    {
        this.playerName = newName;
    }

    public void SavePlayerScoreData()
    {
        var playerScoreData = LoadPlayerScoreData();
        playerScoreData.list.Add(new PlayerScore(score, playerName));//���浱ǰ�������
        playerScoreData.list.Sort((x, y) => y.score.CompareTo(x.score));//��������

        SaveSystem.SaveByJson(saveFileName, playerScoreData);
    }
    public PlayerScoreData LoadPlayerScoreData()//��ȡ��ҵ÷�
    {
        var playerScoreData = new PlayerScoreData();
        if (SaveSystem.SaveFileExists(saveFileName))
        {
            playerScoreData = SaveSystem.LoadFromJson<PlayerScoreData>(saveFileName);
        }
        else
        {
            while (playerScoreData.list.Count < 10)//���������ʼ��10������
            {
                playerScoreData.list.Add(new PlayerScore(0, playerName));

                SaveSystem.SaveByJson(saveFileName, playerScoreData);
            }
        }
        return playerScoreData;
    }

    #endregion
}
