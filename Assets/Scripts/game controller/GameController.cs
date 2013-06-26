using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	
	private float suspiciousness;
	public float maxSuspiciousness = 100f;
	
	public void raiseSuspiciousness (float delta) {
		suspiciousness += Mathf.Abs(delta);
		if (suspiciousness >= maxSuspiciousness) {
			// TODO: Geisterjäger aktivieren
		}
	}
	
	public void lowerSuspiciousness (float delta) {
		suspiciousness -= Mathf.Abs(delta);
	}
	
	public void resetSuspiciousness () {
		suspiciousness = 0;
	}
	
}
