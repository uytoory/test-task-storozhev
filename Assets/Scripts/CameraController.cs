using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera menuCamera = null;
    [SerializeField] private CinemachineVirtualCamera combatCamera = null;
    [SerializeField] private CinemachineVirtualCamera deathCamera = null;
    [SerializeField] private CinemachineVirtualCamera victoryCamera = null;

    private static Dictionary<CameraState, CinemachineVirtualCamera> _allCameras;

    public enum CameraState
    {
        MainMenu,
        Combat,
        Death,
        Victory
    }

    private void Start()
    {
        _allCameras = new Dictionary<CameraState, CinemachineVirtualCamera>();
        _allCameras.Add(CameraState.MainMenu, menuCamera);
        _allCameras.Add(CameraState.Combat, combatCamera);
        _allCameras.Add(CameraState.Death, deathCamera);
        _allCameras.Add(CameraState.Victory, victoryCamera);
    }

    public static void SwitchCamera(CameraState cameraState)
    {
        foreach (CameraState state in _allCameras.Keys)
        {
            if (state == cameraState)
                _allCameras[state].Priority =  1;
            else
                _allCameras[state].Priority = 0;
        }
    }
}
