using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    private new Camera camera;
    private GameManager gameManager;

    public LayerMask ignoreLayer;

    private void Start()
    {
        camera = Camera.main;
        gameManager = GetComponent<GameManager>();
    }

    public void Shoot()
    {
        gameManager.playerController.AimAndShoot(SetTarget());
    }

    private Vector3 SetTarget()
    {
        Vector3 origin = camera.transform.position;
        Vector3 destination = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 50));
        Vector3 direction = destination - origin;

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, 50, ~ignoreLayer))
            return hit.point;
        else
            return destination;
    }
}
