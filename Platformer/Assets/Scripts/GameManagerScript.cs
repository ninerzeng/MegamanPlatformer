using UnityEngine;
using System.Collections;

public enum MenuTypes : byte
{
	MainMenu = 0,
	PassWord = 1,
	GameOverMenu = 3
}
public class GameManagerScript : MonoBehaviour {
	public AudioClip ClickSound;
	private AudioSource m_SoundSource;
	public bool isMenuActive {
		get;
		set;
	}
	public bool GameOver {
		get;
		set;
	}
	
	private Settings m_Settings = new Settings();

	private readonly string[] MenuNames = new string[]
	{
		"Main Menu", // 0
		"Pass Word", // 1
		"Game Over" // 2
	};
	private readonly GUI.WindowFunction[] MenuFunctions = null;
	
	public MenuTypes ActiveMenu {
				get;
				set;
	}
	public GameManagerScript()
	{
		MenuFunctions = new GUI.WindowFunction[]
		{
			MainMenu, // 0
			PassWord, //1
			GameOverMenu
		};
	}

	void Awake()
	{
		isMenuActive = true
			;
		Application.runInBackground = true;
		DontDestroyOnLoad (gameObject);
		m_SoundSource = Camera.main.transform.FindChild ("Sound").GetComponent<AudioSource> ();
		m_Settings.Load( Camera.main.transform.FindChild ("Music").GetComponent<AudioSource> (), 
		                m_SoundSource) ;
		
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (GameOver == true) {
//			this.GameOverMenu (2);				
		}


	}
	private void MainMenu (int id)
	{
		GUILayout.Label ("Test");
		if (GUILayout.Button ("Start Game")) {
			m_SoundSource.PlayOneShot(ClickSound);
			isMenuActive = false;		
		}
		if (GUILayout.Button ("Pass Word")) {
			m_SoundSource.PlayOneShot(ClickSound);
			ActiveMenu= MenuTypes.PassWord;		
		}
	}
	private void PassWord(int id)
	{
		GUILayout.Label ("Megaman II Prototype");
		if(GUILayout.Button ("Back")) // this is temporary 
		{
			m_SoundSource.PlayOneShot(ClickSound);
			ActiveMenu = MenuTypes.MainMenu;
		}
	}

	public static void GameOverMenu(int id){
		GUILayout.Label ("GameOver");
		
	}
	void OnGUI()
	{
		const int Width = 400;
		const int Height = 300;
		if (isMenuActive) {
			Rect windowRect = new Rect((Screen.width - Width)/2, 
			                           (Screen.height - Height)/2,
			                           Width,
			                           Height);		
			GUILayout.Window(0, windowRect, MenuFunctions[(byte)ActiveMenu], MenuNames[(byte) ActiveMenu]);
			
		}
	}
}
