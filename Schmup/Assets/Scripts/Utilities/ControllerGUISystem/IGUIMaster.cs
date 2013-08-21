/*
 * An interface for use with ButtonHandler.
 * 
 * Write a "Master" class that inherits this interface.
 * ButtonHandler will call ButtonClick and pass a string.
 * 
 * Written by: Wai Kay Kong
 */
using System.Collections;

public interface IGUIMaster{
	bool IsOverButton{
        get;
        set;
    }
	
	Hashtable Buttons{
		get;
	}
	
	ButtonHandler GetButton(string ButtonName);
	
	void ButtonClick(string ButtonName);
}
