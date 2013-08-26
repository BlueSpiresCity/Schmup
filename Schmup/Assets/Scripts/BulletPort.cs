using UnityEngine;
using System.Collections;

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
