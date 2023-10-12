using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject destination;

    [SerializeField]
    private GameObject gameEndUI;

    [SerializeField]
    private GameObject enemy;
    private Vector3 instantiatePosition;
    private int enemyCount;

    private void Start()
    {
        enemyCount = (int)Mathf.Sqrt(MazeGenerator.instance._mazeWidth * MazeGenerator.instance._mazeDepth) / 2;

        for (int i = 0; i <= enemyCount; i++)
        {
            instantiatePosition = new Vector3(Random.Range(3, MazeGenerator.instance._mazeWidth),
                0, Random.Range(3, MazeGenerator.instance._mazeDepth));
            Instantiate(enemy, instantiatePosition, Quaternion.identity);
        }


    }

    private void Update()
    {
        if (destination.activeSelf == false)
        {
            Time.timeScale = 0;
            PlayerController.instance.canLook = false;
            Cursor.lockState = CursorLockMode.None;
            gameEndUI.SetActive(true);
        }
    }


}
