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
    // private ChessPlayer activePlayer;     could perhaps use this for turn taking instead of a count, just easier??

    private void Awake()
    {
        SetDependencies();
    }

    private void SetDependencies()
    {
        pieceCreator = GetComponent<PieceCreator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartNewGame();
    }

     private void StartNewGame()
    {
        CreatePiecesFromLayout(startingBoardLayout);
        board.SetDependencies(this);
    }

    private void CreatePlayers()
    {
        whitePlayer = new ChessPlayer(TeamColor.White, board);
        blackPlayer = new ChessPlayer(TeamColor.Black, board);
    }


    private void CreatePiecesFromLayout(BoardLayout layout)
    {
        for (int i = 0; i < layout.GetPiecesCount(); i++)
        {
            Vector2Int squareCoords = layout.GetSquareCoordsAtIndex(i);
            TeamColor team = layout.GetSquareTeamColorAtIndex(i);
            string typeName = layout.GetSquarePieceNameAtIndex(i);

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
        newPiece.GetComponent<ObjectManipulator>().OnManipulationEnded.AddListener ( delegate 
            { 
                float distance = board.squareSize*4;
                Vector2Int newCoords = new Vector2Int(-1, -1);
                for (int i = 0; i<8; i++)
                {
                    for (int j = 0; j<8; j++)
                    {
                        Vector2Int nextSquare = new Vector2Int(i, j);
                        float newDistance = Vector3.Distance (newPiece.transform.position, board.CalculatePositionFromCoords(nextSquare));
                        if (newDistance < distance)
                        {
                            distance = newDistance;
                            newCoords.Set(i,j);
                        }
                    }
                }
                if (distance < board.squareSize*1.5)
                {
                    newPiece.MovePiece(newCoords);
                } else 
                {
                    newPiece.MovePiece(newPiece.occupiedSquare);
                }
            }
        );
        //Debug.Log("This is a sample debugging message"); // this will print the message in the debugging console.
        newPiece.SetData(squareCoords, team, board);
        initailzeActivePieces(newPiece);

        Material teamMaterial = pieceCreator.GetTeamMaterial(team);
        newPiece.SetMaterial(teamMaterial);
        if (team == TeamColor.Black)
        {
            newPiece.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
        }
    }

    public void initailzeActivePieces(Piece piece)
    {

        for (int i = 0; i < 32; i++)
        {
            if (activePieces[i] == null)
                {
                    activePieces[i] = piece;
                    break;
                }
        }
     //   Debug.Log(piece); // this will print the message in the debugging console.
    }

    public void recordPieceRemoval(Piece piece)
    {
        // removedFrom is the player that the piece was taken from
        ChessPlayer removedFrom = piece.getTeam() == TeamColor.White ? whitePlayer : blackPlayer;
        // piece is the piece that was taken

        removedFrom.RemovePiece(piece);
        if (removedFrom == whitePlayer) {
            blackPlayer.AddToTakenPieces(piece);
        } else {
            whitePlayer.AddToTakenPieces(piece);
        }
    }

   

}
