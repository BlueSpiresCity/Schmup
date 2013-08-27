using UnityEngine;
using System.Collections;

//Handles Spawn points for Bullets
[System.Serializable]
public class BulletPort{
	public float Cooldown = 0.1f;
	public Transform PortTransform;
	public GameObject CurrentBulletPrefab;
	public GameObject[] BulletPrefabs;
	public Transform[] Locations;
	
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
		CurrentBulletPrefab = BulletPrefabs[BulletInt];
	}
	
	public IEnumerator Shoot(BulletManager BulletEngine, float RateModifier = 1f){
		if(!CurrentBulletPrefab) yield break;
		CanFireInternal = false;
		GameObject BulletObject = MonoBehaviour.Instantiate(
					CurrentBulletPrefab,
					PortTransform.position, 
					Quaternion.LookRotation(PortTransform.forward)
				) 
				as GameObject;
		BulletHandler Bullet = BulletObject.GetComponent<BulletHandler>();
		//Make it Live, so it will have the correct flag to Move
		Bullet.IsLive = true;
		//Add to Bullet Manager, which will move it
		BulletEngine.Bullets.Add(Bullet);
		//Begin the Port's cooldown procedure
		yield return new WaitForSeconds(Cooldown / RateModifier);
		CanFireInternal = true;
	}
}
