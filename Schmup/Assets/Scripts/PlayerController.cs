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
	public InventorySystem Inventory;
	public BulletPort[] Ports;
	public bool UseMouseInput;
	public float Sensitivity;
	public float AttackRateModifier;
	


	//Local
	private float HorizontalAxis;
	private float VerticalAxis;
	private float OriginalSensitivity;
	private float OriginalAttackRateModifier;
	private float EaseTimer;
	private Vector2 PlayerInput;
	private bool CanAttack;
	private bool ShowingCursor;
	private Vector3 LastPosition;
	
	//Controls what Inspector Values will reset to
	void Reset(){
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
	}
	
	void ShowCursor(bool State){
		ShowingCursor = State;
		Screen.showCursor = State;
		Screen.lockCursor = !State;
	}
	
	//Helper to keep all input stuff in one place
	void ProcessInput(){
		//Get Player Input on Direction
		PlayerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime;
		if(UseMouseInput){
			PlayerInput += new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))/OriginalSensitivity;
		}
		
		//Focus
		if(Input.GetButton("Focus")){
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
		
		//Move Player
		if(PlayerInput != Vector2.zero && !Screen.showCursor){
			Move();
		}
		
		//Attack
		if(Input.GetButton("Fire1") && CanAttack){
			foreach(BulletPort Port in Ports){
				if(Port.CanFire)
					Shoot(Port);
			}
		}
		
		//Unhide the Cursor
		if(Input.GetKeyDown("escape")) 
			ShowCursor(!ShowingCursor);
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

	void Shoot(BulletPort Port){
		GameObject BulletObject = Instantiate(
				Port.BulletPrefab,
				Port.PortTransform.position, 
				Quaternion.LookRotation(Port.PortTransform.forward)
			) 
			as GameObject;
		BulletHandler Bullet = BulletObject.GetComponent<BulletHandler>();
		//Make it Live, so it will have the correct flag to Move
		Bullet.IsLive = true;
		//Add to Bullet Manager, which will move it
		Master.BulletEngine.Bullets.Add(Bullet);
		//Begin the Port's cooldown procedure
		StartCoroutine(Port.StartCooldown(AttackRateModifier));
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

//Handles Spawn points for Bullets
[System.Serializable]
public class BulletPort{
	public float Cooldown = 0.1f;
	public Transform PortTransform;
	public GameObject BulletPrefab;
	public Transform[] Locations;
	public GameObject[] BulletPrefabs;
	
	private bool CanFireInternal = true;
	public bool CanFire{
		get{return CanFireInternal;}
		set{CanFireInternal = value;}
	}
	
	public IEnumerator StartCooldown(float Modifier = 1f){
		CanFireInternal = false;
		yield return new WaitForSeconds(Cooldown / Modifier);
		CanFireInternal = true;
	}
	
	public void SetLocation(int LocationsInt, float LerpValue){
		LocationsInt = Mathf.Clamp(LocationsInt, 0, Locations.Length);
		
		if(LerpValue == 0){
			PortTransform.position = Locations[0].position;
			PortTransform.rotation = Locations[0].rotation;
		}
		
		PortTransform.position = Vector3.Lerp(Locations[0].position, Locations[LocationsInt].position, LerpValue);
		PortTransform.rotation = Quaternion.Lerp(Locations[0].rotation, Locations[LocationsInt].rotation, LerpValue);
	}
	
	public void SetBulletPrefab(int BulletInt){
		BulletPrefab = BulletPrefabs[BulletInt];
	}
}


