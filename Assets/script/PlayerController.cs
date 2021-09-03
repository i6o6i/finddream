using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : KinematicObject
{
    public float maxSpeed = 6;
    public float forcestep =1;
    public float maxforce=6;
    public int jumpycoef =1;
    public int jumpxcoef =1;
    public bool controlEnabled = true;
    SpriteRenderer spriteRenderer;
    public bool faceright=true;
    public float jumpforce;
    public bool m_isheld;
    public Collider2D conllider2d;
    Vector2 move;
    public enum JumpState
    {
	Landed,
	PrepareTojump,
	Jumping,
	InFlight
    };
    public JumpState m_jumpstate = JumpState.Landed;
    internal Animator animator;
    // Start is called before the first frame update

    void Awake()
    {
	spriteRenderer = GetComponent<SpriteRenderer>();
	conllider2d = GetComponent<Collider2D>();
	animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update()
    {
	move.y = 0;
	if(controlEnabled)
	{
	    move.x = Input.GetAxis("Horizontal");
	    if(Input.GetKeyDown(KeyCode.Space))
	    {
		m_isheld =true;
		Debug.Log("PlayerController.Update() m_isheld = true");
	    }else if(Input.GetKeyUp(KeyCode.Space)) {
		m_isheld =false;
		Debug.Log("PlayerController.Update() m_isheld = false");
	    }
	}
	UpdateJumpState();
	base.Update();
    }
    void UpdateJumpState()
    {
	switch(m_jumpstate)
	{
	    case JumpState.Landed:
		if(m_isheld)
		{
		    //Debug.Log("PlayerController.UpdateJumpState() JumpState.PrepareTojump m_jumpstate change to PrepareTojump");
		    m_jumpstate = JumpState.PrepareTojump;
		}
		break;
	    case JumpState.PrepareTojump:
		    //Debug.Log("PlayerController.UpdateJumpState() JumpState.PrepareTojump m_isheld = "+m_isheld);
		if(m_isheld)
		{
		    if(jumpforce < maxforce)
			jumpforce += forcestep;
		    //Debug.Log("PlayerController.UpdateJumpState() JumpState.PrepareTojump jumpforce = "+jumpforce);
		} else {
		    if(faceright)
			move.x = 1;
		    else {
			move.x = -1;
		    }
		    m_jumpstate = JumpState.Jumping;
		}
		break;
	    case JumpState.Jumping:
		m_jumpstate = JumpState.InFlight;
		IsGrounded = false;
		break;
	    case JumpState.InFlight:
		jumpforce =0;
		if(IsGrounded)
		{
		    m_jumpstate = JumpState.Landed;
		    move.x=0;
		}
		break;
	}
    }
    protected override void ComputeVelocity()
    {
	if(move.x > 0.01f)
	    faceright = true;
	else if(move.x < -0.01f)
	    faceright = false;
	spriteRenderer.flipX = !faceright;
	
	if(velocity.x !=0)
	{
	    Debug.Log("PlayerController.ComputeVelocity() m_jumpstate = " +m_jumpstate);
	    Debug.Log("PlayerController.ComputeVelocity() velocity.x = "+velocity.x);
	}
	animator.SetFloat("velocityX", Mathf.Abs(velocity.x));
	//animator.SetFloat("grouded", Mathf.Abs(velocity.x));
	if(m_jumpstate == JumpState.Landed)
	    velocity.x = move.x * maxSpeed;
	if(m_jumpstate == JumpState.Jumping)
	{
	    velocity.x += move.x * jumpxcoef;
	    velocity.y += jumpforce * jumpycoef;
	    //Debug.Log("PlayerController.ComputeVelocity() velocity.y = "+velocity.y);
	    //Debug.Log("PlayerController.ComputeVelocity() Time.deltaTime"+Time.deltaTime);
	}
    }
}
