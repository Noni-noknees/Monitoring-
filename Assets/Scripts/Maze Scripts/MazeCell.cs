using UnityEngine;

	public class MazeCell : MonoBehaviour
	{

	public IntVector2 coordinates;
	private int initializedEdgeCount;
	public GameObject food;
	public GameObject Spawner;
	public GameObject Waypoint;

	public float waypoint = 0.8f;
	public float badProb = 0.2f;
	public float goodProb = 0.2f;
	private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];

		public MazeCellEdge GetEdge(MazeDirection direction)
		{
			return edges[(int)direction];
		}
		
	   public void items()
    {
		float randVal = Random.value;
		if (randVal < badProb)
		{
			Instantiate(Spawner, new Vector3(coordinates.x - 9.5f, 0.3f, coordinates.z - 9.5f), Quaternion.identity);
		}
		/*else if (randVal < badProb + goodProb)
		{
			float randVal2 = Random.value;
			if (randVal2 < 0.4)
			{
				Instantiate(food, new Vector3(coordinates.x - 9.5f, 0.1f, coordinates.z - 9.5f), Quaternion.identity);
			}
			
           

		}*/
		float waypointval = Random.value;
        if (waypointval < waypoint)
        {
			Instantiate(Waypoint, new Vector3(coordinates.x - 9.5f, 0.3f, coordinates.z - 9.5f), Quaternion.identity);
		}
	}	


		public bool IsFullyInitialized
		{
			get
			{
				return initializedEdgeCount == MazeDirections.Count;
			}
		}

		public void SetEdge(MazeDirection direction, MazeCellEdge edge)
		{
		    items();
			edges[(int)direction] = edge;
			initializedEdgeCount += 1;
		}

		public MazeDirection RandomUninitializedDirection
		{
			get
			{
				int skips = Random.Range(0, MazeDirections.Count - initializedEdgeCount);
				for (int i = 0; i < MazeDirections.Count; i++)
				{
					if (edges[i] == null)
					{
						if (skips == 0)
						{
							return (MazeDirection)i;
						}
						skips -= 1;
					}
				}
				throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
			}
		}
	}

