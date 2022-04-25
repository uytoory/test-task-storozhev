using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public CharacterController playerController = null;

    [SerializeField] private Stage[] stages = null;
    private int currentStage = 0;
    private int currentKilled = 0;

    [SerializeField] private GameObject startButton = null;
    [SerializeField] private GameObject attackButton = null;

    [SerializeField] private Animator cnavasAnimator = null;

    public static Action OnStageReached;
    public static Action OnEnemyKilled;
    public static Action OnPlayerKilled;

    private void Start()
    {
        OnStageReached = null;
        OnStageReached += ActivateCombat;
        OnStageReached += CheckEnemies;
        
        OnEnemyKilled = null;
        OnEnemyKilled += GetKill;

        OnPlayerKilled = null;
        OnPlayerKilled += Lose;
    }

    public void StartGame()
    {
        CameraController.SwitchCamera(CameraController.CameraState.Combat);
        NextPoint();
        startButton.SetActive(false);
    }

    public void NextPoint()
    {
        currentKilled = 0;
        currentStage++;
        if (currentStage < stages.Length)
            playerController.MoveToPoint(stages[currentStage].movePoint, false);
        else
            Win();
    }

    private void GetKill()
    {
        currentKilled++;
        CheckEnemies();
    }

    private void CheckEnemies()
    {
        if (currentKilled == stages[currentStage].enemies.Length)
            Invoke("NextPoint", 1f);
    }

    private void ActivateCombat()
    {
        foreach (CharacterController enemy in stages[currentStage].enemies)
        {
            enemy.MoveToPoint(playerController.transform, true);
        }
        attackButton.SetActive(true);
    }

    private void Win()
    {
        CameraController.SwitchCamera(CameraController.CameraState.Victory);
        playerController.GetComponent<Animator>().SetBool("Aim", false);
        Invoke("ShowRestartScreen", 2);
    }

    private void Lose()
    {
        CameraController.SwitchCamera(CameraController.CameraState.Death);
        Invoke("ShowRestartScreen", 1);
    }

    private void ShowRestartScreen()
    {
        cnavasAnimator.SetTrigger("ShowRestartScreen");
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }

    [System.Serializable]
    public class Stage
    {
        public Transform movePoint;
        public CharacterController[] enemies;
    }
}
