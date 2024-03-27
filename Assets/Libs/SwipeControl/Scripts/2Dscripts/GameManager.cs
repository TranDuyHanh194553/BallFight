using UnityEngine;
using System.Collections;

public class IGameManager: MonoBehaviour {
	public static IGameManager Instance{ get; private set;}

	public enum GameState{Menu,Playing, Dead, Finish};
	public GameState State{ get; set; }

	public int starsDefault = 100;

	[Header("Floating Text")]
	public GameObject FloatingText;


	[HideInInspector]
	public bool isNoLives = false;

	void Awake(){
		Instance = this;
		State = GameState.Menu;
	}


	public int Point{ get; set; }
	int savePointCheckPoint;

	public int SavedStars{ 
		get { return PlayerPrefs.GetInt (GlobalValue.Stars, 0); } 
		set { PlayerPrefs.SetInt (GlobalValue.Stars, value); } 
	}
	public int SavedPoints {
		get {
			string mode;

				mode = GlobalValue.ModeDual;
			
			return PlayerPrefs.GetInt (mode, 0); } 
		set { 
			string mode;
		
				mode = GlobalValue.ModeDual;

			PlayerPrefs.SetInt (mode, value); } 
	}


	void Start(){
		if (!PlayerPrefs.HasKey (GlobalValue.Stars))
			SavedStars = starsDefault;
		
		StartGame ();
	}

	public void ShowFloatingText(string text, Vector2 positon, Color color){
		GameObject floatingText = Instantiate (FloatingText) as GameObject;
		var _position = Camera.main.WorldToScreenPoint (positon);

		floatingText.transform.position = _position;
			
	}

	public void StartGame(){
		State = GameState.Playing;
	}

	public void GameOver(){
//		State = GameState.Dead;
		if (Point > SavedPoints)
			SavedPoints = Point;
		



		StartCoroutine (ResetCo (0.1f));


//		AdsController.ShowAds ();
	}

	IEnumerator ResetCo(float time){
		yield return new WaitForSeconds (time);

		//reset all value and send the command to others
		Point = 0;
		//TheBall.Instance.Reset ();
	
		GlobalValue.combo = 1;
	}
}
