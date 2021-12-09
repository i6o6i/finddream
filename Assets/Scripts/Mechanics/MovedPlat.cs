using UnityEngine;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    public class MovedPlat :MonoBehaviour
    {
	private float speed = Instance<PlatformerModel>.get().MovedPlatSpeed;
	private Vector3 direction;

	void OnEnable()
	{
	    direction = new Vector3(1,0,0);
	}
	public void Update()
	{
	    speed = Instance<PlatformerModel>.get().MovedPlatSpeed;
	    Vector3 pos = transform.localPosition;
	    if(pos.x>=5||pos.x<=-4)
	    {
		direction.x = pos.x >0?-1:1;
	    }
	    transform.Translate(direction*speed*Time.deltaTime,Space.Self);
	}
	private void OnTriggerEnter2D(Collider2D collider)
	{
	    Debug.Log("MovedPlat.OnCollisionEnter2D()");
	    var playerController = collider.gameObject.GetComponent<PlayerController>();
	    if(playerController == null)
	    {
		Debug.Log("MovedPlat.OnCollisionEnter2D() playerController is null");
		return;
	    }
	    /*
	    Vector3 speed3D= direction*speed;
	    Vector2 speed2D = new Vector2(speed3D.x,speed3D.y);

	    Debug.Log("MovedPlat.OnCollisionEnter2D() playerController.base_velocity = "+playerController.velocity);
	    playerController.base_velocity +=speed2D;
	    Debug.Log("MovedPlat.OnCollisionEnter2D() playerController.base_velocity = "+playerController.velocity);
	    */
	    collider.gameObject.transform.SetParent(gameObject.transform,true);
	}

    }
}
