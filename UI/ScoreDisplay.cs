using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


///<summary>
///
///</summary>
public class ScoreDisplay : MonoBehaviour
{
    static Text scoreText;
    private void Awake()
    {
        scoreText = GetComponent<Text>();
    }
    private void Start()
    {
        ScoreManager.Instance.ResetScore();
    }

    public static void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }
    
    public static void ScaleScoreText(Vector3 targetScale)//�����仯ʱ�����ı�
    {
        scoreText.rectTransform.localScale = targetScale;
    }
}
