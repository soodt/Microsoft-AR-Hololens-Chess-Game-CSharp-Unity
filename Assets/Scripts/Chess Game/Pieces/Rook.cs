using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    public override List<Vector2Int> SelectAvaliableSquares()
    {
        throw new System.NotImplementedException();
    }

    public bool canMoveThere(Vector2Int coords) {
		int xPos = this.occupiedSquare.x;
        int yPos = this.occupiedSquare.y;
        if (xPos > coords.x) {
            for (xPos = this.occupiedSquare.x; xPos >= coords.x; xPos--) {
                Piece temp = board.getPiece(new Vector2Int(xPos, yPos));
                if (temp && temp != this) {
                    if (!temp.IsFromSameTeam(this) && (xPos == coords.x && yPos == coords.y)) {
                        return true;
                    }
                    return false;
                }
            }
            return true;
        } else if (xPos < coords.x) {
            for (xPos = this.occupiedSquare.x; xPos <= coords.x; xPos++) {
                Piece temp = board.getPiece(new Vector2Int(xPos, yPos));
                if (temp && temp != this) {
                    if (!temp.IsFromSameTeam(this) && (xPos == coords.x && yPos == coords.y)) {
                        return true;
                    }
                    return false;
                }
            }
            return true;
        } else if (yPos > coords.y) {
            for (yPos = this.occupiedSquare.y; yPos >= coords.y; yPos--) {
                Piece temp = board.getPiece(new Vector2Int(xPos, yPos));
                if (temp && temp != this) {
                    if (!temp.IsFromSameTeam(this) && (xPos == coords.x && yPos == coords.y)) {
                        return true;
                    }
                    return false;
                }
            }
            return true;
        } else if (yPos < coords.y) {
            for (yPos = this.occupiedSquare.y; yPos <= coords.y; yPos++) {
                Piece temp = board.getPiece(new Vector2Int(xPos, yPos));
                if (temp && temp != this) {
                    if (!temp.IsFromSameTeam(this) && (xPos == coords.x && yPos == coords.y)) {
                        return true;
                    }
                    return false;
                }
            }
            return true;
        }
        return false; // should never reach here, just clears an error
	}

    public override bool isAttackingSquare(Vector2Int coords) {
        return canMoveThere(coords);
    }

    public override void MovePiece(Vector2Int coords)
	{
        if (this.getTeam() == controller.getActivePlayer().getTeam())
        {
            if ((coords.x - this.occupiedSquare.x == 0 | coords.y - this.occupiedSquare.y == 0) && canMoveThere(coords))
            {
                Piece pieceCheck = board.getPiece(coords);
                if (pieceCheck)
                {
                    board.takePiece(this, coords);
                }
                this.occupiedSquare = coords;
                transform.position = this.board.CalculatePositionFromCoords(coords);
                controller.endTurn();
            }
            else
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

    public override void PossibleMoves()
    {
        avaliableMoves.Clear();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Vector2Int square = new Vector2Int(i, j); // this is to go through all the squares checking which are safe to move to
                if (squareIsMoveable(square) && canMoveThere(square)) // this should be implemented when the obj is picked up to highlight the possible squares. 
                {
                    avaliableMoves.Add(square);
                }
            }
        }
    }

    private bool squareIsMoveable(Vector2Int square)
    {
        if ((square.x - this.occupiedSquare.x == 0 | square.y - this.occupiedSquare.y == 0) && canMoveThere(square))// checks if the piece can move
        {
            //Debug.Log("Turn Green");
            return true;
        }


        return false;
    }
}