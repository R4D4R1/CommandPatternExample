using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{
    public static LevelsManager instance = null;

    [SerializeField] private List<int> numberOfActionsInEachLevelList;
    [SerializeField] private List<GameObject> LevelList;
    [SerializeField] private List<Transform> playerTransformList;
    [SerializeField] private List<GameObject> exitList;
    [SerializeField] private float changeLevelTime = 0;
    [SerializeField] private int currentLevel = 0;

    [SerializeField] private TextMeshProUGUI actionsText;

    [SerializeField] private Transform exitTransform;

    [SerializeField] private UnityEvent pauseMenuEvent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        Destroy(this.gameObject);
    }


    private void Start()
    {
        actionsText.text = "Moves - " + numberOfActionsInEachLevelList[currentLevel].ToString();
    }

    public void ActionNumberDecrease()
    {
        numberOfActionsInEachLevelList[currentLevel] -= 1;
        actionsText.text = "Moves - " + numberOfActionsInEachLevelList[currentLevel].ToString();
    }

    public void ActionNumberIncrease()
    {
        numberOfActionsInEachLevelList[currentLevel] += 1;
        actionsText.text = "Moves - " + numberOfActionsInEachLevelList[currentLevel].ToString();
    }

    public void GameWon()
    {
        if (currentLevel < LevelList.Count - 1) 
        {
            StartCoroutine(ChangeLevel());
        }
        else
        {
            pauseMenuEvent.Invoke();
        }

    }

    IEnumerator ChangeLevel()
    {
        yield return new WaitForSeconds(changeLevelTime);
        LevelList[currentLevel].SetActive(false);
        currentLevel++;
        LevelList[currentLevel].SetActive(true);

        Player.instance.transform.position = playerTransformList[currentLevel].position;
        exitTransform.position = exitList[currentLevel].transform.position;

        actionsText.text = "Moves - " + numberOfActionsInEachLevelList[currentLevel].ToString();
        MovementPlayer.instance.RestartLevel();
        MovementPlayer.instance.ResetPosition();

        ActionRecorder.Instance.ResetACtions();
    }

    public bool MovesLeft()
    {
        if (numberOfActionsInEachLevelList[currentLevel] == 0)
        {
            return false;
        }
        else
            return true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
