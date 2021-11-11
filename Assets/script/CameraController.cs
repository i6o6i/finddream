using Platformer.Mechanics;
using Platformer.Model;
using Platformer.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float m_viewpositionx,m_viewpositiony, m_viewwidth, m_viewheight;
    public Camera m_OrthographicCamera;
    readonly int blockcnt=Instance<PlatformerModel>.get().blockcnt;
    private PlayerController m_pc;
    private float aspect;
    private float size;
    // Start is called before the first frame update
    void initialcamera()
    {
	m_OrthographicCamera = GetComponent<Camera>();
	Debug.Log("Aspect is "+m_OrthographicCamera.aspect);
	aspect = m_OrthographicCamera.aspect;
	m_OrthographicCamera.orthographicSize = blockcnt * 1/aspect/2;
	size = m_OrthographicCamera.orthographicSize;
	gameObject.transform.Translate(-gameObject.transform.position);
	gameObject.transform.Translate(new Vector3(0,size,-1));
    }
    void Start()
    {
	initialcamera();
    }
    public void set_player(PlayerController pc)
    {
	m_pc = pc;
    }

    // Update is called once per frame
    void Update()
    {
	if(IsExitedCamera())
	{
	    MoveToNextScence();
	}
    }

    public bool IsExitedCamera()
    {
	//Debug.Log("CameraController.IsExitedCamera");
	float camerabuttom = gameObject.transform.position.y - m_OrthographicCamera.orthographicSize;
	float cameratop = gameObject.transform.position.y + m_OrthographicCamera.orthographicSize;
	Vector2 buttomleft = m_pc.collider2d.bounds.min;
	Vector2 topright = m_pc.collider2d.bounds.max;
	if(buttomleft.y <= camerabuttom || topright.y >= cameratop)
	{
	    return false;
	}
	return true;
    }
    public void MoveToNextScence()
    {
	//Debug.Log("CameraController.MoveToNextScence");
	float camerabuttom = gameObject.transform.position.y - m_OrthographicCamera.orthographicSize;
	Vector2 buttomleft = m_pc.collider2d.bounds.min;
	Vector3 translation = new Vector3(0,0,0);
	translation.x = 0;
	translation.y = buttomleft.y - camerabuttom -1;
	gameObject.transform.Translate(translation);
    }
}
