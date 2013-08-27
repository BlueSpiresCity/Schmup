using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHandler : MonoBehaviour {
	
	public GameController Master;
	
	public int MaxHealth;
	public float AttackRateModifier;
	public BulletPort[] Ports;
	
	private float CollisionSizeSqrInternal;
	public float CollisionSizeSqr{
		get{return CollisionSizeSqrInternal;}
	}
	
	private bool IsQueuedForDestructInternal;
	public bool IsQueuedForDestruct{
		get{return IsQueuedForDestructInternal;}
		set{IsQueuedForDestructInternal = value;}
	}
	
	private int CurrentHealth;
	
	void Reset(){
		MaxHealth = 10;
		AttackRateModifier = 1f;
	}
	
	void Start(){
		Master = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		//Master.Enemies.Add(this);
		
		tag = "Enemy";
		
		CurrentHealth = MaxHealth;
		
		CollisionSizeSqrInternal = renderer.bounds.extents.x;
		CollisionSizeSqrInternal *= CollisionSizeSqrInternal;
		
		IsQueuedForDestructInternal = false;
	}
	
	void Update(){
		Attack();
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
			CurrentHealth = MaxHealth;
			IsQueuedForDestructInternal = true;
		}
	}
}
