using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;

public class AddComponents : MonoBehaviour, IPunInstantiateMagicCallback
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (this.gameObject.GetComponent<BoxCollider>() != null) return;
        //make each piece interactable with AR
        this.gameObject.AddComponent<BoxCollider>();
        this.gameObject.AddComponent<NearInteractionGrabbable>();
        this.gameObject.AddComponent<ObjectManipulator>();
        //make each piece have the board anchor as parent.
        this.gameObject.AddComponent<BoardAnchorAsParent>();

        
        // add snapping to each piece
        this.GetComponent<ObjectManipulator>().OnManipulationEnded.AddListener(delegate
        {
            float distance = this.gameObject.GetComponent<Board>().squareSize * 4;
            Vector2Int newCoords = new Vector2Int(-1, -1);
            for (int i = 0; i < 8; i++)
            {
                
                for (int j = 0; j < 8; j++)
                {
                    Vector2Int nextSquare = new Vector2Int(i, j);
                    float newDistance = Vector3.Distance(this.transform.position, this.gameObject.GetComponent<Board>().CalculatePositionFromCoords(nextSquare));
                    if (newDistance < distance)
                    {
                        distance = newDistance;
                        newCoords.Set(i, j);
                    }
                }
            }
            if (distance < this.gameObject.GetComponent<Board>().squareSize * 1.5)
            {
                this.gameObject.GetComponent<Piece>().MovePiece(newCoords);
            }
            else
            {
                this.gameObject.GetComponent<Piece>().MovePiece(this.gameObject.GetComponent<Piece>().occupiedSquare);
            }
        }
        );
        
    }
    
}
