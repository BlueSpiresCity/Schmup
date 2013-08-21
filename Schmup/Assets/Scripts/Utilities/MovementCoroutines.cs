/*
 * A small collection of basic, easy to use and generic movement coroutines.
 * For use when iTween is undesireable and GoKit is not useable.
 * 
 * Start these coroutines in any script like so:
 * StartCoroutine(MovementCoroutines.MoveLerpTo(a, b, c));
 * 
 * Written by: Wai Kay Kong
 * 
 */

using UnityEngine;
using System.Collections;

public static class MovementCoroutines{
	/// <summary>
	/// Moves an object from its original position to the MoveTo position over MoveTime. e.g. Moves TheRock to Vector Position (1,2,3) over 5 seconds.
	/// </summary>
	public static IEnumerator MoveLerpTo(GameObject TheObject, Vector3 MoveTo, float MoveTime){
		Vector3 MoveFrom = TheObject.transform.position;
		float CurrentTime = 0;
		while(CurrentTime < 1f){
			TheObject.transform.position = Vector3.Lerp(MoveFrom, MoveTo, CurrentTime);
			CurrentTime += Time.deltaTime/MoveTime;
			yield return new WaitForEndOfFrame();
		}
		TheObject.transform.position = MoveTo;
	}
	
	/// <summary>
	/// Moves an object from its original position to the MoveTo position at Speed. e.g. Moves TheRock to Vector Position (1,2,3) at 5 units/second
	/// </summary>
	public static IEnumerator MoveConstantSpeed(GameObject TheObject, Vector3 MoveTo, float Speed){
		Vector3 TwoVector = MoveTo - TheObject.transform.position;
		while(TwoVector.sqrMagnitude > 0.025f){
			TheObject.transform.Translate(TwoVector.normalized * Time.deltaTime * Speed, Space.World);
			TwoVector =  MoveTo - TheObject.transform.position;
			yield return new WaitForEndOfFrame();
		}
	}
	
	/// <summary>
	/// Moves an object from its original position to the MoveTo position at Speed, affected by a fake Gravity calculated from Initial and Final positions
	/// </summary>
	public static IEnumerator MoveWithGravity(GameObject TheObject, Vector3 MoveTo, float Speed){
		Vector3 MoveFrom = TheObject.transform.position;
		Vector3 MoveToFlattened = new Vector3(MoveTo.x, TheObject.transform.position.y, MoveTo.z);
		float LerpTime = (MoveToFlattened - MoveFrom).magnitude/Speed;
		float CurrentTime = 0;
		while(CurrentTime < 1f){
			TheObject.transform.position = Vector3.Lerp(MoveFrom, MoveToFlattened, CurrentTime);
			//Add Gravity. Equation derived using  P(T) = (1/2)AT^2 + VinitialT + Pinitial (Kinematics).
			//Where P is position, A is acceleration, V is velocity (in this case, initial velocity is 0) and T is time.
			//The solution to this problem is trivial and is left as an exercise for the reader. Har har.
			//Actually, I just don't want to put all the math here, but basically I want to calculate a custom acceleration.
			//Find the acceleration needed to go from A to B over time T (in this case it is 1 'second', since lerp is 0-1).
			//After that acceleration is calculated, it is plugged back into the Kinematic equation (above).
			//The result is the y component below.
			MoveToFlattened = 
				new Vector3(
					MoveTo.x,
					-MoveFrom.y *CurrentTime * CurrentTime + MoveTo.y * CurrentTime * CurrentTime + MoveFrom.y,
					MoveTo.z
					);
			CurrentTime += Time.deltaTime/LerpTime;
			yield return new WaitForEndOfFrame();
		}
		TheObject.transform.position = MoveTo;
	}
	
	/// <summary>
	/// Moves an object from its original position to the Target transform at Speed. For example, Moves TheRock to transform of ThisOtherRock at 5 units/second.
	/// Useful for when the Target position is moving.
	/// </summary>
	public static IEnumerator MoveTrackTransform(GameObject TheObject, Transform Target, float Speed){
		float CurrentTime = 0;
		Vector3 MoveFrom = TheObject.transform.position;
		float Distance = (Target.position - TheObject.transform.position).magnitude;
		while(CurrentTime < 1f){
			TheObject.transform.position = Vector3.Lerp(MoveFrom, Target.position, CurrentTime);
			TheObject.transform.LookAt(Target.position, Vector3.right);
			TheObject.transform.Rotate(new Vector3(90, 0, 0));
			yield return new WaitForEndOfFrame();
			CurrentTime += Time.deltaTime / (Distance/Speed);
		}
	}
	
	/// <summary>
	/// Moves an object from its original position to the MoveTo position over MoveTime. e.g. Moves TheRock to Rect position (1,2,3,4) over 5 seconds.
	/// Same as MoveLerpTo but used for moving GUITextures via pixel space
	/// </summary>
	public static IEnumerator MoveGUILerpTo(GUITexture TheObject, Rect MoveTo, float MoveTime){
		Vector4 MoveFromVector = new Vector4(TheObject.pixelInset.x, TheObject.pixelInset.y, 
																TheObject.pixelInset.width, TheObject.pixelInset.height);
		Vector4 MoveToVector = new Vector4(MoveTo.x, MoveTo.y, MoveTo.width, MoveTo.height);
		Vector4 CurrentPosition;
		
		float CurrentTime = 0;
		while(CurrentTime < 1f){
			CurrentPosition = Vector4.Lerp(MoveFromVector, MoveToVector, CurrentTime);
			TheObject.pixelInset = new Rect(CurrentPosition.x, CurrentPosition.y, CurrentPosition.z, CurrentPosition.w);
			CurrentTime += Time.deltaTime / MoveTime;
			yield return new WaitForEndOfFrame();
		}
		TheObject.pixelInset = MoveTo;
	}
	
	/// <summary>
	/// Rotates an object from its original rotation to the RotateTo rotation over MoveTime. e.g. Rotates TheRock to Quaternion(1,2,3,4) over 5 seconds.
	/// </summary>
	public static IEnumerator RotateLerpTo(GameObject TheObject, Quaternion RotateTo, float MoveTime){
		Quaternion RotateFrom = TheObject.transform.rotation;
		float CurrentTime = 0;
		while(CurrentTime < 1f){
			TheObject.transform.rotation = Quaternion.Lerp(RotateFrom, RotateTo, CurrentTime);
			CurrentTime += Time.deltaTime / MoveTime;
			yield return new WaitForEndOfFrame();
		}
		TheObject.transform.rotation = RotateTo;
	}
	
	/// <summary>
	/// Scales an object from its original local scale to the ScaleTo scale over MoveTime. e.g. Scales TheRock to Vector Scale (1,2,3) over 5 seconds.
	/// </summary>
	public static IEnumerator LocalScaleLerpTo(GameObject TheObject, Vector3 ScaleTo, float MoveTime){
		Vector3 ScaleFrom = TheObject.transform.localScale;
		float CurrentTime = 0;
		while(CurrentTime < 1f){
			TheObject.transform.localScale = Vector3.Lerp(ScaleFrom, ScaleTo, CurrentTime);
			CurrentTime += Time.deltaTime / MoveTime;
			yield return new WaitForEndOfFrame();
		}
		TheObject.transform.localScale = ScaleTo;
	}
	
	/// <summary>
	/// Experimental. Do not Use.
	/// </summary>
	public static IEnumerator MoveTwoVectorCourse(GameObject TheObject, Vector3 MoveTo, Vector3 SecondVector, float MoveTime){
		Vector3 MoveFrom = TheObject.transform.position;
		float CurrentTime = 0;
		while(CurrentTime < 1f){
			TheObject.transform.position = Vector3.Lerp(MoveFrom, MoveTo + (SecondVector*Time.deltaTime/MoveTime), CurrentTime);
			CurrentTime += Time.deltaTime/MoveTime;
			yield return new WaitForEndOfFrame();
		}
		TheObject.transform.position = MoveTo;
	}
	
	/// <summary>
	/// Lerps a color from original color to ColorTo.
	/// </summary>
	public static IEnumerator ColorGUILerpTo(GUITexture TheObject, Color ColorTo, float LerpTime){
		Color ColorFrom = TheObject.color;
		
		float CurrentTime = 0;
		while(CurrentTime < 1f){
			TheObject.color = Color.Lerp(ColorFrom, ColorTo, CurrentTime);
			CurrentTime += Time.deltaTime/LerpTime;
			yield return new WaitForEndOfFrame();
		}
		TheObject.color = ColorTo;
	}
}
