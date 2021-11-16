using UnityEngine;

namespace Platformer.Mechanics
{
    public class TokenController : MonoBehaviour
    {
        public float frameRate = 12;
        public TokenInstance[] tokens;

        float nextFrameTime = 0;

        void FindAllTokensInScene()
        {
            tokens = UnityEngine.Object.FindObjectsOfType<TokenInstance>();
	    Debug.Log("TokenController.FindAllTokensInScene()"
		    +" tokens.Length = "+tokens.Length
		    );
        }

        void Awake()
        {
            //if tokens are empty, find all instances.
            //if tokens are not empty, they've been added at editor time.
            if (tokens.Length == 0)
                FindAllTokensInScene();
            //Register all tokens so they can work with this controller.
            for (var i = 0; i < tokens.Length; i++)
            {
                tokens[i].tokenIndex = i;
                tokens[i].controller = this;
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

    }
}
