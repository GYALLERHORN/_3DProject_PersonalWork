using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private MazeCell _mazeCellPrefab;

    [Range(3, 15)]
    public int _mazeWidth;

    [Range(3, 15)]
    public int _mazeDepth;

    private MazeCell[,] _mazeGrid;

    public static MazeGenerator instance;

    private void Awake()
    {
        instance = this;
    }

    IEnumerator Start() // _mazeWidth, _mazeDepth�� ũ�⸸ŭ �̷� �� ���
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x,z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }

        yield return GenerateMaze(null, _mazeGrid[0, 0]); // ������ = null, ���缿 = [0,0]���� ���� GenerateMaze����
    }

    private IEnumerator GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit(); // ���� ��ġ�� �� ó�� ȣ��
        ClearWalls(previousCell, currentCell); // 

        yield return new WaitForSeconds(0.005f); // 0.005�� ��ٷȴٰ� ����

        MazeCell nextCell; // currentCell -> nextCell�� �� ���� ����

        do // ó�� �ѹ��� ������ �ؾ��ϴϱ� do�� �ݺ�
        {
            nextCell = GetNextUnvisitedCell(currentCell); // ������ �� �� ��ȯ
            if (nextCell != null)
            {
                yield return GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedcells(currentCell);

        return unvisitedCells.OrderBy(x => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedcells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x; // ���� ���� ��ġ
        int z = (int)currentCell.transform.position.z;

        if (x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];
            
            if (cellToRight.IsVisited == false)
            {
                yield return cellToRight;
            }
        }
        if (x - 1 >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];

            if (cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }
        if (z + 1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x, z + 1];

            if (cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }
        }
        if (z - 1 >= 0)
        {
            var cellToBack = _mazeGrid[x, z - 1];

            if (cellToBack.IsVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null) // ���� ���� null�̸�(���� �̷� ���� ����)
        {
            return; // �׳� �Ѿ��.
        }

        if (previousCell.transform.position.x < currentCell.transform.position.x) 
        { // previousCell || currentCell �� ��Ȳ(X�� ��Ȳ�� �����ϰ� �� �� ���̿� �ΰ��� �� || ����)�� ��
            previousCell.ClearRightWall(); // previousCell�� ������ �� ����
            currentCell.ClearLeftWall(); // currentCell�� ���� �� ����
            return; // => previousCell currentCell, �� ���� ������ �̾�����. ��
        }
        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }
        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }
        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }
}
