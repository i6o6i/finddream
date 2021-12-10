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
	private LineRenderer m_LineDrawer;
	private int idx;

	private void OnEnable() {
	    if (_effectMap != null)
		return;

	    _effectMap = new Dictionary<TileBase, CollisionEvent>(effects.Length);
	    foreach (var entry in effects)
		_effectMap.Add(entry.tile, entry.effect);
	    idx = 0;
	}
	void DrawCross(Vector3 center)
	{
	    LineRenderer line_render = gameObject.GetComponent<LineRenderer>();
	    line_render.startColor = Color.white;
	    line_render.endColor = Color.black;
	    line_render.positionCount = 12;
	    idx= idx%12;
	    var x = new Vector3(1,0,0);
	    var y = new Vector3(0,1,0);
	    line_render.SetPosition(idx++,center);
	    line_render.SetPosition(idx++,center-x);
	    line_render.SetPosition(idx++,center+x);
	    line_render.SetPosition(idx++,center-y);
	    line_render.SetPosition(idx++,center+y);
	    line_render.SetPosition(idx++,center);
	    line_render.startWidth = 0.02f;
	    line_render.endWidth = 0.02f;
	    line_render.material =  new Material(Shader.Find("Sprites/Default"));
	}

	private void OnCollisionStay2D(Collision2D collision) {
	    var points=new ContactPoint2D[100];
	    var collider = collision.collider;
	    int contact_point_num = collider.GetContacts(points);
	    /*
	    Debug.Log("TileCollisionEffect.OnCollisionEnter()"
		    +" contact.normal = "+points[0].normal
		    +" contact.points.Count = "+contact_point_num
		    );
	    */
	    if(points[0].normal.y < 0) //和KinematicObject的minGroundNormal一致，玩家碰撞到方块的矢量，落地时为负值
	    {
		Debug.Log("TileCollisionEffect.OnCollisionEnter() contact_points[0].normal.y <= -.65f");
		return;
	    }

	    var map = GetComponent<Tilemap>();
	    var grid = map.layoutGrid;

	    var tileWorldPos = points[1].point;
	    Vector3 localPosition = grid.transform.InverseTransformPoint(tileWorldPos);
	    /*
	    Debug.Log("TileCollisionEffect.OnCollisionEnter()"
		    +" contact.point = "+points[0].point
		    +" localPosition = "+localPosition
		    );
		    */
	    localPosition = localPosition - (Vector3)points[1].normal*0.5f;
	    Vector3Int cell = grid.LocalToCell(localPosition);

	    /*
	    Debug.Log("TileCollisionEffect.OnCollisionEnter()"
		    +" contact.point = "+points[0].point
		    +" localPosition = "+localPosition
		);
		*/
	    DrawCross(points[1].point);
	    Vector3 WorldPos = grid.transform.TransformPoint(localPosition);
	    DrawCross(WorldPos);

	    var tile = map.GetTile(cell);

	    var playerController = collider.gameObject.GetComponent<PlayerController>();
	    if(playerController == null)
	    {
		Debug.Log("TileCollisionEffect.OnCollisionEnter() playerController is null");
		return;
	    }

	    if(tile == null)
	    {
		Debug.Log("TileCollisionEffect.OnCollisionEnter() tile is null");
		return; 
	    }

	    if (_effectMap.TryGetValue(tile, out CollisionEvent effect) && effect != null)
	    {
		Debug.Log("TileCollisionEffect.OnCollisionEnter() found corresponding tile");
		effect.Invoke(playerController);
	    }
	    else {
		playerController.clearstate();
		//Debug.Log("TileCollisionEffect.OnCollisionEnter() cannot find corresponding tile");
	    }
	}

	public void Orange(PlayerController pc) {
	    pc.jump_coef_w =2;
	    Debug.Log("Orange");
	}

	public void Green(PlayerController pc) {
	    if(pc == null)
	    {
		Debug.Log("TileCollisionEffect.Orange() pc is null");
	    }
	    pc.jump_coef_h =1.5f;
	    Debug.Log("Green");
	}
	public void Alice(PlayerController pc) {
	    pc.isStandonIce = true;
	    Debug.Log("Alice");
	}
    }
}
