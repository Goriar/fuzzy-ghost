using UnityEngine;
using System.Collections;

public class Gui : MonoBehaviour {
	
	public Texture heartTexture;
	public Texture barBorderTexture;
	public Texture barTexture;
	
	private Vector2 spriteSize, heartSize, position;
	private float heartSpace;
	private float guiScale;
	
	///
	/// Initialisierung
	///
	void Start () {
		guiScale = 0.7f; // depricated! (eventuell noch von Verwendung)
		// GUI Größen und Positionen
		spriteSize = new Vector2 (305f, 56f);
		heartSize = new Vector2(61f, 56f);
		position = new Vector2(30f, 30f);
		// Platz zwischen den Herzen
		heartSpace = 5f;
	}
	
	void OnGUI () {
		// Lebensanzeige
		float playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().health;
		// Schleife für alle vier Herzen
		for (int i = 0; i < 4; i++) {
			// Abgrenzungen für Anfang und Ende eines Herzens (alle viertel Leben)
			float lowerThreshold = i*25;
			float upperThreshold = (i+1)*25;	
			float textureOffset = 0; // Sprite offset
			// Wenn Leben oberhalb oder innerhalb des Herzens, ansonsten ist Herz schwarz
			if (playerHealth-lowerThreshold > 0) {
				// HP Wert für aktuelles Herz (1 für volles Herz, 0 für leeres)
				// max. Wert 25/25 => 1.0f
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
		
		// Attention Anzeige
		float playerAttention = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().attentionToPlayer;
		float maxAttention = Player.MAX_ATTENTION;
		// Zeichne Balken Rahmen
		GUI.DrawTexture(new Rect(30, 95, 259, 20), barBorderTexture, ScaleMode.StretchToFill, true);
		// Zeichne Balken
		GUI.DrawTexture(new Rect(32, 97, 255*playerAttention/maxAttention, 16), barTexture, ScaleMode.StretchToFill, true);
		
		// Item Anzeige
		Item playerItem = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().getItem();
		// Zeichne Icon, wenn Gegenstand vorhanden
		if (playerItem != null) {
			GUI.DrawTexture(new Rect(30, Screen.height-30-80, 80, 80), playerItem.getIcon(), ScaleMode.StretchToFill, true);
		}
		
		
		
	}
	
	// Zeichne Herz
	void drawHeart (float offset, float textureOffset) {
		// zeichnet herz mit crop
		GUI.BeginGroup( new Rect( position.x + offset, position.y, heartSize.x, heartSize.y ) );		
		GUI.DrawTexture(new Rect(-spriteSize.x * textureOffset, 0, spriteSize.x, spriteSize.y), heartTexture, ScaleMode.StretchToFill, true);
		GUI.EndGroup();
	}
	
	// Update is called once per frame
	void Update () {
		// Klick auf Icon (zum entfernen von Gegenstand aus Inventar)
		if (Input.GetMouseButton(0) && GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().getItem() != null) {
			
			// AABB Check
			bool insideButtonX = (Input.mousePosition.x >= 30 && Input.mousePosition.x <= 110);
			bool insideButtonY = (Input.mousePosition.y >= 30 && Input.mousePosition.y <= 110);
			
			// Wenn innerhalb Icon in X und Y Achse, lasse Item fallen
			if (insideButtonX && insideButtonY) {
				GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().dropItem();
			}
		}
	}
}
