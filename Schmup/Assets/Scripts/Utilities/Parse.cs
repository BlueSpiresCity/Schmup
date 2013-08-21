/*
 * Use for parsing Strings into Ints or Floats
 * 
 * Written by: Wai Kay Kong
 */ 

using UnityEngine;
using System.Collections;

static internal class Parse{
	static public int StringToInt(string str){
		int n;
		if(int.TryParse(str, out n)){
			return n;
		}
		return 0;
	}
	
	static public float StringToFloat(string str){
		float n;
		if(float.TryParse(str, out n)){
			return n;
		}
		return 0;
	}
}
