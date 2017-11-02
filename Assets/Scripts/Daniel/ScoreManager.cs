using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public Text scoreText;
	private float actualScore;

	void Awake(){
		//Acessa o score que está armazenado no PlayerPrefs e armazena em actualScore
		actualScore = PlayerPrefs.GetFloat("Score"); 
	}

	void Update () {
		//Icrementa o score com base no tempo
		actualScore += Time.deltaTime; 
		//Converte actualScore para string e exibe em scoreText, que é um objeto de texto da GUI
		scoreText.text = actualScore.ToString();
		//Armazena o score atual no PlayerPrefs
		PlayerPrefs.SetFloat("Score", actualScore);
	}
}
