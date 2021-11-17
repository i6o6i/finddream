using UnityEngine.SceneManagement;
using UnityEngine;
using Platformer.Model;

namespace Platformer.Mechanics
{
    [RequireComponent(typeof(Collider2D))]
    public class VictoryZone : MonoBehaviour
    {
	public string m_SceneName;
	public bool IsMultiplayer=false;

        void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null) OnPlayerEnter(player);
        }

	void OnPlayerEnter(PlayerController pc)
	{
	    Debug.Log("VictoryZone.OnPlayerEnter()"
		    +" pc.m_PlayerName = "+pc.m_PlayerName
		    +" IsMultiplayer = "+IsMultiplayer
		    );
	    if(IsMultiplayer)
	    {
		SceneData.PlayerName = pc.m_PlayerName;
	    }
	    else
	    {
		SceneData.PlayerName = "";
	    }
	    SceneData.IsMultiplayer = IsMultiplayer;
	    SceneManager.LoadScene(m_SceneName);
	}
    }
}
