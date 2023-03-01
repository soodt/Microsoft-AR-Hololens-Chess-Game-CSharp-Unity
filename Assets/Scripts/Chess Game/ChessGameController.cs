using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;

[RequireComponent(typeof(PieceCreator))]
public class ChessGameController : MonoBehaviour
{
    [SerializeField] private BoardLayout startingBoardLayout;
    [SerializeField] private Board board;
    private PieceCreator pieceCreator;
    public Piece[] activePieces = new Piece[32];

    private ChessPlayer whitePlayer;
    private ChessPlayer blackPlayer;
    private ChessPlayer activePlayer { get; set; }

    private void Awake()
    {
        SetDependencies();
        CreatePlayers();
    }

    //setting up
    private void SetDependencies()
    {
        pieceCreator = GetComponent<PieceCreator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartNewGame();
    }

    //called by Start()
    private void StartNewGame()
    {
        //create pieces and assign white player the first move
        CreatePiecesFromLayout(startingBoardLayout);
        board.SetDependencies(this);
        activePlayer = whitePlayer;
    }

    //returns player with the next move
    public ChessPlayer getActivePlayer()
    {
        return activePlayer;
    }

    //create white and black players
    private void CreatePlayers()
    {
        whitePlayer = new ChessPlayer(TeamColor.White, board);
        blackPlayer = new ChessPlayer(TeamColor.Black, board);
    }


    private void CreatePiecesFromLayout(BoardLayout layout)
    {
        //to iterate through board square array
        for (int i = 0; i < layout.GetPiecesCount(); i++)
        {
            //Assign data of each square to separate variables 
            Vector2Int squareCoords = layout.GetSquareCoordsAtIndex(i);
            TeamColor team = layout.GetSquareTeamColorAtIndex(i);
            string typeName = layout.GetSquarePieceNameAtIndex(i);

            //create pieces based on the data
            Type type = Type.GetType(typeName);
            CreatePieceAndInitialize(squareCoords, team, type);
        }
    }

    private void CreatePieceAndInitialize(Vector2Int squareCoords, TeamColor team, Type type)
    {

        Piece newPiece = pieceCreator.CreatePiece(type).GetComponent<Piece>();
        //make each piece interactable with AR
        newPiece.gameObject.AddComponent<BoxCollider>();
        newPiece.gameObject.AddComponent<NearInteractionGrabbable>();
        newPiece.gameObject.AddComponent<ObjectManipulator>();

        // add snapping to each piece
        newPiece.GetComponent<ObjectManipulator>().OnManipulationEnded.AddListener(delegate
        {
            float distance = board.squareSize * 4;
            Vector2Int newCoords = new Vector2Int(-1, -1);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Vector2Int nextSquare = new Vector2Int(i, j);
                    float newDistance = Vector3.Distance(newPiece.transform.position, board.CalculatePositionFromCoords(nextSquare));
                    if (newDistance < distance)
                    {
                        distance = newDistance;
                        newCoords.Set(i, j);
                    }
                }
            }
            if (distance < board.squareSize * 1.5)
            {
                newPiece.MovePiece(newCoords);
            }
            else
            {
                newPiece.MovePiece(newPiece.occupiedSquare);
            }
        }
        );
        //Debug.Log("This is a sample debugging message"); // this will print the message in the debugging console.
        newPiece.SetData(squareCoords, team, board, this);
        initailzeActivePieces(newPiece);

       
        if (newPiece.getTeam() == TeamColor.White)
        {
            whitePlayer.AddPiece(newPiece);
        }
        else
        {
            blackPlayer.AddPiece(newPiece);
        }

        Material teamMaterial = pieceCreator.GetTeamMaterial(team);
        newPiece.SetMaterial(teamMaterial);

        //rotate the piece if it's black team
        if (team == TeamColor.Black)
        {
            newPiece.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
        }
    }

    public void initailzeActivePieces(Piece piece)
    {
        //store pieces into activePieces array
        for (int i = 0; i < 32; i++)
        {
            if (this.activePieces[i] == null)
            {
                this.activePieces[i] = piece;
                break;
            }
        }
        //   Debug.Log(piece); // this will print the message in the debugging console.
    }

    //remove the piece passed in as parameter
    public void recordPieceRemoval(Piece taken)
    {
        if (taken.getTeam() == TeamColor.White)
        {
            whitePlayer.RemovePiece(taken);
            blackPlayer.AddToTakenPieces(taken);
        }
        else
        {
            blackPlayer.RemovePiece(taken);
            whitePlayer.AddToTakenPieces(taken);
        }
    }


    public void endTurn()
    {
        // Swap active player
        if (getActivePlayer() == whitePlayer)
        {
            activePlayer = blackPlayer;
        }
        else if (getActivePlayer() == blackPlayer)
        {
            activePlayer = whitePlayer;
        }
        // Debug
        if (getActivePlayer() == whitePlayer)
        {
            Debug.Log("White Turn");
        }
        else
        {
            Debug.Log("Black Turn");
        }
    }

    /*public void ChangeTeam()
    {
        if (getActivePlayer() == whitePlayer)
        {
            activePlayer = blackPlayer;
        }
        else if (getActivePlayer() == blackPlayer)
        {
            activePlayer = whitePlayer;
        }
    }*/
}