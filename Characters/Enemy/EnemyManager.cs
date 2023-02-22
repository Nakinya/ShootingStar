using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class EnemyManager : Singleton<EnemyManager>
{
    public int WaveNumber => waveNumber;
    public float TimeBetweenWaves => timeBetweenWaves;
    public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)];//获得随机敌人
    [SerializeField] GameObject waveUI;
    [SerializeField] bool spawnEnemy = true;
    [SerializeField] GameObject[] enemyPrefab;
    [SerializeField] float timeBetweenWaves = 1f;//波数间隔时间 时间为waveUI的持续时间
    [SerializeField] float timeBetweenSpawns = 1f;//敌人生成间隔时间
    [SerializeField] int minEnemyAmount = 4;
    [SerializeField] int maxEnemyAmount = 10;

    [Header("-----Boss Setting-----")]
    [SerializeField] GameObject bossPrefab;
    [SerializeField] int bossWaveNumber = 3;

    int waveNumber = 1;//波数
    int enemyAmount;//敌人数量
    WaitForSeconds waitTimeBetweenSpawns;
    WaitForSeconds waitBetweenWaves;
    WaitUntil waitUntilNoEnemy;
    public  List<GameObject> enemyList;//用于管理场景中的敌人
    protected override void Awake()
    {
        base.Awake();
        waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);
        waitBetweenWaves = new WaitForSeconds(timeBetweenWaves);
        waitUntilNoEnemy = new WaitUntil(() => enemyList.Count == 0);
        enemyList = new List<GameObject>();
    }

    IEnumerator Start()
    {
        while (spawnEnemy && GameManager.GameState !=GameState.GameOver)
        {
            //yield return waitUntilNoEnemy; 当敌人生成的一瞬间就被消灭时，则会跳过当前波数敌人生成，直接开始第二波敌人生成。
            waveUI.SetActive(true);//生成敌人前启用
            yield return waitBetweenWaves;
            waveUI.SetActive(false);
            yield return StartCoroutine(RandomSpawnCoroutine());
        }
    }

    IEnumerator RandomSpawnCoroutine()
    {
        if (waveNumber % bossWaveNumber == 0)
        {
            var boss = PoolManager.Release(bossPrefab);
            enemyList.Add(boss);
        }
        else
        {
            enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / bossWaveNumber, maxEnemyAmount);//根据波数增加敌人数量
            for (int i = 0; i < enemyAmount; i++)
            {
                enemyList.Add(PoolManager.Release(enemyPrefab[Random.Range(0, enemyPrefab.Length)]));//随机生成敌人类型
                yield return waitTimeBetweenSpawns;
            }
        }

        yield return waitUntilNoEnemy;//生成所有敌人后才会挂起等待 解决敌人生成一瞬间被消灭时所造成的敌人生成问题
        waveNumber++;
    }
    public void RemoveFromList(GameObject enemy)//敌人死亡时调用
    {
        enemyList.Remove(enemy);
    }

    //bool NoEnemy()
    //{
    //    return enemyList.Count == 0;
    //}
}
