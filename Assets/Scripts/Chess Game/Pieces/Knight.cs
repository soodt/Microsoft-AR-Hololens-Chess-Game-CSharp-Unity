using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{

	Vector2Int[] offsets = new Vector2Int[]
	{
		//all directions knight can move to 
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

	//returns true if can move to coords passed in as parameter , false otherwise
	public bool canMoveThere(Vector2Int coords)
	{
		Piece temp = board.getPiece(coords);
		if (temp && temp != this)
		{
			//when meets a piece from same team
			if (temp.IsFromSameTeam(this))
			{
				return false;
			}
			return true;
		}
		return true;
	}
	//calls canMoveThere() and returns boolean value
	public override bool isAttackingSquare(Vector2Int coords)
	{
		return canMoveThere(coords);
	}


	public override void MovePiece(Vector2Int coords)
	{
		Vector2Int displacement = coords - this.occupiedSquare;
		bool moved = false;
		//if current player's turn, move the piece
		if (this.getTeam() == controller.getActivePlayer().getTeam())
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
			// if cannot move to the place, stay in the occupied square
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
}