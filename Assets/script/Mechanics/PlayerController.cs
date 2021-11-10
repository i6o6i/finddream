using Platformer.Model;
using Platformer.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics {
    public class PlayerController : KinematicObject
    {
	//readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();
	readonly PlatformerModel model = Instance<PlatformerModel>.get();
	public CameraController m_CameraCtrl;
	public bool controlEnabled = true;
	SpriteRenderer spriteRenderer;
	public bool faceright=true;
	public float jumpforce;
	public bool m_isheld;
	public Collider2D collider2d;
	private float ground;
	internal float jump_coef_h=1;
	internal float jump_coef_w=1;
	Vector2 move;
	public bool isStandonIce=false;
	public enum JumpState
	{
	    Landed,
	    PrepareTojump,
	    Jumping,
	    InFlight
	};
	public JumpState m_jumpstate = JumpState.Landed;
	internal Animator animator;
	internal TokenTeleport teleportTok = null;
	internal TokenAssist TokAssist = null;
	// Start is called before the first frame update

	void Awake()
	{
	    spriteRenderer = GetComponent<SpriteRenderer>();
	    collider2d = GetComponent<Collider2D>();
	    animator = GetComponent<Animator>();
	    m_CameraCtrl.set_player(this);
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
		}else if(Input.GetKeyUp(KeyCode.Space)) {
		    m_isheld =false;
		}
	    }
	    if(Input.GetKeyDown(model.teleportKeyCode))
	    {
		//Debug.Log("Teleport Key down");
		if(teleportTok !=null)
		{
		    teleportTok.use_effect();
		}
	    }
	    if(Input.GetKeyDown(model.assistKeyCode))
	    {
		//Debug.Log("Teleport Key down");
		if(TokAssist !=null)
		{
		    TokAssist.UseEffect();
		}
	    }
	    UpdateJumpState();
	    if(TokAssist !=null)
	    {
		TokAssist.Update();
	    }
	    base.Update();
	}
	void UpdateGound()
	{
	    ground = collider2d.bounds.min.y;
	}
	void UpdateJumpState()
	{
	    switch(m_jumpstate)
	    {
		case JumpState.Landed:
		    if(move.x > 0.01f)
			faceright = true;
		    else if(move.x < -0.01f)
			faceright = false;
		    spriteRenderer.flipX = !faceright;

		    if(move.x == 0 && isStandonIce)
		    {
			animator.SetFloat("velocityX", 0);
			velocity.x =  velocity.x>0?model.iceSpeed:-model.iceSpeed;
		    }else
		    {
			velocity.x = move.x * model.maxSpeed;
			animator.SetFloat("velocityX", Mathf.Abs(velocity.x));
		    }
		    if(m_isheld)
		    {
			//Debug.Log("PlayerController.UpdateJumpState() JumpState.PrepareTojump m_jumpstate change to PrepareTojump");
			m_jumpstate = JumpState.PrepareTojump;
		    }
		    if(velocity.y<-0.01f)
		    {
			//离开平台自由落体时清空状态
			jump_coef_h = 1;
			jump_coef_w = 1;
			isStandonIce = false;
			m_jumpstate = JumpState.InFlight;
			animator.SetBool("grounded",true);
		    }
		    break;
		case JumpState.PrepareTojump:
		    //Debug.Log("PlayerController.UpdateJumpState() JumpState.PrepareTojump m_isheld = "+m_isheld);
		    if(m_isheld)
		    {
			if(jumpforce < jump_coef_h*model.maxforce)
			{
			    //Debug.Log("PlayerController.UpdateJumpState() JumpState.PrepareTojump jumpforce = "+jumpforce);
			    jumpforce += jump_coef_h*model.forcestep* model.jumpycoef;
			}
			//Debug.Log("PlayerController.UpdateJumpState() JumpState.PrepareTojump jumpforce = "+jumpforce);
		    } else {
			if(faceright)
			    velocity.x = jump_coef_w*model.jumpxcoef;
			else {
			    velocity.x = -jump_coef_w*model.jumpxcoef;
			}
			m_jumpstate = JumpState.Jumping;
		    }
		    break;
		case JumpState.Jumping:
		    velocity.y = jumpforce;
		    //Debug.Log("PlayerController.UpdateJumpState() case JumpState.Jumping: velocity = "+velocity);

		    IsGrounded = false;
		    animator.SetBool("grounded",false);
		    //开始跳跃时清空状态
		    jump_coef_h = 1;
		    jump_coef_w = 1;
		    isStandonIce = false;

		    m_jumpstate = JumpState.InFlight;
		    break;
		case JumpState.InFlight:
		    jumpforce =0;
		    animator.SetBool("grounded",false);
		    //Debug.Log("PlayerController.UpdateJumpState() case JumpState.InFlight: "+velocity+"IsGrounded ="+IsGrounded);
		    if(IsGrounded)
		    {
			m_jumpstate = JumpState.Landed;
			animator.SetBool("grounded",true);
			UpdateGound();
			m_CameraCtrl.MoveToNextScence(this);
			move.x=0;
		    }
		    break;
	    }
	}
	protected override void ComputeVelocity()
	{

	}
    }
}
