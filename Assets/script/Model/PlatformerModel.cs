using Platformer.Mechanics;
using UnityEngine;

namespace Platformer.Model
{
    [System.Serializable]
    public class PlatformerModel
    {
	public PlayerController player;

	public float maxSpeed = 6;
	public float forcestep =0.05F;
	public float maxforce=10;
	public float jumpycoef =1;
	public float jumpxcoef =2;
	public int blockcnt=8;
    }
}