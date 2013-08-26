using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletManager : MonoBehaviour {
	//Inspector Assignments
	public GameController Master;	
	
	private List<BulletHandler> BulletsInternal;
	public List<BulletHandler> Bullets{
		get{return BulletsInternal;}
		set{BulletsInternal = value;}
	}
	
	void Awake(){
		BulletsInternal = new List<BulletHandler>();
		if(!Master)
			Master = GetComponent<GameController>();
	}
	
	//Update is called once per frame
	void Update(){
		UpdateBullets();
	}
	
	void LateUpdate(){
		SweepDestructedBullets();
	}
	
	void FixedUpdate(){
		CheckCollisions();
	}
	
	#region Update Bullets
	void UpdateBullets(){
		if(BulletsInternal.Count > 0){
			foreach(BulletHandler Bullet in BulletsInternal){
				if(Bullet.IsLive){
					Bullet.transform.Translate(Bullet.Velocity * Time.deltaTime, Space.Self);
					Bullet.transform.Rotate(Vector3.up * Time.deltaTime * Bullet.Rotation, Space.Self);
					
					//Add to Current Lifetime. Max Life of 0 means it lives forever
					if(Bullet.MaxLifeTime > 0){
						Bullet.CurrentLifeTime += Time.deltaTime;
						//Check with Max Lifetime
						if(Bullet.CurrentLifeTime >= Bullet.MaxLifeTime)
							Bullet.IsQueuedForDestruct = true;
					}
					
					//Check if out of bounds
					Vector3 BulletPosition = Bullet.transform.position;
					if(BulletPosition. x > Master.ScreenBounds.x || BulletPosition.x < -Master.ScreenBounds.x ||
						BulletPosition.z > Master.ScreenBounds.y || BulletPosition.z < -Master.ScreenBounds.y){
						Bullet.IsQueuedForDestruct = true;
					}
				}
			}
		}
	}
	
	void CheckCollisions(){
		float Distance;
		foreach(BulletHandler Bullet in Bullets){
			if(Bullet.IsHostile){
				Distance = (Bullet.transform.position - Master.Player.transform.position).sqrMagnitude - Bullet.CollisionSizeSqr;
				if(Distance < Master.Player.CollisionSizeSqr){
					Bullet.IsQueuedForDestruct = true;
					Master.Player.DoDamage(Bullet.Damage);
				}
			}
			else{
				foreach(EnemyHandler Enemy in Master.Enemies){
					Distance = (Bullet.transform.position - Enemy.transform.position).sqrMagnitude - Bullet.CollisionSizeSqr;
					if(Distance < Enemy.CollisionSizeSqr){
						Bullet.IsQueuedForDestruct = true;
						Enemy.DoDamage(Bullet.Damage);
					}
				}
			}
		}
	}
	
	void SweepDestructedBullets(){
		//Copied to prevent issues when removing elements from List
		BulletHandler[] CopyOfBulletsAsArray = new BulletHandler[BulletsInternal.Count];
		BulletsInternal.CopyTo(CopyOfBulletsAsArray);
		
		foreach(BulletHandler Bullet in CopyOfBulletsAsArray){
			if(Bullet.IsQueuedForDestruct){
				DestroyBullet(Bullet);
			}
		}
	}
	
	public void DestroyBullet(BulletHandler Bullet){
		BulletsInternal.Remove(Bullet);
		Destroy(Bullet.gameObject);
	}
	#endregion
}
