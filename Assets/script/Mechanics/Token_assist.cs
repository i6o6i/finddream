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
	float expireTime = 0;
	private LineRenderer m_LineDrawer;

	public TokenAssist(PlayerController pc)
	{
	    m_pc= pc;
	    m_LineDrawer = pc.gameObject.AddComponent<LineRenderer>();
	}
	public bool IsActive()
	{
	    return m_IsActive;
	}
	public void UseEffect()
	{
	    m_IsActive = true;
	    expireTime = Time.time + duration;
	    DrawParabola();
	}
	void DrawParabola()
	{
	    var velocityx =m_pc.jump_coef_w*model.jumpxcoef;
	    velocityx *=m_pc.faceright?1:-1;
	    var velocityy =m_pc.jumpforce;
	    var velocity= new Vector3( velocityx, velocityy, 0);
	    Vector3 pos = m_pc.get_pos();
	    //int step= 100;
	    m_LineDrawer.positionCount = 100;
	    float steptime = model.linesteptime;
	    Debug.Log("TokenAssist.DrawParabola() m_pc.pos = "+pos);
	    for(var i = 0; i < m_LineDrawer.positionCount;i++ )
	    {
		m_LineDrawer.SetPosition(i,pos);
		pos += velocity*steptime+0.5f * Physics.gravity * steptime*steptime;
		Debug.Log("TokenAssist.DrawParabola() i "+i+" pos = "+pos);
	    }
	}
	void ClearParabola()
	{
	    Debug.Log("TokenAssist.ClearParabola() m_pc.velocity = "+m_pc.velocity);
	}
	public void Update()
	{
	    if(m_IsActive)
	    {
		if(Time.time <= expireTime)
		{
		    DrawParabola();
		}
		else{
		    ClearParabola();
		    //m_pc.TokAssist = null;
		}
	    }
	}

    }
}
