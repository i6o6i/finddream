using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace Platformer.Mechanics
{
    [RequireComponent(typeof(Collider2D))]
    public class TokenInstance : MonoBehaviour
    {
	[System.Serializable]
	public class TokenEvent : UnityEvent<PlayerController> { }
	[System.Serializable]
	public struct TokenEffect {
	    public Sprite sprite;
	    public TokenEvent effect;
	}
	public TokenEffect[] effects;
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
	public List<TokenEvent> all_effect;

	// Pack our map of tile effects into a dictionary for ease of lookups.
	private void OnEnable() {
	    if (_effectMap != null)
		return;

	    _effectMap = new Dictionary<Sprite, TokenEvent>(effects.Length);
	    foreach (var entry in effects)
		_effectMap.Add(entry.sprite, entry.effect);
	}
        void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            if (randomAnimationStartTime)
                frame = Random.Range(0, sprites.Length);
            sprites = idleAnimation;
	    //Debug.Log("token is awaken object name"+sprites[frame].name+" sprites.frame is "+frame);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            //only exectue OnPlayerEnter if the player collides with this token.
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null) OnPlayerEnter(player);
        }

        void OnPlayerEnter(PlayerController player)
        {
	    Debug.Log("TokenInstance.OnPlayerEnter() collected = "+collected);
            if (collected) return;
            //disable the gameObject and remove it from the controller update list.
            frame = 0;
            sprites = collectedAnimation;
            if (controller != null)
                collected = true;

	    var sprite = idleAnimation[0];
	    if (_effectMap.TryGetValue(sprite, out TokenEvent effect) && effect != null)
		effect.Invoke(player);
	    else Debug.Log("TokenInstance.OnPlayerEnter() cannot retrieve event handler in _effectMap");
        }
	public void teleport_effect(PlayerController pc)
	{
	    Debug.Log("TokenInstance.teleport_effect() teleport token is collected");
	    pc.teleportTok = new TokenTeleport(pc);

	}

	public void assistance_effect(PlayerController pc)
	{
	    Debug.Log("TokenInstance.assistance_effect() assistance token is collected");
	    pc.TokAssist = new TokenAssist(pc);
	}

	public void down_effect(PlayerController pc)
	{
	    Debug.Log("TokenInstance.down_effect() down token is collected");
	    TokenMoveDown tok = new TokenMoveDown(pc);
	    tok.UseEffect();
	}

	public void up_effect(PlayerController pc)
	{
	    Debug.Log("TokenInstance.up_effect() up token is collected");
	    TokenMoveUp tok = new TokenMoveUp(pc);
	    tok.UseEffect();
	}

	public void randomvert_effect(PlayerController pc)
	{
	    Debug.Log("TokenInstance.randomvert_effect() random verticle move token is collected");
	    TokenMoveVert tok = new TokenMoveVert(pc);
	    tok.m_movestep = Random.Range(-3,4);
	    tok.UseEffect();
	}

	public void random_effect(PlayerController pc)
	{
	    Debug.Log("TokenInstance.random_effect() random effect token is collected");
	    
	    Debug.Log("TokenInstance.random_effect() all_effect.Count = "+all_effect.Count);
	    int idx = Random.Range(0,all_effect.Count);
	    all_effect[idx].Invoke(pc);

	}

    }
}
