using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public override List<Vector2Int> SelectAvaliableSquares()
    {
        throw new NotImplementedException();
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
            if (this.getTeam() == controller.getActivePlayer().getTeam() && this.avaliableMoves.Contains(coords))
            {
                if ((coords.x - this.occupiedSquare.x <= 1 & coords.x - this.occupiedSquare.x >= -1 &
                 coords.y - this.occupiedSquare.y <= 1 & coords.y - this.occupiedSquare.y >= -1) && canMoveThere(coords) && coords != this.occupiedSquare)
                {
                    Piece pieceCheck = board.getPiece(coords);
                    if (pieceCheck)
                    {
                        board.takePiece(this, coords);
                    }
                    this.occupiedSquare = coords;
                    transform.position = this.board.CalculatePositionFromCoords(coords);
                    if (this.hasMoved == false)
                    {
                        this.hasMoved = true;
                    }
                    controller.endTurn();
                }
                else
                {
                    Piece pieceCheck = board.getPiece(coords);
                    if (pieceCheck && pieceCheck.typeName == "Rook" && pieceCheck.getTeam() == this.getTeam() && canCastle(pieceCheck))
                    {
                        castleMove(pieceCheck);
                    }
                    else
                    {
                        transform.position = this.board.CalculatePositionFromCoords(this.occupiedSquare);
                    }
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
                    Vector2Int square = new Vector2Int(i, j); // this is to go through all the squares checking which are safe to move to
                    Piece pieceCheck = board.getPiece(square);
                    if (squareIsMoveable(square) && canMoveThere(square) && square != this.occupiedSquare) // this should be implemented when the obj is picked up to highlight the possible squares. 
                    {
                        avaliableMoves.Add(square);
                    }
                    else if (pieceCheck && pieceCheck.typeName == "Rook" && pieceCheck.getTeam() == this.getTeam() && canCastle(pieceCheck))
                    {
                        avaliableMoves.Add(square); // add castling move if available
                    }
                }
            }
        }
    }

    private bool squareIsMoveable(Vector2Int square)
    {
        if ((square.x - this.occupiedSquare.x <= 1 & square.x - this.occupiedSquare.x >= -1 &
        square.y - this.occupiedSquare.y <= 1 & square.y - this.occupiedSquare.y >= -1))
        {
            //Debug.Log("Turn Green");
            return true;
        }


        return false;
    }

    public bool canCastle(Piece rook) {
        if (rook.typeName == "Rook") {
            if (!rook.hasMoved && !this.hasMoved) {
                if (rook.occupiedSquare.x < this.occupiedSquare.x) {
                    // rook is to the left of the king
                    for (int i = this.occupiedSquare.x - 1; i > rook.occupiedSquare.x; i--) {
                        Vector2Int coords = new Vector2Int(i, this.occupiedSquare.y);
                        if (board.getPiece(coords)) {
                            return false; // there is a piece in between the rook and king, no castle allowed
                        }
                    }
                    return true; // if there is no piece between the rook and king, allow castle
                } else {
                    // rook is to the right of the king
                    for (int i = this.occupiedSquare.x + 1; i < rook.occupiedSquare.x; i++) {
                        Vector2Int coords = new Vector2Int(i, this.occupiedSquare.y);
                        if (board.getPiece(coords)) {
                            return false; // there is a piece in between the rook and king, no castle allowed
                        }
                    }
                    return true; // if there is no piece between the rook and king, allow castle
                }
            }
            return false;
        } else {
            return false;
        }
    }

    public void castleMove(Piece rook) {
        if (rook.occupiedSquare.x < this.occupiedSquare.x) {
            // rook to the left of the king
            Vector2Int kingNewCoords = new Vector2Int((this.occupiedSquare.x - 2), this.occupiedSquare.y);
            Vector2Int rookNewCoords = new Vector2Int(kingNewCoords.x + 1, this.occupiedSquare.y);

            this.occupiedSquare = kingNewCoords;
            rook.occupiedSquare = rookNewCoords;
        } else {
            // rook to the right of the king
            Vector2Int kingNewCoords = new Vector2Int((this.occupiedSquare.x + 2), this.occupiedSquare.y);
            Vector2Int rookNewCoords = new Vector2Int(kingNewCoords.x - 1, this.occupiedSquare.y);

            this.occupiedSquare = kingNewCoords;
            rook.occupiedSquare = rookNewCoords;
        }
        transform.position = this.board.CalculatePositionFromCoords(this.occupiedSquare);
        rook.transform.position = this.board.CalculatePositionFromCoords(rook.occupiedSquare);
        this.hasMoved = true;
        rook.hasMoved = true;
        controller.endTurn();
    }

    public override bool hasMovedTwoSquares()
    {
        return false;
    }

}