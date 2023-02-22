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
    public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)];//����������
    [SerializeField] GameObject waveUI;
    [SerializeField] bool spawnEnemy = true;
    [SerializeField] GameObject[] enemyPrefab;
    [SerializeField] float timeBetweenWaves = 1f;//�������ʱ�� ʱ��ΪwaveUI�ĳ���ʱ��
    [SerializeField] float timeBetweenSpawns = 1f;//�������ɼ��ʱ��
    [SerializeField] int minEnemyAmount = 4;
    [SerializeField] int maxEnemyAmount = 10;

    [Header("-----Boss Setting-----")]
    [SerializeField] GameObject bossPrefab;
    [SerializeField] int bossWaveNumber = 3;

    int waveNumber = 1;//����
    int enemyAmount;//��������
    WaitForSeconds waitTimeBetweenSpawns;
    WaitForSeconds waitBetweenWaves;
    WaitUntil waitUntilNoEnemy;
    public  List<GameObject> enemyList;//���ڹ������еĵ���
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
            //yield return waitUntilNoEnemy; ���������ɵ�һ˲��ͱ�����ʱ�����������ǰ�����������ɣ�ֱ�ӿ�ʼ�ڶ����������ɡ�
            waveUI.SetActive(true);//���ɵ���ǰ����
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
            enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / bossWaveNumber, maxEnemyAmount);//���ݲ������ӵ�������
            for (int i = 0; i < enemyAmount; i++)
            {
                enemyList.Add(PoolManager.Release(enemyPrefab[Random.Range(0, enemyPrefab.Length)]));//������ɵ�������
                yield return waitTimeBetweenSpawns;
            }
        }

        yield return waitUntilNoEnemy;//�������е��˺�Ż����ȴ� �����������һ˲�䱻����ʱ����ɵĵ�����������
        waveNumber++;
    }
    public void RemoveFromList(GameObject enemy)//��������ʱ����
    {
        enemyList.Remove(enemy);
    }

    //bool NoEnemy()
    //{
    //    return enemyList.Count == 0;
    //}
}
