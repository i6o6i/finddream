using UnityEngine;
using UnityEngine.UI;
using Platformer.Mechanics;

namespace Platformer.UI
{
    public class SliderController :MonoBehaviour
    {
	public PlayerController m_pc;
	public float maxheight;
	private Slider m_slider;
	void Awake()
	{
	    m_slider = GetComponent<Slider>();

	}
	void Update()
	{
	    if(m_pc.IsGrounded)
	    {
		float persent = m_pc.get_pos().y/maxheight;
		m_slider.value = persent;
	    }
	}
    }
}
