using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	#region Fields and Properties
	//Inspector Assignments
	public GameController Master;
	
	//Public
	public InventorySystem Inventory;
	public bool UseMouseInput;
	public float Sensitivity;
	public float AttackRateModifier;

	//Local
	private float HorizontalAxis;
	private float VerticalAxis;
	private Vector2 PlayerInput;
	private bool CanAttack;
	private Vector3 LastPosition;
	
	//Controls what Inspector Values will reset to
	void Reset(){
		UseMouseInput = false;
		Sensitivity = 2f;
		AttackRateModifier = 1f;
	}
	#endregion
	
	void Awake(){
		Inventory.Initialize();
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
		
		Screen.showCursor = false;
		Screen.lockCursor = true;
		
		LastPosition = transform.position;
		
		CanAttack = true;
	}
	
	void ShowCursor(bool State){
		Screen.showCursor = State;
		Screen.lockCursor = !State;
	}
	
	//Helper to keep all input stuff in one place
	void ProcessInput(){
		//Get Player Input on Direction
		PlayerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime;
		if(UseMouseInput){
			PlayerInput += new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))/Sensitivity;
		}
		
		//Move Player
		if(PlayerInput != Vector2.zero && !Screen.showCursor){
			Movement();
		}
		
		//Attack
		if(Input.GetButton("Fire1") && CanAttack){
			StartCoroutine(Attack());
		}
		
		//Unhide the Cursor
		if(Input.GetKeyDown("escape")) 
			ShowCursor(!Screen.showCursor);
	}
	
	void Movement(){
		//Add Input
		HorizontalAxis += PlayerInput.x * Sensitivity;
		VerticalAxis += PlayerInput.y * Sensitivity;
		
		//Clamp to Bounds
		HorizontalAxis = Mathf.Clamp(HorizontalAxis, -Master.ScreenBounds.x, Master.ScreenBounds.x);
		VerticalAxis = Mathf.Clamp(VerticalAxis, -Master.ScreenBounds.y, Master.ScreenBounds.y);
		
		//Apply to transform
		transform.position = new Vector3(HorizontalAxis, 0, VerticalAxis);
	}
	
	IEnumerator Attack(){
		CanAttack = false;
		GameObject BulletObject = Instantiate(
				Inventory.BulletPrefabs[Inventory.CurrentBulletType],
				transform.position, 
				Quaternion.LookRotation(transform.forward)
			) 
			as GameObject;
		BulletHandler Bullet = BulletObject.GetComponent<BulletHandler>();
		//Assign the BulletType according to Inventory
		Bullet.BulletType = Inventory.CurrentBulletType;
		//Make it Live, so it will have the correct flag to Move
		Bullet.IsLive = true;
		//Add current Velocity to Bullet
		//Bullet.Velocity += CalculateCurrentVelocity();
		//Add to Bullet Manager, which will move it
		Master.BulletControl.Bullets.Add(Bullet);
		yield return new WaitForSeconds(Bullet.AttackCooldown * AttackRateModifier);
		CanAttack = true;
	}
	
	Vector3 CalculateCurrentVelocity(){
		return (transform.position - LastPosition)/Time.deltaTime;
	}
	
	#endregion
}

//Inventory System
[System.Serializable]
public class InventorySystem{
	public GameObject[] BulletPrefabs;
	public int CurrentBulletType;
	
	public void Initialize(){
		CurrentBulletType = 0;
	}
}
