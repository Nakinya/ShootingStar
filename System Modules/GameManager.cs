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
    public static System.Action onGameOver;//不用事件是因为要在player脚本的die()调用此委托
    public static GameState GameState { get => Instance.gameState; set => Instance.gameState = value; }
    [SerializeField] GameState gameState = GameState.Playing;
    
}
