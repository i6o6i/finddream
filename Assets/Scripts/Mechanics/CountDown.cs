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
	private DateTime start_time ;
	private TimeSpan  maxtime = new TimeSpan(0,10,0);
	private TimeSpan OneSec = new TimeSpan(0,0,1);
	public string m_SceneName;
	private TextMeshProUGUI mText;
	private string format = @"mm\:ss";

	void Start()
	{
	    start_time = DateTime.Now;
	}
	void Awake()
	{
	    mText = GetComponent<TextMeshProUGUI>();
	}
	void Update()
	{
	    var timespan = DateTime.Now - start_time;

	    /*
	    Debug.Log("CountDown.Update()"
		    +"deltaTimeSpan = "+deltaTimeSpan.Ticks
		    );
		    */
	    if(TimeSpan.Compare(timespan,maxtime)>=0)
	    {
		SceneData.IsMultiplayer = Instance<PlatformerModel>.get().IsMultiplayer;
		SceneManager.LoadScene(m_SceneName);
	    }else
	    {
		mText.text = timespan.ToString(format);
	    }
	}
    }

}
