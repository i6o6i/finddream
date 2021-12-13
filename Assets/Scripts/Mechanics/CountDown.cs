using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using Platformer.Core;
using Platformer.Model;

namespace Platformer.Mechanics
{
    public class CountDown :MonoBehaviour
    {
	private TimeSpan  time = new TimeSpan(0,2,0);
	private TimeSpan OneSec = new TimeSpan(0,0,1);
	public string m_SceneName;
	private TextMeshProUGUI mText;
	private string format = @"mm\:ss";

	void Awake()
	{
	    mText = GetComponent<TextMeshProUGUI>();
	}
	void Update()
	{
	    var deltaTimeSpan = TimeSpan.FromTicks((long)(Time.deltaTime* TimeSpan.TicksPerSecond));

	    /*
	    Debug.Log("CountDown.Update()"
		    +"deltaTimeSpan = "+deltaTimeSpan.Ticks
		    );
		    */
	    time = time - deltaTimeSpan;
	    if(TimeSpan.Compare(time,TimeSpan.Zero)<=0)
	    {
		SceneData.IsMultiplayer = Instance<PlatformerModel>.get().IsMultiplayer;
		SceneManager.LoadScene(m_SceneName);
	    }else
	    {
		mText.text = time.ToString(format);
	    }
	}
    }

}
