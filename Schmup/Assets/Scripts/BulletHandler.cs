using UnityEngine;
using System.Collections;

public class BulletHandler : MonoBehaviour {
	
	public int BulletType;
	public bool IsLive;
	public float AttackCooldown;
	public float Rotation;
	public float MaxLifeTime;
	public float CurrentLifeTime;
	public Vector3 Velocity;
	
	private bool IsQueuedForDestructInternal;
	public bool IsQueuedForDestruct{
		get{return IsQueuedForDestructInternal;}
		set{IsQueuedForDestructInternal = value;}
	}
	
	void Reset(){
		BulletType = 0;
		IsLive = false;
		AttackCooldown = 1f;
		Rotation = 0;
		MaxLifeTime = 0;					//0 means it lives forever
		Velocity = Vector3.forward;
	}
	
	void Awake(){
		CurrentLifeTime = 0;
		IsQueuedForDestructInternal = false;
	}
}
