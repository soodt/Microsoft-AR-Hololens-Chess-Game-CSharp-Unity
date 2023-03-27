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

	public override bool isAttackingSquare(Vector2Int coords) {
        return canMoveThere(coords);
    }

	public override void MovePiece(Vector2Int coords)
	{
		if (!taken)
		{
			Vector2Int displacement = coords - this.occupiedSquare;
			bool moved = false;
			if (this.getTeam() == controller.getActivePlayer().getTeam() && this.avaliableMoves.Contains(coords))
			{
				for (int i = 0; i < offsets.Length; i++)
				{
					if (offsets[i] == displacement)
					{
						Piece pieceCheck = board.getPiece(coords);
						if (pieceCheck)
						{
							board.takePiece(this, coords);
						}
						this.occupiedSquare = coords;
						transform.position = this.board.CalculatePositionFromCoords(coords);
						controller.endTurn();
						moved = true;
						i = offsets.Length;
					}
				}
				if (!moved)
				{
					transform.position = this.board.CalculatePositionFromCoords(this.occupiedSquare);
				}
			}
			else
			{
				// If not this team's turn, snap back to occupied square
				transform.position = this.board.CalculatePositionFromCoords(this.occupiedSquare);
				Debug.Log("NoMoving!");
			}
		}
        else
        {
			transform.position = finalCoords;
		}
	}

    public override void PossibleMoves()
    {
		avaliableMoves.Clear();
		if (!taken)
		{
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					Vector2Int square = new Vector2Int(i, j);
					Vector2Int displacementToSquare = square - this.occupiedSquare; // this is to go through all the squares checking which are safe to move to
					if (squareIsMoveable(displacementToSquare) && canMoveThere(square)) // this should be implemented when the obj is picked up to highlight the possible squares. 
					{
						avaliableMoves.Add(square);
					}
				}
			}
		}
    }

    private bool squareIsMoveable(Vector2Int displacementToSquare)
    {
		if (!taken)
		{
			for (int i = 0; i < offsets.Length; i++)
			{
				if (offsets[i] == displacementToSquare)
				{
					//Debug.Log("Turn Green");
					return true;
				}
			}
		}
        return false;
    }
    public override bool hasMovedTwoSquares()
    {
        return false;
    }
}