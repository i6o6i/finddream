using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Platformer.Mechanics {
    public class TileCollisionEffect : MonoBehaviour
    {
	// Define what kind of script methods can handle our tile collision events.
	[System.Serializable]
	public class CollisionEvent : UnityEvent<PlayerController> { }

	// Create a data structure to pair up a particular tile with a particular effect.
	[System.Serializable]
	public struct TileEffect {
	    public TileBase tile;
	    public CollisionEvent effect;
	}

	// Expose in the inspector a list of tile-effect mappings.
	public TileEffect[] effects;
	Dictionary<TileBase, CollisionEvent> _effectMap;

	// Pack our map of tile effects into a dictionary for ease of lookups.
	private void OnEnable() {
	    if (_effectMap != null)
		return;

	    _effectMap = new Dictionary<TileBase, CollisionEvent>(effects.Length);
	    foreach (var entry in effects)
		_effectMap.Add(entry.tile, entry.effect);
	}

	private void OnCollisionEnter2D(Collision2D collision) {
	    // If you know in advance what tilemap you're going to collide with,
	    // you can cache this reference instead of searching for it with GetComponent.        
	    var contact = collision.GetContact(0);
	    Debug.Log("TileCollisionEffect.OnCollisionEnter() contact.normal = "+contact.normal);
	    if(contact.normal.y > -.65f) //和KinematicObject的minGroundNormal一致
	    {
		return;
	    }
	    var map = GetComponent<Tilemap>();
	    var grid = map.layoutGrid;

	    // Find the coordinates of the tile we hit.
	    Vector3 contactPoint = contact.point + 0.05f * contact.normal;
	    Vector3 gridPosition = grid.transform.InverseTransformPoint(contactPoint);
	    Vector3Int cell = grid.LocalToCell(gridPosition);

	    // Extract the tile asset at that location.
	    var tile = map.GetTile(cell);

	    if(tile == null)
	    {
		Debug.Log("tile is null");
		return; // No valid tile! Abort!
	    }

	    var playerController = collision.collider.gameObject.GetComponent<PlayerController>();
	    // Check if we have an effect for this tile type. If so, fire it!
	    if (_effectMap.TryGetValue(tile, out CollisionEvent effect) && effect != null)
		effect.Invoke(playerController);
	}

	public void Orange(PlayerController pc) {
	    // TODO: apply conveyor effect.
	    pc.jump_coef_w =2;
	    Debug.Log("Orange");
	}

	public void Green(PlayerController pc) {
	    // TODO: apply bouncy effect.
	    pc.jump_coef_h =2;
	    Debug.Log("Green");
	}
	public void Alice(PlayerController pc) {
	    pc.isStandonIce = true;
	    Debug.Log("Alice");
	}
    }
}
