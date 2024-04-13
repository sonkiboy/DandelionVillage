using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour
{
    #region Object and Components

    private PolygonCollider2D containterCollider;

    private GameObject virtualCam;
    private CinemachineConfiner cameraConfiner;

    #endregion 

    private void Awake()
    {
        virtualCam = GameObject.Find("Virtual Camera");
        cameraConfiner = virtualCam.GetComponent<CinemachineConfiner>();

        containterCollider = GetComponent<PolygonCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Trigger hit");

        cameraConfiner.m_BoundingShape2D = containterCollider;
        collision.gameObject.GetComponent<PlayerController>().MoveToRoom();
        
    }
}
