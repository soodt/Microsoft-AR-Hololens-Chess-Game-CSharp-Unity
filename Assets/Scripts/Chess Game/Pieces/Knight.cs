using System;
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
	public override List<Vector2Int> SelectAvailableSquares()
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
			Vector2Int prevCoords = this.occupiedSquare;
			if (this.getTeam() == controller.getActivePlayer().getTeam() && this.availableMoves.Contains(coords))
			{
				bool capture = false;
				for (int i = 0; i < offsets.Length; i++)
				{
					if (offsets[i] == displacement)
					{
						Piece pieceCheck = board.getPiece(coords);
						if (pieceCheck)
						{
							board.takePiece(this, coords);
							capture = true;
						}
						this.occupiedSquare = coords;
						transform.position = this.board.CalculatePositionFromCoords(coords);
                        print(AlgebraicNotation(coords, prevCoords, capture, false, false, false));
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
				//Debug.Log("NoMoving!");
			}
		}
        else
        {
			transform.position = finalCoords;
		}
	}

    public override void PossibleMoves()
    {
		availableMoves.Clear();
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
						availableMoves.Add(square);
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
    public override String AlgebraicNotation(Vector2Int coords, Vector2Int prevCoords, bool capture, bool pawnPromote, bool enPassant, bool castle)
    {
        String s = "N";

		foreach (Piece p in controller.getActivePlayer().activePieces)
		{
			if (!p.taken)
			{
                if (p.typeName == "Knight" && p != this)
				{
					if (p.CanMoveTo(coords))
					{
						if (prevCoords.x != p.occupiedSquare.x)
						{
                            if (prevCoords.x == 0) s += "a";
                            if (prevCoords.x == 1) s += "b";
                            if (prevCoords.x == 2) s += "c";
                            if (prevCoords.x == 3) s += "d";
                            if (prevCoords.x == 4) s += "e";
                            if (prevCoords.x == 5) s += "f";
                            if (prevCoords.x == 6) s += "g";
                            if (prevCoords.x == 7) s += "h";
                        }
						else
						{
							s += prevCoords.y + 1;
						}
					}
				}
			}
		}

        if (capture) s += "x";
        if (coords.x == 0) s += "a";
        if (coords.x == 1) s += "b";
        if (coords.x == 2) s += "c";
        if (coords.x == 3) s += "d";
        if (coords.x == 4) s += "e";
        if (coords.x == 5) s += "f";
        if (coords.x == 6) s += "g";
        if (coords.x == 7) s += "h";
        s += coords.y + 1;
        if (controller.checkmate()) s += "#";
        else if (controller.checkCond()) s += "+";
        return s;
    }
}