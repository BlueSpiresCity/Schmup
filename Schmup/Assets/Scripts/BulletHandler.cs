using UnityEngine;
using System.Collections;

public class BulletHandler : MonoBehaviour {
	
	public enum BulletType{
		Normal = 0,
		SpiralIn = 1,
		SpiralOut = 2
	}
	public BulletType Type;
	public int Damage;
	public bool IsLive;
	public bool IsHostile;
	public float AttackCooldown;
	public float Rotation;
	public float MaxLifeTime;
	public Vector3 Velocity;
	
	private float CurrentLifeTimeInternal;
	public float CurrentLifeTime{
		get{return CurrentLifeTimeInternal;}
		set{CurrentLifeTimeInternal = value;}
	}
	
	private bool IsQueuedForDestructInternal;
	public bool IsQueuedForDestruct{
		get{return IsQueuedForDestructInternal;}
		set{IsQueuedForDestructInternal = value;}
	}
	
	private float CollisionSizeSqrInternal;
	public float CollisionSizeSqr{
		get{return CollisionSizeSqrInternal;}
	}
	
	void Reset(){
		Type = BulletType.Normal;
		Damage = 1;
		IsLive = false;
		AttackCooldown = 1f;
		Rotation = 0;
		MaxLifeTime = 0;					//0 means it lives forever
		Velocity = Vector3.forward;
	}
	
	void Awake(){
		CurrentLifeTimeInternal = 0;
		IsQueuedForDestructInternal = false;
		
		CollisionSizeSqrInternal = GetComponentInChildren<Renderer>().bounds.extents.x;
		CollisionSizeSqrInternal *= CollisionSizeSqrInternal;
		
		//Check if Hostile
		if(IsHostile)
			tag = "HostileBullet";
		
		//Check for Special behavior
		if(Type != BulletType.Normal)
			StartSpecialBehavior(Type);
	}
	
	public void StartSpecialBehavior(BulletType BehaviorType){
		StopAllCoroutines();
		
		switch(BehaviorType){
		case BulletType.Normal:
			break;
		case BulletType.SpiralIn:
			StartCoroutine(SpiralIn());
			break;
		default:
			print("Please write special behavior code for " + BehaviorType.ToString() + ".");
			break;
		}
	}
	
	#region Bullet Bevhaiors
	IEnumerator SpiralIn(){
		while(CurrentLifeTimeInternal < MaxLifeTime){
			Rotation *= 1.1f;
			yield return new WaitForFixedUpdate();
		}
	}
	#endregion
}
