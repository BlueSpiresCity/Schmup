using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BulletManager))]
public class GameController : MonoBehaviour, IGUIMaster {
	#region Fields and Properties
	//Inspector Assignments
	public BulletManager BulletControl;
	public Camera MainCamera;
	
	[SerializeField]
	private float BoundsOffset;
	
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
	#endregion
	
	void Reset(){
		BoundsOffset = 1f;
	}
	
	void Awake(){
		tag = "GameController";
		ButtonsInternal = new Hashtable();
		if(!BulletControl)
			BulletControl = GetComponent<BulletManager>();
		if(!MainCamera)
			MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		MainCamera.isOrthoGraphic = true;
	}
	
	void Start(){
		GetScreenBounds();
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
