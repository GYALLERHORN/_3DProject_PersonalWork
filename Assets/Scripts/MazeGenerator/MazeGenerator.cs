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

    IEnumerator Start() // _mazeWidth, _mazeDepth의 크기만큼 미로 판 깔기
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x,z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }

        yield return GenerateMaze(null, _mazeGrid[0, 0]); // 이전셀 = null, 현재셀 = [0,0]으로 놓고 GenerateMaze시작
    }

    private IEnumerator GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit(); // 현재 위치의 셀 처리 호출
        ClearWalls(previousCell, currentCell); // 

        yield return new WaitForSeconds(0.005f); // 0.005초 기다렸다가 진행

        MazeCell nextCell; // currentCell -> nextCell로 갈 변수 선언

        do // 처음 한번은 무조건 해야하니까 do로 반복
        {
            nextCell = GetNextUnvisitedCell(currentCell); // 다음에 갈 셀 반환
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
        int x = (int)currentCell.transform.position.x; // 현재 셀의 위치
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
        if (previousCell == null) // 이전 셀이 null이면(최초 미로 제작 시점)
        {
            return; // 그냥 넘어간다.
        }

        if (previousCell.transform.position.x < currentCell.transform.position.x) 
        { // previousCell || currentCell 인 상황(X축 상황을 가정하고 양 셀 사이에 두개의 벽 || 존재)일 때
            previousCell.ClearRightWall(); // previousCell의 오른쪽 벽 제거
            currentCell.ClearLeftWall(); // currentCell의 왼쪽 벽 제거
            return; // => previousCell currentCell, 양 셀의 공간이 이어졌다. 끝
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
