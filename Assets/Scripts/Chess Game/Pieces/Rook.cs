using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Rook : Piece
{
    public override List<Vector2Int> SelectAvailableSquares()
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
        if (!taken)
        {
            if (this.getTeam() == controller.getActivePlayer().getTeam() && this.availableMoves.Contains(coords))
            {
                bool capture = false;
                Vector2Int prevCoords = new Vector2Int(this.occupiedSquare.x, this.occupiedSquare.y);

                if ((coords.x - this.occupiedSquare.x == 0 | coords.y - this.occupiedSquare.y == 0) && canMoveThere(coords))
                {
                    Piece pieceCheck = board.getPiece(coords);
                    if (pieceCheck)
                    {
                        board.takePiece(this, coords);
                        capture = true;
                    }
                    this.occupiedSquare = coords;
                    transform.position = this.board.CalculatePositionFromCoords(coords);
                    if (this.hasMoved == false)
                    {
                        this.hasMoved = true;
                    }
                    print(AlgebraicNotation(coords, prevCoords, capture, false, false, false));
                    controller.endTurn();
                }
                else
                {
                    Piece pieceCheck = board.getPiece(coords);
                    if (pieceCheck && pieceCheck.typeName == "King" && pieceCheck.getTeam() == this.getTeam() && canCastle(pieceCheck))
                    {
                        print(AlgebraicNotation(coords, prevCoords, capture, false, false, true));
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
        availableMoves.Clear();
        if (!taken)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Vector2Int square = new Vector2Int(i, j); // this is to go through all the squares checking which are safe to move to
                    Piece pieceCheck = board.getPiece(square);
                    if (squareIsMoveable(square) && canMoveThere(square)) // this should be implemented when the obj is picked up to highlight the possible squares. 
                    {
                        availableMoves.Add(square);
                    }
                    else if (pieceCheck && pieceCheck.typeName == "King" && pieceCheck.getTeam() == this.getTeam() && canCastle(pieceCheck))
                    {
                        availableMoves.Add(square); // add castling move if available
                    }
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

    public bool canCastle(Piece king) {
        if (king.typeName == "King") {
            if (!king.hasMoved && !this.hasMoved) {
                if (king.occupiedSquare.x < this.occupiedSquare.x) {
                    // rook is to the right of the king
                    for (int i = this.occupiedSquare.x - 1; i > king.occupiedSquare.x; i--) {
                        Vector2Int coords = new Vector2Int(i, this.occupiedSquare.y);
                        if (board.getPiece(coords)) {
                            return false; // there is a piece in between the rook and king, no castle allowed
                        }
                    }
                    return true; // if there is no piece between the rook and king, allow castle
                } else {
                    // rook is the the left of the king
                    for (int i = this.occupiedSquare.x + 1; i < king.occupiedSquare.x; i++) {
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

    public void castleMove(Piece king) {
        if (king.occupiedSquare.x < this.occupiedSquare.x) {
            // rook to the right of the king
            Vector2Int kingNewCoords = new Vector2Int((king.occupiedSquare.x + 2), this.occupiedSquare.y);
            Vector2Int rookNewCoords = new Vector2Int(kingNewCoords.x - 1, this.occupiedSquare.y);

            this.occupiedSquare = rookNewCoords;
            king.occupiedSquare = kingNewCoords;
        } else {
            // rook to the left of the king
            Vector2Int kingNewCoords = new Vector2Int((king.occupiedSquare.x - 2), this.occupiedSquare.y);
            Vector2Int rookNewCoords = new Vector2Int(kingNewCoords.x + 1, this.occupiedSquare.y);

            this.occupiedSquare = rookNewCoords;
            king.occupiedSquare = kingNewCoords;
        }
        transform.position = this.board.CalculatePositionFromCoords(this.occupiedSquare);
        king.transform.position = this.board.CalculatePositionFromCoords(king.occupiedSquare);
        this.hasMoved = true;
        king.hasMoved = true;
        controller.endTurn();
    }

    public override bool hasMovedTwoSquares()
    {
        return false;
    }
    public override String AlgebraicNotation(Vector2Int coords, Vector2Int prevCoords, bool capture, bool pawnPromote, bool enPassant, bool castle)
    {
        String s = "R";

        foreach (Piece p in controller.getActivePlayer().activePieces)
        {
            if (!p.taken)
            {
                if (p.typeName == "Rook" && p != this)
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
        if (castle)
        {
            if (this.getTeam() == TeamColor.White)
            {
                if (prevCoords[0] == 0) s = "0-0-0";
                else s = "0-0";
            }
            else
            {
                if (prevCoords[0] == 0) s = "0-0-0";
                else s = "0-0";
            }
        }
        if (controller.checkmate()) s += "#";
        else if (controller.checkCond()) s += "+";
        return s;
    }
}