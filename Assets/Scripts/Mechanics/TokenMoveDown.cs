using UnityEngine;
using UnityEngine.Tilemaps;

namespace Platformer.Mechanics
{
    public class TokenMoveDown : TokenMoveVert
    {
	public TokenMoveDown(PlayerController pc):base(pc)
	{

	}
	public override void UseEffect()
	{
	    m_movestep =-3;
	    base.UseEffect();
	}
    }

}
