using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {

    public enum SpawnState { SPAWNING, WAITING, COUNTING, NULL };

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Enemy[] enemy;
        //public Transform enemy;
        public float rate;  // Rate of spawning the enemies
    }
    [System.Serializable]
    public class Enemy
    {
        public Transform spawn;
        public ObjectPooler enemy; 
        public string animation;
        public int amount;
        public GameObject[] enemies = null;
    }

    public Wave[] waves;
    private int nextWave = 0;
    public bool replayWave = false;
    public float timeBetweenWaves = 2.5f;
    private float waveCountdown;

    private SpawnState state = SpawnState.NULL;

	// Use this for initialization
	void Start () {
        waveCountdown = timeBetweenWaves;
        
	}
	
	// Update is called once per frame
	void Update () {
        print("Current state: " + state);
        if (state == SpawnState.WAITING)
        {
            //if (!IsEnemyAlive())
            //{
            //    WaveCompleted();
            //}
            //return;
            //StartCoroutine(ActivatingEnemies(waves[nextWave]));
            return;
        }
        if (state == SpawnState.COUNTING)
        {
            
            //return;
        }   
        if (waveCountdown <= 0f)
        {
            if (state != SpawnState.SPAWNING)
            {
                // Start spawning wave
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        } else
        {
            waveCountdown -= Time.deltaTime;
        }
        
    }

    //IEnumerator ActivatingEnemies (Wave _wave)
    //{
    //    for (int i = 0; i < _wave.enemy.Length; i++)
    //    {
    //        //yield return new WaitForSeconds(3f);
    //        int j = 0;
    //        while (j < _wave.enemy[i].enemies.Length)
    //        {
    //            Debug.Log("Activating...");
    //            //Animator _enemyAnimator = _wave.enemy[i].enemies[j].GetComponent<Animator>();
    //            //Debug.Log(_enemyAnimator);
    //            //_enemyAnimator.enabled = true;
    //            yield return new WaitForSeconds(2f);
    //            //Animator _enemyAnimator = Teste2(_wave.enemy[i].enemies[j]);
    //           // _enemyAnimator.Play(_wave.enemy[i].animation);
    //            Debug.Log(j);
    //            Debug.Log("Waiting..");
    //            j++;

    //        }
            
    //    }

    //    state = SpawnState.COUNTING;
    //    yield break;
    //}


    IEnumerator SpawnWave (Wave _wave)
    {
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.enemy.Length; i++)
        {
            //_wave.enemy[i].enemies = new GameObject[_wave.enemy[i].amount];
            for (int j = 0; j <_wave.enemy[i].amount; j++)
            {
                GameObject _currentEnemy = SpawnEnemies(_wave.enemy[i].enemy, _wave.enemy[i].spawn, _wave.enemy[i].animation);
                //_wave.enemy[i].enemies[j] = _currentEnemy;
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(0.5f);
        }
        
        state = SpawnState.WAITING;
        yield break; // Return null value
    }


    GameObject SpawnEnemies(ObjectPooler _enemyObjectPooler, Transform _spawnPosition, string _animation)
    {
        if (!_spawnPosition)
            Debug.LogError("No spawn points referenced!");
        if (!_enemyObjectPooler)
            Debug.LogError("No enemy pooler referenced!");

        GameObject _enemy = _enemyObjectPooler.GetPooledObject();
        if (_enemy == null)
            return null;
        
        _enemy.transform.SetParent(_spawnPosition);
        _enemy.transform.position = _spawnPosition.position;
        _enemy.transform.rotation = _spawnPosition.rotation;
        Animator _enemyAnimator = _enemy.GetComponent<Animator>();
        _enemyAnimator.Play(_animation);
        return _enemy;
    }

    private float searchCountdown = 1f;
    bool IsEnemyAlive(Wave _wave)
    {
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            Enemy[] _allEnemies = _wave.enemy;

        }
        searchCountdown -= Time.deltaTime;
        return true;    
    }

    void WaveCompleted()
    {
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0; // All waves completed!
            return;
        }
        nextWave++;
    }


    //IEnumerator Teste(Wave _wave)
    //{
    //    for (int i = 0; i < _wave.enemy.Length; i++)
    //    {
    //        int currentAmount = _wave.enemy[i].amount;
    //        if (currentAmount <= 0)
    //            continue;
    //        foreach (GameObject _enemy in _wave.enemy[i].enemies)
    //        {
    //            if (_enemy.activeInHierarchy)
    //            {
    //                currentAmount--;
    //            }
    //        }
    //        Debug.Log(currentAmount);
    //        _wave.enemy[i].amount = currentAmount;
            
    //    }
    //    yield return null;
    //}

}
