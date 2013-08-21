/*
 * A simplistic GUI System involving GUITextures.
 * 
 * Assign this script to whatever GUI Buttons, assign the textures then assign a "Master Object."
 *The Master Object would contain a script that uses the IController interface. 
 *
 *When clicked, this script calls ButtonClick in the Master and passes this object's name.
 *What that does is up to the class that inherits the IController interface.
 * 
 * Written by: Wai Kay Kong
 */ 

using UnityEngine;
using System.Collections;

public class ButtonHandler : MonoBehaviour {
	
	#region Fields and Properties
	public GameObject MasterObject;
	IGUIMaster Master;
	
	//Pre-made arrays of size 1; higher number of toggle states possible
	public Texture2D[] Up = new Texture2D[1];
	public Color UpColorIfNoTexture;
	public Texture2D[] Over = new Texture2D[1];
	public Color OverColorIfNoTexture;
	public Texture2D[] Down = new Texture2D[1];
	public Color DownColorIfNoTexture;
	public Texture2D[] Off = new Texture2D[1];
	
	//Can be held down to repeatedly execute button
	[SerializeField]
	private bool CanHoldDown;
	//Time until Holding Down kicks in
	[SerializeField]
	private float HoldThreshold;
	
	private Color OriginalColor;
	private Color FadedColor;
	private bool IsMouseDown;
	private bool IsClickable;
	private float ButtonHoldTimer;
	
	//Track which toggle state we're in
	private int ToggleStateInternal;
	public int ToggleState{
		get{return ToggleStateInternal;}
		set{ToggleStateInternal = value;}
	}
	#endregion
	
	void Reset(){
		UpColorIfNoTexture = guiTexture.color;
		OverColorIfNoTexture = Color.white;
		DownColorIfNoTexture = Color.black;
	}
	
	void Awake() {
		//Get the Master. GameObject.Find is here because assigning via inspector is getting cumbersome
		if(!MasterObject) MasterObject = GameObject.FindGameObjectWithTag("GameController");
		Master = (IGUIMaster)MasterObject.GetComponent(typeof(IGUIMaster));
		
		OriginalColor = guiTexture.color;
		FadedColor = new Color(0.5f, 0.5f, 0.5f, 0.1f);
		IsMouseDown = false;
		IsClickable = true;
		ButtonHoldTimer = 0;
		ToggleStateInternal = 0;
	}
	
	void Start(){
		//make sure Up, Over and Down are the same size
		ArraySizeCheck();
	}
	
	#region OnMouse Functions
	void OnMouseUpAsButton(){	
		if(!IsClickable) return;
		
		Master.ButtonClick(name);
	}
	
	void OnMouseOver(){
		if(!IsClickable) return;
		if(!Master.IsOverButton){
			Master.IsOverButton = true;
		}
		
		if(!IsMouseDown){
			SetButtonTo(Over[ToggleStateInternal], OverColorIfNoTexture);
		}
	}
	
	void OnMouseExit(){
		if(!IsClickable) return;
		if(Master.IsOverButton){
			Master.IsOverButton = false;
		}
		
		SetButtonTo(Up[ToggleStateInternal], UpColorIfNoTexture);
	}
	
	void OnMouseDown(){
		if(!IsClickable) return;
		IsMouseDown = true;
		SetButtonTo(Down[ToggleStateInternal], DownColorIfNoTexture);
	}
	
	void OnMouseUp(){
		if(CanHoldDown)
			ButtonHoldTimer = 0;
		IsMouseDown = false;
	}
	
	void OnMouseDrag(){
		if(!IsClickable) return;
		if(CanHoldDown){
			if(ButtonHoldTimer < HoldThreshold)
				ButtonHoldTimer += Time.deltaTime;
			else
				Master.ButtonClick(name);
		}
	}
	#endregion
	
	#region Messenger
	void OnEnable(){		
		Master.Buttons.Add(name, this);
		
		if(!IsClickable) return;
		//Reset states button had been disabled
		IsMouseDown = false;
		OnMouseExit();
	}
	
	public void SetClickable(bool State){
		//Ignore if State is already correct
		if(IsClickable == State) return;
		
		IsClickable = State;
		if(IsClickable) SetButtonTo(Up[ToggleStateInternal], OriginalColor);
		else SetButtonTo(Off[ToggleStateInternal], FadedColor);
	}
	
	public void SetToggleState(int StateInt){
		ToggleStateInternal = StateInt;
		OnMouseExit();
	}
	#endregion
	
	#region Script Functions
	void SetButtonTo(Texture2D ButtonTexture, Color ColorIfNoTexture){
		if(ButtonTexture != null){
			guiTexture.color = OriginalColor;
			guiTexture.texture = ButtonTexture;
		}
		else guiTexture.color = ColorIfNoTexture;
	}
	
	void ToggleNext(Texture2D[] ButtonTextures){
		//Skip if there is no toggle
		if(ButtonTextures.Length <= 1) return;
		//Loop back to 0 if needed
		if(ToggleStateInternal + 1 > ButtonTextures.Length - 1){
			ToggleStateInternal = 0;
		}
		else{
			ToggleStateInternal++;
		}
	}
	
	void ArraySizeCheck(){
		//Check array sizes; Only need to check these two conditions because logic.
		if(Up.Length != Over.Length || Up.Length != Down.Length){
			Debug.LogError("Array sizes mismatch. Please fix the array sizes for Texture2D on " + name + ".");
		}
	}
	#endregion
}
