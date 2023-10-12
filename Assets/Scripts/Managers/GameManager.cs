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
    public List<Vector3> enemyInstantiatePositions;
    private Vector3 destinationInstantiatePosition;
    private int enemyCount;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        enemyCount = (int)Mathf.Sqrt(MazeGenerator.instance._mazeWidth * MazeGenerator.instance._mazeDepth) / 2;
        destinationInstantiatePosition = new Vector3((int)MazeGenerator.instance._mazeWidth / 2, 1f, (int)MazeGenerator.instance._mazeDepth / 2);

        Instantiate(destination, destinationInstantiatePosition, Quaternion.identity);
        for (int i = 0; i <= enemyCount; i++)
        {
            enemyInstantiatePositions.Add(new Vector3(Random.Range(3, MazeGenerator.instance._mazeWidth),
                0, Random.Range(3, MazeGenerator.instance._mazeDepth)));
            if (enemyInstantiatePositions[i] != destinationInstantiatePosition)
            {
                Instantiate(enemy, enemyInstantiatePositions[i], Quaternion.identity);
            }
        }
    }

    public void GameEnd()
    {
        Time.timeScale = 0;
        PlayerController.instance.canLook = false;
        Cursor.lockState = CursorLockMode.None;
        gameEndUI.SetActive(true);
    }
}
