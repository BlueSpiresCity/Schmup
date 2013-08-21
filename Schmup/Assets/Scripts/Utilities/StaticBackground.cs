/*
 * Used to create a static, unmoving background of assigned image.
 * Make sure to set the camera clear flag to Depth Only.
 * 
 * Script found at:
 * http://wiki.unity3d.com/index.php?title=StaticBackground
 */

using UnityEngine;
 
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Static Background")]
public class StaticBackground : MonoBehaviour {
    public Texture2D background;
	
	// OnPreRender is used instead ( onPreCull works too )
	void OnPreRender (){
        if( background != null )
            Graphics.Blit( background, RenderTexture.active );
    }
}
