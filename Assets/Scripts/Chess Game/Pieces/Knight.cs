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

	public bool canMoveThere(Vector2Int coords) {
		Piece temp = board.getPiece(coords);
        if (temp && temp != this) {
			if (temp.IsFromSameTeam(this)) {
				return false;
			}
			return true;
        }
        return true;
	}

	public override void MovePiece(Vector2Int coords)
	{
		Vector2Int displacement = coords - this.occupiedSquare;
		bool moved = false;
		for (int i = 0; i < offsets.Length; i++) {
			if (offsets[i] == displacement) {
				Piece pieceCheck = board.getPiece(coords);
				if (pieceCheck)
				{
					board.takePiece(this, coords);
				}
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