using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Mechanics
{
    public class GameController :MonoBehaviour
    {
	public static GameController Instance { get; private set; }
	public PlatformerModel model;

	void Start()
	{
	    model = Instance<PlatformerModel>.get();

	}
        void OnEnable()
        {
            Instance = this;
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }
    }
}
