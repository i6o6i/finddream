using UnityEngine;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    public class TokenAssist 
    {
	private PlayerController m_pc;
	private bool m_IsActive;
	readonly float duration = Instance<PlatformerModel>.get().lineDuration;
	readonly PlatformerModel model = Instance<PlatformerModel>.get();
	private LineRenderer m_LineDrawer;
	internal float m_Timesum=0;
	internal bool isFirstLevel=false;

	public TokenAssist(PlayerController pc)
	{
	    m_pc= pc;
	    m_LineDrawer = pc.gameObject.GetComponent<LineRenderer>();
	    m_LineDrawer.startColor = Color.white;
	    m_LineDrawer.endColor = Color.white;
	    m_LineDrawer.startWidth = 0.046f;
	    m_LineDrawer.endWidth = 0.2f;
	    //Material newMat = Resources.Load("unity_builtin_extra/Default-Particle.mat", typeof(Material)) as Material;
	    m_LineDrawer.material =  new Material(Shader.Find("Sprites/Default"));
	    m_Timesum=0;
	}
	public bool IsActive()
	{
	    return m_IsActive;
	}
	public void UseEffect()
	{
	    m_IsActive = true;
	    //DrawParabola();
	}
	void DrawParabola()
	{
	    var velocityx =m_pc.jump_coef_w*model.maxSpeed*model.jumpxcoef;
	    velocityx *=m_pc.faceright?1:-1;
	    var velocityy =m_pc.jumpforce;
	    if(velocityy <=0)
		return ;
	    var velocity= new Vector3( velocityx, velocityy, 0);

	    Vector3 pos = m_pc.get_pos();
	    Debug.Log("TokenAssist.DrawParabola() m_pc.pos = "+pos+" velocity = "+ velocity);

	    float steptime = model.linesteptime;
	    m_LineDrawer.positionCount = 80;
	    var line_pos = pos;
	    for(var i = 0; i < m_LineDrawer.positionCount;i++ )
	    {
		m_LineDrawer.SetPosition(i,line_pos);
		line_pos = pos+velocity*steptime+0.5f * Physics.gravity * steptime*steptime;
		steptime +=model.linesteptime;
		//Debug.Log("TokenAssist.DrawParabola() i "+i+" pos = "+pos);
	    }
	}
	void ClearParabola()
	{
	    m_LineDrawer.positionCount =0;
	    Debug.Log("TokenAssist.ClearParabola() m_pc.velocity = "+m_pc.velocity);
	}
	public void Update()
	{
	    if(m_IsActive)
	    {
		if(m_pc.m_isheld)
		{
		    if(m_pc.get_pos().y >57f)
		    {
			m_Timesum += Time.deltaTime;
		    }
		    if(m_Timesum <= duration)
		    {
			DrawParabola();
		    }
		    else {
			if(m_pc.get_pos().y > 57f)
			{
			    ClearParabola();
			    m_pc.TokAssist = null;
			}
		    }
		}else if(m_pc.IsGrounded==false)
		{
		    ClearParabola();
		    if(m_pc.get_pos().y > 57f)
		    {
			m_pc.TokAssist = null;
		    }
		}
	    }
	}

    }
}
