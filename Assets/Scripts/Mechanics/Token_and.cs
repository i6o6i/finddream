using UnityEngine;

namespace Platformer.Mechanics
{
    public class TokenTeleport 
    {
	private PlayerController m_pc;
	private Vector3 m_pos;
	public TokenTeleport(PlayerController pc)
	{
	    m_pc = pc;
	    m_pos = pc.get_pos();
	}
	public void use_effect()
	{
	    Debug.Log("TokenAndEffect.use_effect() pc.and_effect.m_pos= "+m_pos);
	   m_pc.Teleport(m_pos);
	   m_pc.teleportTok = null;
	}
    }
}
