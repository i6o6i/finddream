using UnityEngine;
using System.Collections.Generic;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    public class MovedPlat :MonoBehaviour
    {
	internal Vector3 velocity;
	internal List<MovingWithPlat> moves;
	internal int idx;

	void OnEnable()
	{
	    moves = new List<MovingWithPlat>();
	    idx = 0;
	}
	void Awake()
	{
	    velocity = Instance<PlatformerModel>.get().MovedPlatSpeed * Vector3.right;
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
	public void Update()
	{
	    transform.Translate((Vector3)velocity*Time.deltaTime,Space.Self);
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
	    var collider = collision.collider;
	    Debug.Log("MovedPlat.OnCollisionEnter2D() collider.gameObject.tag = " + collider.gameObject.tag);
	    if(collider.gameObject.tag == "blocks")
	    {
		Debug.Log("MovedPlat.OnCollisionEnter2D() collided with blocks");
		velocity *=-1;
		foreach(MovingWithPlat move in moves)
		{
		    move.velocity =velocity;
		}
	    }else if(collider.gameObject.tag == "Player")
	    {
		var playerController = collider.gameObject.GetComponent<PlayerController>();
		var points = new ContactPoint2D[2];
		int contact_point_num = collider.GetContacts(points);
		DrawCross(points[0].point);
		Debug.Log("MovedPlat.OnTriggerEnter2D()"
			+" contact.normal = "+points[0].normal
			+" contact.points.Count = "+contact_point_num
			+" playerController.IsGrounded = "+playerController.IsGrounded
			);
		if(points[0].normal.y>=0&&playerController.IsGrounded)
		{
		    var MovingWithPlat = collider.GetComponent<Collider2D>().gameObject.GetComponent<MovingWithPlat>();
		    MovingWithPlat.idx = moves.Count;
		    MovingWithPlat.velocity = velocity;
		    moves.Add(MovingWithPlat);
		}
		playerController.clearstate();

	    }
	    
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
	    var collider  = collision.collider;
	    if(collider.gameObject.tag == "Player")
	    {
		    var MovingWithPlat = collider.GetComponent<Collider2D>().gameObject.GetComponent<MovingWithPlat>();
		    MovingWithPlat.velocity *=0;
		    moves.RemoveAt(MovingWithPlat.idx);
	    }
	}

    }
}
