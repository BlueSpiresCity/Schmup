using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	#region Fields and Properties
	//Inspector Assignments
	public GameController Master;
	public Camera MainCamera;
	
	//Public
	public bool UseMouseInput;
	public float Sensitivity;
	
	//Private Serialized
	[SerializeField]
	private float BoundsOffset;

	//Local
	private float HorizontalAxis;
	private float VerticalAxis;
	private Vector2 PlayerInput;
	private Vector2 PlayerBounds;
	
	//Controls what Inspector Values will reset to
	void Reset(){
		UseMouseInput = false;
		Sensitivity = 2f;
		BoundsOffset = 1f;
	}
	#endregion
	
	void Awake(){
		Preflight();
	}
	
	void Update(){
		Movement();
		Attack();
		if(Input.GetKeyDown("escape")) 
			ShowCursor(!Screen.showCursor);
	}
	
	#region Functions
	void Preflight(){
		//Check if we have a Master
		if(!Master) Master = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		//Check if we have a reference to the Camera
		if(!MainCamera) MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		MainCamera.isOrthoGraphic = true;
		
		//Get Screen Bounds
		PlayerBounds = new Vector2(MainCamera.orthographicSize * ((float)Screen.width/(float)Screen.height), MainCamera.orthographicSize);
		PlayerBounds -= new Vector2(BoundsOffset, BoundsOffset);
		
		//Get current axis position
		HorizontalAxis = transform.position.x;
		VerticalAxis = transform.position.z;
		
		Screen.showCursor = false;
		Screen.lockCursor = true;
	}
	
	void ShowCursor(bool State){
		Screen.showCursor = State;
		Screen.lockCursor = !State;
	}
	
	void Movement(){
		//Get Player Input
		PlayerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		
		//Check Mouse
		if(UseMouseInput){
			PlayerInput += new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
		}
		
		//Do not run if no Input or if cursor is showing
		if(PlayerInput == Vector2.zero || Screen.showCursor) return;
		
		//Add Input
		HorizontalAxis += PlayerInput.x * Sensitivity;
		VerticalAxis += PlayerInput.y * Sensitivity;
		
		//Clamp to Bounds
		HorizontalAxis = Mathf.Clamp(HorizontalAxis, -PlayerBounds.x, PlayerBounds.x);
		VerticalAxis = Mathf.Clamp(VerticalAxis, -PlayerBounds.y, PlayerBounds.y);
		
		//Apply to transform
		transform.position = new Vector3(HorizontalAxis, 0, VerticalAxis);
	}
	
	void Attack(){
		
	}
	#endregion
}
