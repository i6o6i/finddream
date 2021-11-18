using Platformer.Model;
using Platformer.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics {
    public class PlayerController : KinematicObject
    {
	//readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();
	public string m_PlayerName = "";
	readonly PlatformerModel model = Instance<PlatformerModel>.get();
	public CameraController m_CameraCtrl;
	public bool controlEnabled = true;
	SpriteRenderer spriteRenderer;
	internal bool faceright=true;
	public float jumpforce;
	internal bool m_isheld;
	public Collider2D collider2d;
	private float ground;
	public float jump_coef_h=1;
	public float jump_coef_w=1;
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
	public KeyCode jump_key;
	public KeyCode left_key;
	public KeyCode right_key;
	public KeyCode teleport_key;
	public KeyCode assist_key;
	internal Animator animator;
	internal TokenTeleport teleportTok = null;
	internal TokenAssist TokAssist = null;
	// Start is called before the first frame update

	void OnEnable()
	{
	    base.OnEnable();
	    if(get_pos().y <= 57&&TokAssist == null)
	    {
		TokAssist = new TokenAssist(this);
		TokAssist.isFirstLevel =true;
		TokAssist.UseEffect();
	    }
	}
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
		if(Input.GetKeyDown(right_key))
		{
		    move.x = 1;
		}else if(Input.GetKeyDown(left_key))
		{
		    move.x = -1;
		}else if(Input.GetKey(right_key) == false&&Input.GetKey(left_key) == false)
		{
		    move.x =0;
		}
		if(Input.GetKeyDown(jump_key))
		{
		    m_isheld =true;
		}else if(Input.GetKeyUp(jump_key)) {
		    m_isheld =false;
		}
	    }
	    if(Input.GetKeyDown(teleport_key))
	    {
		//Debug.Log("Teleport Key down");
		if(teleportTok !=null)
		{
		    teleportTok.use_effect();
		}
	    }
	    if(Input.GetKeyDown(assist_key))
	    {
		//Debug.Log("Assist Key down");
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
	    //Debug.Log("PlayerController.UpdateJumpState() Time.deltaTime ="+Time.deltaTime);
	    switch(m_jumpstate)
	    {
		case JumpState.Landed:
		    animator.SetBool("IsHeld",false);
		    if(move.x > 0.01f)
			faceright = true;
		    else if(move.x < -0.01f)
			faceright = false;
		    spriteRenderer.flipX = !faceright;

		    animator.SetFloat("velocityX", Mathf.Abs(move.x));
		    if(move.x==0&&isStandonIce)
		    {
			animator.SetFloat("velocityX", 0);
			var abs = velocity.x>0?velocity.x:-velocity.x;
			var minus = model.icefriction*Time.deltaTime>abs?abs:model.icefriction*Time.deltaTime;
			Debug.Log("PlayerController.UpdateJumpState()"
				+" move.x = "+move.x
				+" model.icefriction*Time.deltaTime = "+model.icefriction*Time.deltaTime
				+" velocity.x = "+velocity.x
				+" minus = "+minus
				);
			velocity.x +=velocity.x>0?-minus:minus;
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
			    jumpforce += jump_coef_h*model.forcestep*Time.deltaTime;
			}
			//Debug.Log("PlayerController.UpdateJumpState() JumpState.PrepareTojump jumpforce = "+jumpforce);
		    } else {
			velocity.x = jump_coef_w*model.maxSpeed*model.jumpxcoef;
			velocity.x *= faceright?1:-1;
			m_jumpstate = JumpState.Jumping;
		    }
		    animator.SetBool("IsHeld",m_isheld);
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
			//m_CameraCtrl.Adjust();
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
