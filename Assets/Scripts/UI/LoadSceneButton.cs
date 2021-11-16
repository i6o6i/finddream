using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Platformer.UI
{
    public class LoadSceneButton : MonoBehaviour
    {
	public string SceneName = "";
	void Update()
	{
	    if(EventSystem.
		   current.
		   currentSelectedGameObject == gameObject
		    && Input.GetButtonDown("Submit"))
	    {
		LoadTargetScene();
	    }

	}
	public void LoadTargetScene()
	{
		SceneManager.LoadScene(SceneName);
	}
    }
}
