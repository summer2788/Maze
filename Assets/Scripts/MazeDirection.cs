using UnityEngine;

public enum MazeDirection
{
	North,
	East,
	South,
	West
}

public static class MazeDirections
{

	public const int Count = 4;

	public static MazeDirection RandomValue
	{
		get
		{
			return (MazeDirection)Random.Range(0, Count);
		}
	}

	private static IntVector2[] vectors = {
		new IntVector2(-1, 0), //북
		new IntVector2(0, 1), //동
		new IntVector2(1, 0), //남 
		new IntVector2(0, -1) //서
	};

	public static IntVector2 ToIntVector2(this MazeDirection direction)
	{
		return vectors[(int)direction];   //해당 방향 벡터로 변환
	}

	private static MazeDirection[] opposites = {
		MazeDirection.South, //남
		MazeDirection.West, //서
		MazeDirection.North, //북
		MazeDirection.East //동
	};

	public static MazeDirection GetOpposite(this MazeDirection direction)
	{
		return opposites[(int)direction];
	}

	private static Quaternion[] rotations = {

		Quaternion.Euler(0f, 270f, 0f),
		Quaternion.identity,
		Quaternion.Euler(0f, 90f, 0f),
		Quaternion.Euler(0f, 180f, 0f)
		
	};

	public static Quaternion ToRotation(this MazeDirection direction)
	{
		return rotations[(int)direction];
	}

	public static MazeDirection GetNextClockwise(this MazeDirection direction)
	{
		return (MazeDirection)(((int)direction + 1) % Count);
	}

	public static MazeDirection GetNextCounterclockwise(this MazeDirection direction)
	{
		return (MazeDirection)(((int)direction + Count - 1) % Count);
	}
}