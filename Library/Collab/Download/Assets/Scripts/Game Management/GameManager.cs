using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{

    private static GameManager gameManagerInstance;
    public static GameManager GameManagerInstance { get { return gameManagerInstance; } }

    [Header("Player Settings")]
	private PlayerController playerController;
	private int maxPlayerHealth;

	[Header("Enemies Settings")]
	private int totalEnemiesKilled;
	private int levelEnemiesKilled;

	[Header ("Game Settings")]
	private int totalScoredPoints;
	private int gameCurrency;
	private bool hasLevelEnded;

    [Header("Loading Settings")]
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Slider loadingSlider;
    [SerializeField] Text loadingText;


    private void Awake()
    {
        gameManagerInstance = this;
    }

    // Use this for initialization
    void Start () {
        playerController = PlayerController.PlayerControllerInstance;
	}
	
	// Update is called once per frame
	void Update () {

	}

    IEnumerator LoadingOperation(string sceneIndex)
    {
        AsyncOperation loadingOp = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(true);
        while (!loadingOp.isDone)
        {
            float progress = Mathf.Clamp01((loadingOp.progress / 0.9f));
            loadingSlider.value = progress;
            loadingText.text = progress.ToString("0%");
            Debug.Log(progress);
            yield return null;
        }
    }

    public void LoadScene (string sceneName)
    {
        StartCoroutine(LoadingOperation(sceneName));
        Debug.Log("Scene \"" + sceneName + "\" loaded!");
    }

    public void RestartScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Scene \"" + SceneManager.GetActiveScene().name + "\" reloaded!" );
    }

    public void QuitGame()
    {
        Debug.LogAssertion("Quiting game!");
        Application.Quit();
    }

}
