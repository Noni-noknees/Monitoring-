using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;


public class Maze : MonoBehaviour {
	public Maze mazePrefab;
	//private Maze mazeInstance;

	public IntVector2 size;

	public MazeCell cellPrefab;

	private MazeCell[,] cells;

	public Cell[,] grid;
	public MazePassage passagePrefab;

	public MazeWall[] wallPrefabs;

	public float generationStepDelay;

	private int counter = 0;

    private void Start()
    {

		
	}
	public MazeCell GetCell(IntVector2 coordinates)
	{
		return cells[coordinates.x, coordinates.z];
	}

	public IEnumerator Generate()
	{
		WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
		cells = new MazeCell[size.x, size.z];
		List<MazeCell> activeCells = new List<MazeCell>();
		DoFirstGenerationStep(activeCells);
		while (activeCells.Count > 0)
		{
			yield return delay;
			DoNextGenerationStep(activeCells);
		}
	}


	public IntVector2 RandomCoordinates
	{
		get
		{
			return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
		}
	}

	public bool ContainsCoordinates(IntVector2 coordinate)
	{
		return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
	}
	private void DoFirstGenerationStep(List<MazeCell> activeCells)
	{
		activeCells.Add(CreateCell(RandomCoordinates));
	}

	private void DoNextGenerationStep(List<MazeCell> activeCells)
	{
		int currentIndex = activeCells.Count - 1;
		MazeCell currentCell = activeCells[currentIndex];
		if (currentCell.IsFullyInitialized)
		{
			activeCells.RemoveAt(currentIndex);
			return;
		}
		MazeDirection direction = currentCell.RandomUninitializedDirection;
		IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
		if (ContainsCoordinates(coordinates))
		{
			MazeCell neighbor = GetCell(coordinates);
			if (neighbor == null)
			{
				neighbor = CreateCell(coordinates);
				CreatePassage(currentCell, neighbor, direction);
				activeCells.Add(neighbor);
			}
			else
			{
				CreateWall(currentCell, neighbor, direction);
				// No longer remove the cell here.
			}
		}
		else
		{
			CreateWall(currentCell, null, direction);
			// No longer remove the cell here.
		}
	}

		private MazeCell CreateCell(IntVector2 coordinates)
	{
		MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
		cells[coordinates.x, coordinates.z] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
		newCell.transform.parent = transform;
		newCell.transform.localPosition =
			new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
	
		return newCell;
	


	}

	private void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction)
	{
		MazePassage passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(otherCell, cell, direction.GetOpposite());

	}

	private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
	{
		float i = Random.Range(0, 10000000000);
		MazeWall wall; 
		if (i < 9800000000)
		{
			 wall = Instantiate(wallPrefabs[0]) as MazeWall;
		}
        else
        {
			wall = Instantiate(wallPrefabs[1]) as MazeWall;
			counter++;
			/*Camera camera = wall.GetComponentInChildren<Camera>();
			if (camera != null)
			{
				camera.gameObject.SetActive(false); 
			}*/
		}


		wall.Initialize(cell, otherCell, direction);
	
		if (otherCell != null) //Physics.CheckSphere(transform.position, sphereRadius)
		{
			//wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
			wall.Initialize(otherCell, cell, direction.GetOpposite());
		}


	}

}
