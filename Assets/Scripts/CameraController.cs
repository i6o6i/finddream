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
    public float frameRate=12;
    Vector2 trans;
    private bool adjusted = true;
    private float nextFrameTime =0;
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

    void Update()
    {
	UpdateCam();
	if(m_pc.IsGrounded == false)
	{
	    adjusted = false;
	}
	if (Time.time - nextFrameTime > (1f / frameRate)&&(trans.x!=0||trans.y!=0))
	{
	    Debug.Log("CameraController.Update()"
		    +" Time.time = "+Time.time
		    +" nextFrameTime = "+nextFrameTime
		    );
	    gameObject.transform.Translate(trans);
	    trans = new Vector2(0,0);
	    nextFrameTime += 1f / frameRate;
	}
    }
    public void UpdateCam()
    {
	int i=IsExitedCamera();
	if(i!=0)
	{
	    MoveToNextScence(i);
	}

    }

    public int IsExitedCamera()
    {
	//Debug.Log("CameraController.IsExitedCamera");
	float camerabuttom = gameObject.transform.position.y - m_OrthographicCamera.orthographicSize;
	float cameratop = gameObject.transform.position.y + m_OrthographicCamera.orthographicSize;
	Vector2 buttomleft = m_pc.collider2d.bounds.min;
	Vector2 topright = m_pc.collider2d.bounds.max;
	/*
	Debug.Log("CameraController.IsExitedCamera()"
		+" buttomleft = "+buttomleft
		+" camerabuttom = "+camerabuttom
		);
	*/
	if(buttomleft.y > cameratop) 
	{
	    return 1;
	}else if(topright.y < camerabuttom)
	{
	    return -1;
	}else {
	    return 0;
	}
    }
    public void MoveToNextScence(int i)
    {
	Debug.Log("CameraController.MoveToNextScence() i = "+ i);
	float distance = 1 * m_OrthographicCamera.orthographicSize;
	trans= new Vector3(0,i*distance,0);
	adjusted = false;
    }
    public void Adjust()
    {
	Debug.Log("CameraController.Adjust()"
		+" adjusted = "+ adjusted
		);
	if(adjusted == false)
	{
	    float camerabuttom = gameObject.transform.position.y - m_OrthographicCamera.orthographicSize;
	    Vector2 buttomleft = m_pc.collider2d.bounds.min;
	    trans.x = 0;
	    trans.y = buttomleft.y - camerabuttom - 2*0.618f;
	    Debug.Log("CameraController.Adjust() adjusted = "+adjusted);
	    adjusted = true;
	}
    }
}
