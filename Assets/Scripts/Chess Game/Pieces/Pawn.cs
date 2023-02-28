using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece, IMixedRealityPointerHandler
{
    public override List<Vector2Int> SelectAvaliableSquares()
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
            if ((displacement.x == 1 || displacement.x == -1) && (displacement.y == 1) && board.getPiece(coords)) {
                return true;
            }
            return false;
        } else {
            if ((displacement.x == 1 || displacement.x == -1) && (displacement.y == -1) && board.getPiece(coords)) {
                return true;
            }
            return false;
        }
    }

    public override bool isAttackingSquare(Vector2Int coords) {
        return canPawnTake(coords);
    }
    public override void MovePiece(Vector2Int coords)
    {
        if (this.getTeam() == controller.getActivePlayer().getTeam()) {
            // If it is this team's turn
            if (this.team == TeamColor.White & ((this.occupiedSquare.y != 1 & coords.x - this.occupiedSquare.x == 0 & coords.y - this.occupiedSquare.y == 1) |
            (this.occupiedSquare.y == 1 & coords.x - this.occupiedSquare.x == 0 & coords.y - this.occupiedSquare.y <= 2 & coords.y - this.occupiedSquare.y >= 1)) && canMoveThere(coords))
            {
                this.occupiedSquare = coords;
                transform.position = this.board.CalculatePositionFromCoords(coords);
                controller.endTurn();
            }
            else if (this.team == TeamColor.Black & ((this.occupiedSquare.y != 6 & coords.x - this.occupiedSquare.x == 0 & this.occupiedSquare.y - coords.y == 1) |
                (this.occupiedSquare.y == 6 & coords.x - this.occupiedSquare.x == 0 & this.occupiedSquare.y - coords.y <= 2 & this.occupiedSquare.y - coords.y >= 1))  && canMoveThere(coords))
            {
                this.occupiedSquare = coords;
                transform.position = this.board.CalculatePositionFromCoords(coords);
                controller.endTurn();
            } else if (canPawnTake(coords) && board.getPiece(coords)){
                board.takePiece(this, coords);
                this.occupiedSquare = coords;
                transform.position = this.board.CalculatePositionFromCoords(coords);
                controller.endTurn();
            } else
            {
                transform.position = this.board.CalculatePositionFromCoords(this.occupiedSquare);
            }
        } else {
            // If not this team's turn, snap back to occupied square
            transform.position = this.board.CalculatePositionFromCoords(this.occupiedSquare);
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
                if (squareIsMoveable(square)) // this should be implemented when the obj is picked up to highlight the possible squares. 
                {
                    avaliableMoves.Add(square);
                }
            }
        }
    }

    private bool squareIsMoveable(Vector2Int square)
    {

        if (this.team == TeamColor.White & ((this.occupiedSquare.y != 1 & square.x - this.occupiedSquare.x == 0 & square.y - this.occupiedSquare.y == 1) |
        (this.occupiedSquare.y == 1 & square.x - this.occupiedSquare.x == 0 & square.y - this.occupiedSquare.y <= 2 & square.y - this.occupiedSquare.y >= 1)))
        {
            Debug.Log("Turn Green");
            return true;
        }
        else if (this.team == TeamColor.Black & ((this.occupiedSquare.y != 6 & square.x - this.occupiedSquare.x == 0 & this.occupiedSquare.y - square.y == 1) |
            (this.occupiedSquare.y == 6 & square.x - this.occupiedSquare.x == 0 & this.occupiedSquare.y - square.y <= 2 & this.occupiedSquare.y - square.y >= 1)))
        {
            Debug.Log("Turn Green");
            return true;
        }
        else if (canPawnTake(square)) // doesn't work for diagonal takes...
        {
            Debug.Log("Turn Green");
            return true;
        }
        else
        {
            return false;
        }
    }
    /*
    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        PossibleMoves();
        board.HightlightTiles(avaliableMoves);
        Debug.Log("Down"); ;
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        Debug.Log("Drag");
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        avaliableMoves.Clear();
        board.HightlightTiles(avaliableMoves);
        Debug.Log("up");
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        Debug.Log("click");
    }
    */
}
