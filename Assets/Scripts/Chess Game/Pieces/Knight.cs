using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
	
	Vector2Int[] offsets = new Vector2Int[]
	{
		new Vector2Int(2, 1),
		new Vector2Int(2, -1),
		new Vector2Int(1, 2),
		new Vector2Int(1, -2),
		new Vector2Int(-2, 1),
		new Vector2Int(-2, -1),
		new Vector2Int(-1, 2),
		new Vector2Int(-1, -2),
	};
	public override List<Vector2Int> SelectAvaliableSquares()
	{
		throw new System.NotImplementedException();
	}

	public override void MovePiece(Vector2Int coords)
	{
		Vector2Int displacement = coords - this.occupiedSquare;
		bool moved = false;
		for (int i = 0; i < offsets.Length; i++) {
			if (offsets[i] == displacement) {
				this.occupiedSquare = coords;
				transform.position = this.board.CalculatePositionFromCoords(coords);
				moved = true;
				i = offsets.Length;
			}
		}
		if (!moved) {
			transform.position = this.board.CalculatePositionFromCoords(this.occupiedSquare);
		}

	}
}