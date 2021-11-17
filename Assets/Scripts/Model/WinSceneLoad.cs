using UnityEngine;
using TMPro;

namespace Platformer.Model
{
    public class WinSceneLoad  : MonoBehaviour
    {
	public string SingleWinText;
	public string MultiplayWinText;
	void Start()
	{
	    TextMeshProUGUI mText = GetComponent<TextMeshProUGUI>();

		Debug.Log("WinSceneLoad.Start()"
			+" SceneData.PlayerName = "+SceneData.PlayerName
			+" SceneData.IsMultiplayer = "+SceneData.IsMultiplayer
			);
	    if(SceneData.IsMultiplayer)
	    {
		mText.text = SceneData.PlayerName+MultiplayWinText;
	    }
	    else
	    {
		mText.text = SingleWinText;
	    }
	    
	}
    }
}
