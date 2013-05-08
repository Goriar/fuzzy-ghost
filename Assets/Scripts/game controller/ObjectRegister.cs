using UnityEngine;
using System.Collections;

public static class ObjectRegister {
	
	private static ArrayList layers = new ArrayList();
	
	/// 
	/// Registriert Layer
	/// @param layer Layer, der hinzugefügt werden soll
	/// 
	public static void registerLayer (GameObject layer) {
		layers.Add(layer);
	}
	
	/// 
	/// Deregistriert Layer
	/// @param layer Layer, der gelöscht werden soll
	/// 
	public static void unregisterLayer (GameObject layer) {
		layers.Remove(layer);
	}
	
	///
	/// Durchgeht alle Layer, wenn Layer mit gewünschtem Tag gefunden wurde, wird die Layer ausgegeben
	/// @param tag gesuchter Tag für Layer
	/// 
	public static GameObject getLayer(string tag) {
		foreach (GameObject layer in layers) {
			if (tag.Equals(layer.tag)) {
				return layer;
			}
		}
		return null;
	}
	
}
