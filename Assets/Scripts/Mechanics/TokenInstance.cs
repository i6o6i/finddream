using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace Platformer.Mechanics
{

    [RequireComponent(typeof(Collider2D))]
    public class TokenInstance : MonoBehaviour
    {
	public TokenEvent effect;

        public bool randomAnimationStartTime = false;
        public Sprite[] idleAnimation;//, collectedAnimation;

        internal Sprite[] sprites = new Sprite[0];

        internal SpriteRenderer _renderer;

        internal int tokenIndex = -1;
        internal TokenController controller;
        internal int frame = 0;
        internal bool collected = false;

	/*
	private void OnEnable() {
	    if (_effectMap != null)
		return;

	    _effectMap = new Dictionary<Sprite, TokenEvent>(effects.Length);
	    foreach (var entry in effects)
		_effectMap.Add(entry.sprite, entry.effect);
	}
	*/
        void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            if (randomAnimationStartTime)
                frame = Random.Range(0, sprites.Length);
            sprites = idleAnimation;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null) OnPlayerEnter(player);
        }

        void OnPlayerEnter(PlayerController player)
        {
	    Debug.Log("TokenInstance.OnPlayerEnter() collected = "+collected);
            if (collected) return;
            frame = 0;
            //sprites = collectedAnimation;
            if (controller != null)
                collected = true;

	    effect.Invoke(player);
        }

    }
}
