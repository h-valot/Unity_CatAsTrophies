using System.Collections.Generic;
using UnityEngine;

public static class PoissonDiscSampling
{
	private static List<Vector2> _points = new ();
	private static List<Vector2> _spawnPoints = new ();
	
	public static List<Vector2> GeneratePoints(int numOfPoint, float radius, int numSamplesBeforeRejection = 30)
	{
		Vector2 regionSize = new Vector2(radius, radius * numOfPoint);
		float cellSize = radius/Mathf.Sqrt(2);
		int[,] grid = new int[Mathf.CeilToInt(regionSize.x/cellSize), Mathf.CeilToInt(regionSize.y/cellSize)];
		_points.Clear();
		_spawnPoints.Clear();

		_spawnPoints.Add(regionSize/2);
		while (_spawnPoints.Count > 0) 
		{
			// get a random spawn point center
			int spawnIndex = Random.Range(0, _spawnPoints.Count);
			Vector2 spawnCentre = _spawnPoints[spawnIndex];
			bool candidateAccepted = false;
			
			for (int i = 0; i < numSamplesBeforeRejection; i++)
			{
				// get a random candidate point position
				float rndAngle = Random.value * Mathf.PI * 2;
				Vector2 dir = new Vector2(Mathf.Sin(rndAngle), Mathf.Cos(rndAngle));
				Vector2 candidate = spawnCentre + dir * Random.Range(radius, 2*radius);
				
				// verify if this position is valid or not
				if (IsValid(candidate, regionSize, cellSize, radius, grid)) 
				{
					_points.Add(candidate);
					_spawnPoints.Add(candidate);
					grid[(int)(candidate.x/cellSize), (int)(candidate.y/cellSize)] = _points.Count;
					candidateAccepted = true;
					break;
				}
			}
			
			// remove invalid spawn points from the spawn point candidate list
			if (!candidateAccepted) _spawnPoints.RemoveAt(spawnIndex);
		}

		// re-generate points, if the number of points generated doesn't match
		if (_points.Count != numOfPoint) _points = GeneratePoints(numOfPoint, radius, numSamplesBeforeRejection);
		
		return _points;
	}

	static bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, float radius, int[,] grid) 
	{
		if (candidate.x >=0 && candidate.x < sampleRegionSize.x &&
		    candidate.y >= 0 && candidate.y < sampleRegionSize.y) 
		{
			int cellX = (int)(candidate.x/cellSize);
			int cellY = (int)(candidate.y/cellSize);
			
			int searchStartX = Mathf.Max(0, cellX -2);
			int searchEndX = Mathf.Min(cellX+2, grid.GetLength(0)-1);
			
			int searchStartY = Mathf.Max(0, cellY -2);
			int searchEndY = Mathf.Min(cellY+2, grid.GetLength(1)-1);

			for (int x = searchStartX; x <= searchEndX; x++) 
			{
				for (int y = searchStartY; y <= searchEndY; y++) 
				{
					int pointIndex = grid[x,y]-1;
					if (pointIndex != -1) 
					{
						float sqrDistance = (candidate - _points[pointIndex]).sqrMagnitude;
						if (sqrDistance < radius*radius) 
						{
							return false;
						}
					}
				}
			}
			return true;
		}
		return false;
	}
}