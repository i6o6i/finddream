using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Platformer.Mechanics {
    public class TileCollisionEffect : MonoBehaviour
    {
	[System.Serializable]
	public class CollisionEvent : UnityEvent<PlayerController> { }

	[System.Serializable]
	public struct TileEffect {
	    public TileBase tile;
	    public CollisionEvent effect;
	}

	public TileEffect[] effects;
	Dictionary<TileBase, CollisionEvent> _effectMap;

	private void OnEnable() {
	    if (_effectMap != null)
		return;

	    _effectMap = new Dictionary<TileBase, CollisionEvent>(effects.Length);
	    foreach (var entry in effects)
		_effectMap.Add(entry.tile, entry.effect);
	}

	private void OnCollisionEnter2D(Collision2D collision) {
	    var contact = collision.GetContact(0);
	    Debug.Log("TileCollisionEffect.OnCollisionEnter() contact.normal = "+contact.normal);
	    if(contact.normal.y > -.65f) //和KinematicObject的minGroundNormal一致，玩家碰撞到方块的矢量，落地时为负值
	    {
		return;
	    }
	    var map = GetComponent<Tilemap>();
	    var grid = map.layoutGrid;

	    Vector3 contactPoint = contact.point + 0.05f * contact.normal;
	    Vector3 gridPosition = grid.transform.InverseTransformPoint(contactPoint);
	    Vector3Int cell = grid.LocalToCell(gridPosition);

	    var tile = map.GetTile(cell);

	    if(tile == null)
	    {
		Debug.Log("tile is null");
		return; 
	    }

	    var playerController = collision.collider.gameObject.GetComponent<PlayerController>();
	    if (_effectMap.TryGetValue(tile, out CollisionEvent effect) && effect != null)
		effect.Invoke(playerController);
	    else {
		Debug.Log("TileCollisionEffect.OnCollisionEnter() cannot find corresponding tile");
	    }
	}

	public void Orange(PlayerController pc) {
	    pc.jump_coef_w =2;
	    Debug.Log("Orange");
	}

	public void Green(PlayerController pc) {
	    pc.jump_coef_h =1.5f;
	    Debug.Log("Green");
	}
	public void Alice(PlayerController pc) {
	    pc.isStandonIce = true;
	    Debug.Log("Alice");
	}
    }
}
