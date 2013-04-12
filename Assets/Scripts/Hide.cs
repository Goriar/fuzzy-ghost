using UnityEngine;
using System.Collections;

public class Hide : MonoBehaviour {
	
	// Constants
	private static float VISIBLE = 1f;
	private static float HIDDEN = 0.2f;
	
	// Attributes
	private Component[] renderers;
	
	// Use this for initialization
	void Start () {
	
		renderers = GetComponentsInChildren(typeof(Renderer));
        hide (5f);
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void hide (float duration) {
		foreach (Renderer curRenderer in renderers) {
            Color color;
            foreach (Material material in curRenderer.materials) {
                color = material.color;
                // change alfa for transparency
                if (duration == 0) {
					color.a = HIDDEN;
				} else {
					color.a = Mathf.Lerp(color.a, HIDDEN, 1);
				}
				
                material.color = color;
            }

        }
	}
	
}
