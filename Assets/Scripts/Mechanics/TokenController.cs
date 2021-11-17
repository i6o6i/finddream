using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace Platformer.Mechanics
{
    [System.Serializable]
    public class TokenEvent : UnityEvent<PlayerController> { }

    [System.Serializable]
    public struct TokenData {
	public Sprite[] idleAnimation;//, collectedAnimation;
	public TokenEvent effect;
    }
    [System.Serializable]
    public struct TokenDataKV {
	public Sprite sprite;
	public TokenData tokenData;
    }

    public class TokenController : MonoBehaviour
    {
        public float frameRate = 12;
        public TokenInstance[] tokens;
	public TokenDataKV[] tokenDataKV;
	public Dictionary<Sprite, TokenData> _tokenDatas;

        float nextFrameTime = 0;
	internal List<TokenEvent> all_effect;

        void FindAllTokensInScene()
        {
            tokens = UnityEngine.Object.FindObjectsOfType<TokenInstance>();
	    Debug.Log("TokenController.FindAllTokensInScene()"
		    +" tokens.Length = "+tokens.Length
		    );
        }

	void OnEnable()
	{
	    if(_tokenDatas!=null)
		return;

	    _tokenDatas = new Dictionary<Sprite, TokenData>(tokenDataKV.Length);
	    foreach (var entry in tokenDataKV)
		_tokenDatas.Add(entry.sprite, entry.tokenData);
	}

        void Awake()
        {
            //if tokens are empty, find all instances.
            //if tokens are not empty, they've been added at editor time.
            if (tokens.Length == 0)
                FindAllTokensInScene();
            //Register all tokens so they can work with this controller.
	    
	    if(_tokenDatas==null)
	    {
		_tokenDatas = new Dictionary<Sprite, TokenData>(tokenDataKV.Length);
		foreach (var entry in tokenDataKV)
		    _tokenDatas.Add(entry.sprite, entry.tokenData);
	    }
	    all_effect = new List<TokenEvent>();
	    Debug.Log("TokenController.Awake() _tokenDatas.Count = "+_tokenDatas.Count);
	    foreach(KeyValuePair<Sprite, TokenData> entry in _tokenDatas)
	    {
		all_effect.Add(entry.Value.effect);
	    }
	    Debug.Log("TokenController.Awake() tokens.Length = "+tokens.Length);
            for (var i = 0; i < tokens.Length; i++)
            {
                tokens[i].tokenIndex = i;
                tokens[i].controller = this;
		Sprite sprite = tokens[i].GetComponent<SpriteRenderer>().sprite;
		if (_tokenDatas.TryGetValue(sprite, out TokenData tokenData))
		{
		    tokens[i].idleAnimation = tokenData.idleAnimation;
		    tokens[i].effect = tokenData.effect;
		}
            }
        }

        void Update()
        {
            if (Time.time - nextFrameTime > (1f / frameRate))
            {
                for (var i = 0; i < tokens.Length; i++)
                {
                    var token = tokens[i];
                    if (token != null)
                    {
			//Debug.Log("tokens.index is "+ i +" tokens[index].frame is "+ token.frame);
                        token._renderer.sprite = token.sprites[token.frame];
                        if (token.collected && token.frame == token.sprites.Length - 1)
                        {
                            token.gameObject.SetActive(false);
                            tokens[i] = null;
                        }
                        else
                        {
                            token.frame = (token.frame + 1) % token.sprites.Length;
			    //Debug.Log("tokens.index is "+ i +"after ifelse branch tokens[index].frame is "+ token.frame);
                        }
                    }
                }
                nextFrameTime += 1f / frameRate;
            }
        }
	public void teleport_effect(PlayerController pc)
	{
	    Debug.Log("TokenController.teleport_effect() teleport token is collected");
	    pc.teleportTok = new TokenTeleport(pc);

	}

	public void assistance_effect(PlayerController pc)
	{
	    Debug.Log("TokenController.assistance_effect() assistance token is collected");
	    pc.TokAssist = new TokenAssist(pc);
	}

	public void down_effect(PlayerController pc)
	{
	    Debug.Log("TokenController.down_effect() down token is collected");
	    TokenMoveDown tok = new TokenMoveDown(pc);
	    tok.UseEffect();
	}

	public void up_effect(PlayerController pc)
	{
	    Debug.Log("TokenController.up_effect() up token is collected");
	    TokenMoveUp tok = new TokenMoveUp(pc);
	    tok.UseEffect();
	}

	public void randomvert_effect(PlayerController pc)
	{
	    Debug.Log("TokenController.randomvert_effect() random verticle move token is collected");
	    TokenMoveVert tok = new TokenMoveVert(pc);
	    List<int> distri = new List<int>();
	    int range = 3;
	    for(int i=-range;i<range+1;i++)
	    {
		if(i!=0)
		{
		    distri.Add(i);
		}
	    }
	    tok.m_movestep = distri[Random.Range(0,distri.Count)];
	    Debug.Log("TokenController.randomvert_effect() tok.m_movestep = "+tok.m_movestep);
	    tok.UseEffect();
	}

	public void random_effect(PlayerController pc)
	{
	    Debug.Log("TokenController.random_effect() random effect token is collected");
	    
	    Debug.Log("TokenController.random_effect() all_effect.Count = "+all_effect.Count);
	    int idx = Random.Range(0,all_effect.Count);
	    all_effect[idx].Invoke(pc);

	}

    }
}
