using UnityEngine;
using UnityEngine.Tilemaps;

namespace Platformer.Mechanics
{
    public class TokenMoveUp : TokenMoveVert
    {
	public TokenMoveUp(PlayerController pc):base(pc)
	{

	}
	public override void UseEffect()
	{
	    m_movestep =3;
	    base.UseEffect();

	}
    }

}
