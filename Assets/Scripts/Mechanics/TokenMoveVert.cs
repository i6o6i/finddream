using UnityEngine;
using UnityEngine.Tilemaps;

namespace Platformer.Mechanics
{
    public class TokenMoveVert
    {
	private PlayerController m_pc;
	internal int m_movestep;
	public TokenMoveVert(PlayerController pc)
	{
	    m_pc = pc;
	}
	protected void move()
	{
	    var tilemap=UnityEngine.Object.FindObjectsOfType<TileCollisionEffect>()[0].GetComponent<Tilemap>();
	    var grid = tilemap.layoutGrid;
	    Vector3 pos=m_pc.get_pos();

	    Vector3 gridPosition = grid.transform.InverseTransformPoint(pos);
	    Vector3Int cell;
	    TileBase tile;

	    int cnt =m_movestep>0?m_movestep:-m_movestep;
	    int step = m_movestep >0?1:-1;
	    do{
		gridPosition.y +=step;
		/*
		do{
		    gridPosition.y +=step;
		    cell = grid.LocalToCell(gridPosition);
		    tile = tilemap.GetTile(cell);
		    if(tile!=null)
		    {
			Debug.Log("hit blocks continue gridPosition = "+gridPosition);
		    }
		}while(tile!=null);
		*/
		cnt--;
	    }while(cnt!=0);


	    pos = grid.transform.TransformPoint(gridPosition);

	    m_pc.Teleport(pos);
	    m_pc.m_CameraCtrl.UpdateCam();

	}
	public virtual void UseEffect()
	{
	    move();
	}
    }

}
