using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    private static LevelManager levelManagerInstance;
    public static LevelManager LevelManagerInstance { get { return levelManagerInstance; } }

    private enum GameState { RUNNING, PAUSED, ENDED }
    private GameState gameState;
    public static event Action GameStateEvt;
    
    private void GameStateStatus(GameState _gameState)
    {
        switch (_gameState)
        {
            case GameState.RUNNING:
                if (GameStateEvt != null) GameStateEvt();
                Time.timeScale = timeScale;
                Time.fixedDeltaTime = fixedTimeStep * Time.timeScale;
                uiPause.SetActive(false);
                uiEndGame.SetActive(false);
                Debug.Log("Game state " + _gameState);
                break;
            case GameState.PAUSED:
                Time.timeScale = 0f;
                Time.fixedDeltaTime = fixedTimeStep * Time.timeScale;
                uiPause.SetActive(true);
                uiEndGame.SetActive(false);
                Debug.Log("Game state " + _gameState);
                break;
            case GameState.ENDED:
                Time.timeScale = 1f;
                Time.fixedDeltaTime = fixedTimeStep * Time.timeScale;
                uiPause.SetActive(false);
                uiEndGame.SetActive(true);
                Debug.Log("Game state " + _gameState);
                break;
            default:
                break;
        }
    }
    

    [Header("Level Settings")]
	private PlayerController playerController;
    private EnemyController enemyController;
    private GameManager gameManager;
	private int levelScoredPoints = 0;
    private int levelShootsFired = 0;
    private int levelEnemiesKilled = 0;
	private bool isGamePaused;
    private float timeScale;
    private float fixedTimeStep = 0.02f;
    private float levelTimer, levelSeconds, levelMinutes;
    private bool hasLevelEnded;
    
    [Header("UI Settings")]
    public Camera mainCamera;
	public GameObject uiPause;
    public GameObject uiHUD;
    public GameObject uiEndGame;
    public Text scoreText;
	public Text missileTimerText;
    public Text levelTimerText;
    public Image health_FillImage;
    public Text health_Text;
    public Animator healthAnim;
    public Animator hudAnimation;

    void Awake()
    {
        levelManagerInstance = this;
        timeScale = Time.timeScale;
        PlayerController.PlayerHealthEvt  += SetHealthUI;
        PlayerController.OnPlayerDeathEvt += GameLevelOver;

        playerController = PlayerController.PlayerControllerInstance;
    }

    private void OnDisable()
    {
        PlayerController.PlayerHealthEvt  -= SetHealthUI;
        PlayerController.OnPlayerDeathEvt -= GameLevelOver;
    }

    // Use this for initialization
    void Start () {
        GameStateStatus(gameState = GameState.RUNNING);
        //healthAnim.SetBool("IsPlayerAlive", playerController.IsAlive);
        //SetHealthUI();
        EnemyController.OnEnemyDeathEvt += ScoreLogic;
        isGamePaused = false;
        hasLevelEnded = false;
        hudAnimation.enabled = false;
	}

    float initialTimer = 3f;
    public Text tst;
	// Update is called once per frame
	void Update () {
        //if (initialTimer <= 0f)
        //{
        //    initialTimer = 0;
        //    tst.enabled = false;
        //    hudAnimation.enabled = true;
        //    uiHUD.SetActive(true);
        //    hudAnimation.Play("InitialHUD_Animation");
        //} else
        //{
        //    tst.text = Mathf.Ceil(initialTimer).ToString("0");
        //    uiHUD.SetActive(false);
        //    initialTimer -= Time.deltaTime;
        //}

        if (gameState != GameState.RUNNING) return;

        scoreText.text =  LevelScore.ToString ("000000");
		missileTimerText.text = Mathf.Ceil(playerController.MissileTimer).ToString ("0");
		if (playerController.MissileTimer <= 0f) {
			missileTimerText.gameObject.SetActive (false);
		} else {
			missileTimerText.gameObject.SetActive (true);
		}
        LevelTimerUI();
    }
    

	void LateUpdate () {
		if (Input.GetButtonDown ("Cancel") && gameState == GameState.RUNNING) {

            GameStateStatus(gameState = GameState.PAUSED);
        } else if (Input.GetButtonDown ("Cancel") && gameState == GameState.PAUSED) {

            GameStateStatus(gameState = GameState.RUNNING);
        }
	}

    private void GameLevelOver()
    {
        hasLevelEnded = true;
        GameStateStatus(gameState = GameState.ENDED);
    }


    private void SetHealthUI(float _currentHealth, float _maximumHealth)
    {
        if (playerController.HasShield)
        {
            //health_Slider.value = playerController.ShieldHealth;
            health_FillImage.color = Color.cyan;
            if (playerController.ShieldHealth <= 0f)
            {
                playerController.HasShield = false;
            }
        }
        else
        {
            health_FillImage.fillAmount = _currentHealth /  _maximumHealth; 
            health_Text.text = (_currentHealth /  _maximumHealth).ToString("0% HEALTH");
        }
    }

    public void LevelTimerUI()
    {
        if (playerController.IsAlive)
        {
            levelTimer += Time.deltaTime;
            levelSeconds = Mathf.Floor(levelTimer % 60f);
            levelMinutes = Mathf.Floor(levelTimer / 60f);
            levelTimerText.text = levelMinutes.ToString("00") + ":" + levelSeconds.ToString("00");
        }
    }

    private void ScoreLogic (int enemyPoints) {
        levelEnemiesKilled++;
        float tempScore = enemyPoints * Mathf.Abs((Time.deltaTime / (1 / Time.time)));
        Debug.Log(tempScore);
        levelScoredPoints += (int)tempScore;
	}

	public int LevelScore
    {
        get { return levelScoredPoints; }
        set { levelScoredPoints = value; }
    }

    public int LevelShootsFired
    {
        get { return levelShootsFired; }
        set { levelShootsFired = value; }
    }

    public bool HasLevelEnded
    {
        get { return hasLevelEnded; }
    }

    public bool IsGamePaused
    {
        get { return isGamePaused; }
    }

 //   public static event Action<bool> PausedGameEvt;
	//public void PauseGame()
	//{
 //       if (PausedGameEvt != null) PausedGameEvt(true);
 //       Debug.Log("Game Paused!");
 //       isGamePaused = !isGamePaused;
 //       playerController.AllowShooting = false;
 //       playerController.AllowMissile = false;
 //       uiPause.SetActive (true);
	//	Time.timeScale = 0f;
	//	Time.fixedDeltaTime = fixedTimeStep * Time.timeScale;
	//}

 //   public static event Action<bool> ResumedGameEvt;
 //   public void ResumeGame()
	//{
 //       if (ResumedGameEvt != null) ResumedGameEvt(false);
 //       Debug.Log("Game Resumed!");
 //       isGamePaused = !isGamePaused;
 //       playerController.AllowShooting = true;
 //       playerController.AllowMissile = true;
 //       uiPause.SetActive (false);
	//	Time.timeScale = 1f;
	//	Time.fixedDeltaTime = fixedTimeStep * Time.timeScale;
	//}
}
