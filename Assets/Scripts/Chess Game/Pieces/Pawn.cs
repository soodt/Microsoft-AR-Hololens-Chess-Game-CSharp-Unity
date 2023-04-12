using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece, IMixedRealityPointerHandler
{

    public bool movedTwoSquares = false; // bool to track if pawn has moved two squares on the last turn and is vulnerable to en passant

    public override bool hasMovedTwoSquares() {
        return movedTwoSquares;
    }
    public override List<Vector2Int> SelectAvailableSquares()
    {
        throw new NotImplementedException();
    }

     public bool canMoveThere(Vector2Int coords) {
		Piece temp = board.getPiece(coords);
        if (temp && temp != this) {
            return false;
        }
        return true;
	}

    public bool canPawnTake(Vector2Int coords) {
        int xPos = this.occupiedSquare.x;
        int yPos = this.occupiedSquare.y;
        Vector2Int displacement = new Vector2Int(xPos, yPos);
        displacement = coords - displacement;
        if (this.team == TeamColor.White) {
            if ((displacement.x == 1 || displacement.x == -1) && (displacement.y == 1) && board.getPiece(coords) && (board.getPiece(coords).getTeam() != this.getTeam())) {
                return true;
            }
            return false;
        } else {
            if ((displacement.x == 1 || displacement.x == -1) && (displacement.y == -1) && board.getPiece(coords) && (board.getPiece(coords).getTeam() != this.getTeam())) {
                return true;
            }
            return false;
        }
    }

    public bool canTakeEnPassant(Vector2Int coords){
        Vector2Int displacement = new Vector2Int(this.occupiedSquare.x, this.occupiedSquare.y);
        displacement = coords - displacement;   
        if (this.team == TeamColor.White) {
            Vector2Int passedSquare = new Vector2Int(coords.x, coords.y - 1);

            if ((displacement.x == 1 || displacement.x == -1) && (displacement.y == 1) &&  board.getPiece(passedSquare)) {
                if ((board.getPiece(passedSquare).getTeam() != this.getTeam()) && (board.getPiece(passedSquare).typeName == "Pawn")) {
                    if (board.getPiece(passedSquare).hasMovedTwoSquares()) {
                        return true;
                    }
                }
                    
            }
            return false;
        } else {
            Vector2Int passedSquare = new Vector2Int(coords.x, coords.y + 1);

            if ((displacement.x == 1 || displacement.x == -1) && (displacement.y == -1) && board.getPiece(passedSquare)) {
                if ((board.getPiece(passedSquare).getTeam() != this.getTeam()) && (board.getPiece(passedSquare).typeName == "Pawn"))
                {
                    if (board.getPiece(passedSquare).hasMovedTwoSquares())
                    {
                        return true;
                    }
                }

            }
            return false;
        }
    }

    public void queening()
    {
        if (this.occupiedSquare.y == 7 || this.occupiedSquare.y == 0)
        {
            Debug.Log("Queening");
            //Debug.Log("" + pQueenName);
            Type type = Type.GetType("Queen");
            controller.CreatePieceAndInitialize(this.occupiedSquare, this.team, type);
            Destroy(this.gameObject);
        }
        return;
    }

    public override bool isAttackingSquare(Vector2Int coords) {
        return canPawnTake(coords);
    }
    public override void MovePiece(Vector2Int coords)
    {
        if (this.getTeam() == controller.getActivePlayer().getTeam() && this.availableMoves.Contains(coords)) {
            // If it is this team's turn
            Vector2Int prevCoords = new Vector2Int(this.occupiedSquare.x, this.occupiedSquare.y);
            bool capture = false;
            bool enPassant = false;
            bool promoted = false;
            if (squareIsMoveable(coords))
            {
                if (this.occupiedSquare.y - coords.y == 2 || this.occupiedSquare.y - coords.y == -2)
                {
                    this.movedTwoSquares = true;
                }
                else
                {
                    this.movedTwoSquares = false;
                }
                this.occupiedSquare = coords;
                transform.position = this.board.CalculatePositionFromCoords(coords);
                this.hasMoved = true;
                queening();
                controller.endTurn();
            } else if (canPawnTake(coords)){
                capture = true;
                this.movedTwoSquares = false;
                board.takePiece(this, coords);
                this.occupiedSquare = coords;
                transform.position = this.board.CalculatePositionFromCoords(coords);
                this.hasMoved = true;
                queening();
                controller.endTurn();
            } else if (canTakeEnPassant(coords)) {
                enPassant= true;
                capture = true;
                if (this.getTeam() == TeamColor.White) { 
                    Vector2Int passedSquare = new Vector2Int(coords.x, coords.y - 1);
                    board.takePiece(this, passedSquare);
                } else {
                    Vector2Int passedSquare = new Vector2Int(coords.x, coords.y + 1);
                    board.takePiece(this, passedSquare);
                }
                this.occupiedSquare = coords;
                transform.position = this.board.CalculatePositionFromCoords(coords);
                this.hasMoved = true;
                queening();
                controller.endTurn();
            }
            {
                transform.position = this.board.CalculatePositionFromCoords(this.occupiedSquare);
            }
            print(AlgebraicNotation(coords, prevCoords, capture, promoted, enPassant, false));
        } else {
            // If not this team's turn, snap back to occupied square
            transform.position = this.board.CalculatePositionFromCoords(this.occupiedSquare);
        }
    }


    public override void PossibleMoves()
    {
        availableMoves.Clear();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Vector2Int square = new Vector2Int(i, j); // this is to go through all the squares checking which are safe to move to
                if (squareIsMoveable(square) || canPawnTake(square) || canTakeEnPassant(square)) // this should be implemented when the obj is picked up to highlight the possible squares. 
                {
                    availableMoves.Add(square);
                }
            }
        }
    }

    private bool squareIsMoveable(Vector2Int square)
    {

        if (this.team == TeamColor.White & ((this.occupiedSquare.y != 1 & square.x - this.occupiedSquare.x == 0 & square.y - this.occupiedSquare.y == 1) |
        (this.occupiedSquare.y == 1 & square.x - this.occupiedSquare.x == 0 & square.y - this.occupiedSquare.y <= 2 & square.y - this.occupiedSquare.y >= 1)))
        {
            //Debug.Log("Turn Green");
            if ((square.y - this.occupiedSquare.y == 2) && (!canMoveThere(new Vector2Int(this.occupiedSquare.x, this.occupiedSquare.y + 1)))) {
                return false;
            }
            if (canMoveThere(square)) {
                return true;
            }
            return false;
        }
        else if (this.team == TeamColor.Black & ((this.occupiedSquare.y != 6 & square.x - this.occupiedSquare.x == 0 & this.occupiedSquare.y - square.y == 1) |
            (this.occupiedSquare.y == 6 & square.x - this.occupiedSquare.x == 0 & this.occupiedSquare.y - square.y <= 2 & this.occupiedSquare.y - square.y >= 1)))
        {
           // Debug.Log("Turn Green");
           if ((this.occupiedSquare.y - square.y == 2) && (!canMoveThere(new Vector2Int(this.occupiedSquare.x, this.occupiedSquare.y - 1)))) {
                return false;
            }
           if (canMoveThere(square)) {
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }
    public override String AlgebraicNotation(Vector2Int coords, Vector2Int prevCoords, bool capture, bool pawnPromote, bool enPassant, bool castle)
    {
        String s = "";

        if (capture)
        {
            if (prevCoords.x == 0) s += "a";
            if (prevCoords.x == 1) s += "b";
            if (prevCoords.x == 2) s += "c";
            if (prevCoords.x == 3) s += "d";
            if (prevCoords.x == 4) s += "e";
            if (prevCoords.x == 5) s += "f";
            if (prevCoords.x == 6) s += "g";
            if (prevCoords.x == 7) s += "h";
            s += "x";
        }
        if (coords.x == 0) s += "a";
        if (coords.x == 1) s += "b";
        if (coords.x == 2) s += "c";
        if (coords.x == 3) s += "d";
        if (coords.x == 4) s += "e";
        if (coords.x == 5) s += "f";
        if (coords.x == 6) s += "g";
        if (coords.x == 7) s += "h";
        s += coords.y + 1;
        if (enPassant) s += " e.p.";
        if (pawnPromote) s += "=Q";

        if(controller.checkmate()) s += "#";
        else if (controller.checkCond()) s += "+";
        return s;
    }
    /*
    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        PossibleMoves();
        board.HightlightTiles(availableMoves);
        Debug.Log("Down"); ;
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        Debug.Log("Drag");
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        availableMoves.Clear();
        board.HightlightTiles(availableMoves);
        Debug.Log("up");
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        Debug.Log("click");
    }
    */
}
