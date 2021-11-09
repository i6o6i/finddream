using UnityEngine;

namespace Platformer.Mechanics
{
    public class TokenAndEffect 
    {
	void token_effect(PlayerController player)
	{
	    Debug.Log("TokenAndEffect.token_effect() pos = "+((KinematicObject)player).body.position);
	    player.previous_pos = ((KinematicObject)player).body.position;
	}
	public void transfer(PlayerController pc)
	{
	   Vector3 pos;
	   pos.x = m_pos.x;
	   pos.y = m_pos.y;
	   pc.Teleport(pos);
	}
    }
}
