using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace Platformer.Mechanics
{
    [RequireComponent(typeof(Collider2D))]
    public class TokenInstance : MonoBehaviour
    {
	public class TokenEvent : UnityEvent<PlayerController> { }
	[System.Serializable]
	public struct TokenEffect {
	    public Sprite sprite;
	    public TokenEvent effect;
	}
	public TokenEffect effects;
	Dictionary<Sprite, TokenEvent> _effectMap;

        public bool randomAnimationStartTime = false;
        public Sprite[] idleAnimation, collectedAnimation;

        internal Sprite[] sprites = new Sprite[0];

        internal SpriteRenderer _renderer;

        //unique index which is assigned by the TokenController in a scene.
        internal int tokenIndex = -1;
        internal TokenController controller;
        //active frame in animation, updated by the controller.
        internal int frame = 0;
        internal bool collected = false;


	// Pack our map of tile effects into a dictionary for ease of lookups.
	private void OnEnable() {
	    if (_effectMap != null)
		return;

	    _effectMap = new Dictionary<TileBase, CollisionEvent>(effects.Length);
	    foreach (var entry in effects)
		_effectMap.Add(entry.tile, entry.effect);
	}
        void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            if (randomAnimationStartTime)
                frame = Random.Range(0, sprites.Length);
            sprites = idleAnimation;
	    Debug.Log("token is awaken object name"+sprites[frame].name+" sprites.frame is "+frame);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            //only exectue OnPlayerEnter if the player collides with this token.
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null) OnPlayerEnter(player);
        }

        void OnPlayerEnter(PlayerController player)
        {
            if (collected) return;
            //disable the gameObject and remove it from the controller update list.
            frame = 0;
            sprites = collectedAnimation;
            if (controller != null)
                collected = true;

	    if (_effectMap.TryGetValue(tile, out CollisionEvent effect) && effect != null)
		effect.Invoke(playerController);
        }

    }
}
