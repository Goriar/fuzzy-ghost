using UnityEngine;
using System.Collections;

public class Gui : MonoBehaviour {
	
	public Texture heartTexture;
	public Texture barBorderTexture;
	public Texture barTexture;
	
	private Vector2 spriteSize, heartSize, position;
	private float heartSpace;
	private float guiScale;
	
	// Use this for initialization
	void Start () {
		guiScale = 0.7f;
		spriteSize = new Vector2 (305f, 56f);
		heartSize = new Vector2(61f, 56f);
		position = new Vector2(30f, 30f);
		heartSpace = 5f;
	}
	
	void OnGUI () {
		float playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().health;
		for (int i = 0; i < 4; i++) {
			float lowerThreshold = i*25;
			float upperThreshold = (i+1)*25;	
			float textureOffset = 0;
			if (playerHealth-lowerThreshold > 0) {
				float quarterHealth = ((playerHealth-lowerThreshold <= 25f) ? playerHealth-lowerThreshold : 25f) /25f;
				// ein viertel Herz
				if (quarterHealth <= 0.25f) {
					textureOffset = 0.2f;
				// halbes Herz
				} else if (quarterHealth <= 0.5f) {
					textureOffset = 0.4f;
				// drei viertel Herz
				} else if (quarterHealth <= 0.75f) {
					textureOffset = 0.6f;
				// volles Herz
				} else {
					textureOffset = 0.8f;
				}
			}
			
			// Zeichne Herz
			drawHeart(i*(heartSize.x+heartSpace), textureOffset);
		}
		
		float playerAttention = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().attentionToPlayer;
		float maxAttention = Player.MAX_ATTENTION;
		GUI.DrawTexture(new Rect(30, 95, 259, 20), barBorderTexture, ScaleMode.StretchToFill, true);
		GUI.DrawTexture(new Rect(32, 97, 255*playerAttention/maxAttention, 16), barTexture, ScaleMode.StretchToFill, true);
		
		
	}
	
	void drawHeart (float offset, float textureOffset) {
		// zeichnet herz mit crop
		GUI.BeginGroup( new Rect( position.x + offset, position.y, heartSize.x, heartSize.y ) );		
		GUI.DrawTexture(new Rect(-spriteSize.x * textureOffset, 0, spriteSize.x, spriteSize.y), heartTexture, ScaleMode.StretchToFill, true);
		GUI.EndGroup();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
