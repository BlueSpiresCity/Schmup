using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour, IGUIMaster {

	private bool IsOverButtonInternal;
	public bool IsOverButton{
        get{return IsOverButtonInternal;}
        set{IsOverButtonInternal = value;}
    }
	
	private Hashtable ButtonsInternal;
	public Hashtable Buttons{
		get{return ButtonsInternal;}
	}
	
	public ButtonHandler GetButton(string ButtonName){
		return (ButtonHandler)ButtonsInternal[ButtonName];
	}
	
	void Awake(){
		tag = "GameController";
		ButtonsInternal = new Hashtable();
	}
	
	public void ButtonClick(string ButtonName){
		//Put code here
	}
}
