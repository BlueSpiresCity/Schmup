using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	#region Fields and Properties
	//Inspector Assignments
	public GameController Master;
	
	//Public
	public enum PlayerStates{
		Normal = 0,
		Focused = 1
	}
	public PlayerStates State;
	public int MaxHealth;
	public InventorySystem Inventory;
	public BulletPort[] Ports;
	public bool UseMouseInput;
	public float Sensitivity;
	public float AttackRateModifier;
	
	private float CollisionSizeSqrInternal;
	public float CollisionSizeSqr{
		get{return CollisionSizeSqrInternal;}
	}

	//Local
	private float HorizontalAxis;
	private float VerticalAxis;
	private float OriginalSensitivity;
	private float OriginalAttackRateModifier;
	private float EaseTimer;
	private int CurrentHealth;
	private Vector2 PlayerInput;
	private bool CanAttack;
	private bool ShowingCursor;
	private Vector3 LastPosition;
	
	//Controls what Inspector Values will reset to
	void Reset(){
		MaxHealth = 10;
		UseMouseInput = false;
		Sensitivity = 2f;
		AttackRateModifier = 1f;
	}
	#endregion
	
	void Awake(){
		Preflight();
	}
	
	void Update(){
		ProcessInput();
	}
	
	void LateUpdate(){
		LastPosition = transform.position;
	}
	
	#region Functions
	void Preflight(){
		//Check if we have a Master
		if(!Master) Master = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		
		//Get current axis position
		HorizontalAxis = transform.position.x;
		VerticalAxis = transform.position.z;
		
		OriginalSensitivity = Sensitivity;
		OriginalAttackRateModifier = AttackRateModifier;
		
		ShowCursor(false);
		
		LastPosition = transform.position;
		
		CanAttack = true;
		
		CurrentHealth = MaxHealth;
		
		CollisionSizeSqrInternal = GetComponentInChildren<Renderer>().bounds.extents.x;
		CollisionSizeSqrInternal *= CollisionSizeSqrInternal;
	}
	
	//Helper to keep all input stuff in one place
	void ProcessInput(){
		//Get Player Input on Direction
		PlayerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime;
		if(UseMouseInput){
			PlayerInput += new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))/OriginalSensitivity;
		}
		
		//Focus
		Focus(Input.GetButton("Focus"));
		
		//Move Player
		if(PlayerInput != Vector2.zero && !Screen.showCursor){
			Move();
		}
		
		//Attack
		if(Input.GetButton("Fire1") && CanAttack){
			Attack();
		}
		
		//Unhide the Cursor
		if(Input.GetKeyDown("escape")) 
			ShowCursor(!ShowingCursor);
	}
	
	void Focus(bool State){
		if(State){
			Sensitivity = OriginalSensitivity/4f;
			AttackRateModifier = OriginalAttackRateModifier * 1.1f;
			if(EaseTimer < 1f){
				EaseTimer += Time.deltaTime * 2f;
				EaseTimer = Mathf.Clamp01(EaseTimer);
			}
		}
		else{
			if(Sensitivity != OriginalSensitivity)
				Sensitivity = OriginalSensitivity;
			if(AttackRateModifier != OriginalAttackRateModifier)
				AttackRateModifier = OriginalAttackRateModifier;
			if(EaseTimer > 0){
				EaseTimer -= Time.deltaTime * 2f;
				EaseTimer = Mathf.Clamp01(EaseTimer);
			}
		}
		foreach(BulletPort Port in Ports){
			Port.SetLocation(1, EaseTimer);
		}
	}
	
	void Move(){
		//Add Input
		HorizontalAxis += PlayerInput.x * Sensitivity;
		VerticalAxis += PlayerInput.y * Sensitivity;
		
		//Clamp to Bounds
		HorizontalAxis = Mathf.Clamp(HorizontalAxis, -Master.ScreenBounds.x, Master.ScreenBounds.x);
		VerticalAxis = Mathf.Clamp(VerticalAxis, -Master.ScreenBounds.y, Master.ScreenBounds.y);
		
		//Apply to transform
		transform.position = new Vector3(HorizontalAxis, 0, VerticalAxis);
	}
	
	public void Teleport(float X, float Y){
		//Clamp to Bounds
		HorizontalAxis = Mathf.Clamp(X, -Master.ScreenBounds.x, Master.ScreenBounds.x);
		VerticalAxis = Mathf.Clamp(Y, -Master.ScreenBounds.y, Master.ScreenBounds.y);
		
		//Apply to transform
		transform.position = new Vector3(HorizontalAxis, 0, VerticalAxis);
	}

	void Attack(){
		foreach(BulletPort Port in Ports){
			if(Port.CanFire)
				StartCoroutine(Port.Shoot(Master.BulletEngine, AttackRateModifier));
		}
	}
	
	public void DoDamage(int DamageInt){
		CurrentHealth -= DamageInt;
		if(CurrentHealth <= 0){
			//TEMP
			print("Dead");
			CurrentHealth = MaxHealth;
		}
	}
	
	void ShowCursor(bool State){
		ShowingCursor = State;
		Screen.showCursor = State;
		Screen.lockCursor = !State;
	}
	
	Vector3 CalculateCurrentVelocity(){
		return (transform.position - LastPosition)/Time.deltaTime;
	}
	#endregion
}

//Inventory System
[System.Serializable]
public class InventorySystem{
	public GameObject Thing;
}


