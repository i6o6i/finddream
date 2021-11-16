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
	public int blockcnt=14;
	public int iceSpeed = 1;
	public KeyCode player1jump = KeyCode.Space;
	public KeyCode player1moveleft = KeyCode.A;
	public KeyCode player1moveright = KeyCode.D;
	public KeyCode player1assistKeyCode = KeyCode.Y;
	public KeyCode player1teleportKeyCode = KeyCode.T;
	public float lineDuration = 2.0F;
	public float linesteptime = 0.02F;
	public KeyCode player2jump = KeyCode.Keypad0;
	public KeyCode player2moveleft = KeyCode.LeftArrow;
	public KeyCode player2moveright = KeyCode.RightArrow;
	public KeyCode player2assistKeyCode = KeyCode.Keypad1;
	public KeyCode player2teleportKeyCode = KeyCode.Keypad2;
    }
}
