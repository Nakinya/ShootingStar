using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public enum GameState
{
    Playing,
    Paused,
    GameOver,
    Scoring
}

public class GameManager : PersistanSingleton<GameManager>
{
    public static System.Action onGameOver;//�����¼�����ΪҪ��player�ű���die()���ô�ί��
    public static GameState GameState { get => Instance.gameState; set => Instance.gameState = value; }
    [SerializeField] GameState gameState = GameState.Playing;
    
}
