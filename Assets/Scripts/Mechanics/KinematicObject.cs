using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics
{
    public class KinematicObject : MonoBehaviour
    {
        public float minGroundNormalY = .65f;
        public float gravityModifier = 1f;
        public Vector2 velocity;
        public bool IsGrounded { get; set; }

        protected Vector2 targetVelocity;
        protected Vector2 groundNormal;
        protected Rigidbody2D body;
	//public Collider2D collider2d;
        protected ContactFilter2D contactFilter;
        protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

        protected const float minMoveDistance = 0.001f;
        protected const float shellRadius = 0.01f;

        public void Teleport(Vector3 position)
        {
            body.position = position;
            velocity *= 0;
            body.velocity *= 0;
        }
	public Vector3 get_pos()
	{
	    Vector3 pos;
	    pos = body.position;
	    return pos;
	}

        protected virtual void OnEnable()
        {
            body = GetComponent<Rigidbody2D>();
	    //collider2d = GetComponent<Collider2D>();
            body.isKinematic = true;
        }

        protected virtual void OnDisable()
        {
            body.isKinematic = false;
        }

        protected virtual void Start()
        {
            contactFilter.useTriggers = false;
            contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
            contactFilter.useLayerMask = true;
        }

        protected virtual void Update()
        {
            //targetVelocity = Vector2.zero;
            ComputeVelocity();
        }

        protected virtual void ComputeVelocity()
        {

        }

        protected virtual void FixedUpdate()
        {
	    //Debug.Log("KinematicObject.FixedUpdate() before velocity.y modified"+velocity);
            if (velocity.y < 0)
                velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
            else
                velocity += Physics2D.gravity * Time.deltaTime;

            //velocity.x = targetVelocity.x;
	    //Debug.Log("KinematicObject.FixedUpdate()"+velocity);

            IsGrounded = false;

            var deltaPosition = velocity * Time.deltaTime;
	    //Debug.Log("KinematicObject.FixedUpdate() Time.deltaTime = "+Time.deltaTime);

            var moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

            var move = moveAlongGround * deltaPosition.x;

            PerformMovement(move, false);

            move = Vector2.up * deltaPosition.y;

            PerformMovement(move, true);

        }

        void PerformMovement(Vector2 move, bool yMovement)
        {
            var distance = move.magnitude;

            if (distance > minMoveDistance)
            {
                var count = body.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
                for (var i = 0; i < count; i++)
                {
                    var currentNormal = hitBuffer[i].normal;

                    if (currentNormal.y > minGroundNormalY)
                    {
                        IsGrounded = true;
                        if (yMovement)
                        {
                            groundNormal = currentNormal;
                            currentNormal.x = 0;
                        }
                    }
                    if (IsGrounded)
                    {
                        var projection = Vector2.Dot(velocity, currentNormal);
                        if (projection < 0)
                        {
                            velocity = velocity - projection * currentNormal;
                        }
                    }
                    else
                    {
			//Debug.Log("KinematicObject.PerformMovement() branch IsGrounded == false"+velocity);
			var projection=Vector2.Dot(velocity,currentNormal);
			velocity = velocity - 2*projection*currentNormal;
                    }
                    var modifiedDistance = hitBuffer[i].distance - shellRadius;
		    Debug.Log("KinematicObject.PerformMovement()"
			    +" modifiedDistance = "+modifiedDistance
			    +" distance = "+distance
			    +" hitBuffer[i].distance = "+hitBuffer[i].distance
			    +" velocity = "+velocity
			    +" move.normalized = "+move.normalized
			    );
		    //hitBuffer[i].collider.gameObject.GetComponent<MonoBehaviour>().OnTriggerEnter2D(collider2d);
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
		    Debug.Log("KinematicObject.PerformMovement()"
			    +" distance = "+distance
			    );
                }
            }else
	    {
		Debug.Log("KinematicObject.PerformMovement() distance <= minMoveDistance distance = " + distance);
	    }
	    
            body.position = body.position + move.normalized * distance;
        }

    }
}
