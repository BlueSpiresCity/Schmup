using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BulletManager))]
public class GameController : MonoBehaviour, IGUIMaster {
	#region Fields and Properties
	//Inspector Assignments
	public BulletManager BulletEngine;
	public PlayerController Player;
	public Camera MainCamera;
	
	//Table of Enemies
	public List<EnemyHandler> Enemies;
	
	[SerializeField]
	private float BoundsOffset;
	
	//Screen Bounds
	private Vector2 ScreenBoundsInternal;
	public Vector2 ScreenBounds{
		get{return ScreenBoundsInternal;}
		set{ScreenBoundsInternal = value;}
	}
	
	//Boolean to check if Cursor is over a button
	private bool IsOverButtonInternal;
	public bool IsOverButton{
        get{return IsOverButtonInternal;}
        set{IsOverButtonInternal = value;}
    }
	
	//A table of all GUI Buttons
	private Hashtable ButtonsInternal;
	public Hashtable Buttons{
		get{return ButtonsInternal;}
	}
	
	private float LevelTimeInternal;
	public float LevelTime{
		get{return LevelTimeInternal;}
	}
	#endregion
	
	void Reset(){
		BoundsOffset = 1f;
	}
	
	void Awake(){
		tag = "GameController";
		ButtonsInternal = new Hashtable();
		Enemies = new List<EnemyHandler>();
		if(!BulletEngine)
			BulletEngine = GetComponent<BulletManager>();
		if(!Player)
			Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		if(!MainCamera)
			MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		MainCamera.isOrthoGraphic = true;
	}
	
	void Start(){
		GetScreenBounds();
	}
	
	void FixedUpdate(){
		LevelTimeInternal += Time.fixedDeltaTime;
	}
	
	public void ButtonClick(string ButtonName){
		//Put code here
	}
	
	//Get a specific Button
	public ButtonHandler GetButton(string ButtonName){
		return (ButtonHandler)ButtonsInternal[ButtonName];
	}
	
	void GetScreenBounds(){
		//Get Screen Bounds
		ScreenBoundsInternal = new Vector2(MainCamera.orthographicSize * ((float)Screen.width/(float)Screen.height), MainCamera.orthographicSize);
		ScreenBoundsInternal -= new Vector2(BoundsOffset, BoundsOffset);
	}
}
