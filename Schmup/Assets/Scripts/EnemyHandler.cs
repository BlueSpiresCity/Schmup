using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHandler : MonoBehaviour {
	
	public int MaxHealth;
	
	private float CollisionSizeSqrInternal;
	public float CollisionSizeSqr{
		get{return CollisionSizeSqrInternal;}
	}
	
	private int CurrentHealth;
	
	void Reset(){
		MaxHealth = 10;
	}
	
	void Start(){
		tag = "Enemy";
		
		CurrentHealth = MaxHealth;
		
		CollisionSizeSqrInternal = renderer.bounds.extents.x;
		CollisionSizeSqrInternal *= CollisionSizeSqrInternal;
		GameController Master = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		Master.Enemies.Add(this);
	}
	
	public void DoDamage(int DamageInt){
		CurrentHealth -= DamageInt;
		
		if(CurrentHealth <= 0){
			CurrentHealth = MaxHealth;
			print("Dead");
		}
	}
}
