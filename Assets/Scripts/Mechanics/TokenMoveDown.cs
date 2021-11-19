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
	    m_movestep =-4;
	    base.UseEffect();
	}
    }

}
