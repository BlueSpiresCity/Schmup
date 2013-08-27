using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {
	
	public GameController Master;
	
	public EnemySpawn[] Spawns;
	
	void Awake(){
		if(!Master)
			Master = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}
	
	void FixedUpdate(){
		SpawningEngine();
	}
	
	void LateUpdate(){
		SweepDestructedEnemies();
	}
	
	void SpawningEngine(){
		//Write Spawing Code Here
	}
	
	public IEnumerator Spawn(EnemySpawn Spawn){
		GameObject EnemyObject = Instantiate(Spawn.Prefab, Spawn.MovePath[0].position, Spawn.MovePath[0].rotation) as GameObject;
		EnemyHandler Enemy = EnemyObject.GetComponent<EnemyHandler>();
		Master.Enemies.Add(Enemy);
		for(int i = 1; i < Spawn.MovePath.Length - 1; i++){
			yield return StartCoroutine(
				MovementCoroutines.MoveLerpWithRotaTo(
					EnemyObject, 
					Spawn.MovePath[i].position,
					Spawn.MovePath[i].rotation,
					Spawn.LerpTime
				)
			);
		}
		Enemy.IsQueuedForDestruct = true;
	}
	
	void SweepDestructedEnemies(){
		//Copied to prevent issues when removing elements from List
		EnemyHandler[] CopyOfEnemiesAsArray = new EnemyHandler[Master.Enemies.Count];
		Master.Enemies.CopyTo(CopyOfEnemiesAsArray);
		
		foreach(EnemyHandler Enemy in CopyOfEnemiesAsArray){
			if(Enemy.IsQueuedForDestruct){
				Master.Enemies.Remove(Enemy);
				Destroy(Enemy.gameObject);
			}
		}
	}
}

[System.Serializable]
public class EnemySpawn{
	public GameObject Prefab;
	public float SpawnTime;
	public Transform[] MovePath;
	public float LerpTime;
	
	public bool SpawnsRepeatedly;
	public float SpawnCooldown;
	
	private bool CanSpawnInternal = true;
	public bool CanSpawn{
		get{return CanSpawnInternal;}
		set{CanSpawnInternal = value;}
	}
}
