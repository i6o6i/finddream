using Platformer.Mechanics;
using UnityEngine;

namespace Platformer.Model
{
    [System.Serializable]
    public class PlatformerModel
    {
	public PlayerController player1;
	public PlayerController player2;

	public float maxSpeed = 6;
	public float forcestep =0.05F;
	public float maxforce=10;
	public float jumpycoef =1;
	public float jumpxcoef =2;
	public int blockcnt=8;
	public int iceSpeed = 1;
	public KeyCode teleportKeyCode = KeyCode.T;
	public KeyCode assistKeyCode = KeyCode.Y;
	public float lineDuration = 3.0F;
	public float linesteptime = 0.5F;
    }
}
