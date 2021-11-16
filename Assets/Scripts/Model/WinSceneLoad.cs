using UnityEngine;
using TMPro;

namespace Platformer.Model
{
    public class WinSceneLoad  : MonoBehaviour
    {
	void Start()
	{
	    TextMeshPro mText = gameObject.GetComponent<TextMeshPro>();

	    mText.text = SceneData.PlayerName+"胜利！";
	    
	}
    }
}
