using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace ChessRoom
{
    // Modified file from MRTK.Tutorials.MultiUserCapabilities.
    public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
    {
        public static PhotonRoom Room;

        [SerializeField] private GameObject gameMaster;
        [SerializeField] private GameObject playerWhite;
        [SerializeField] private GameObject playerBlack;
        [SerializeField] private GameObject spectator;

        // private PhotonView pv;
        private Player[] photonPlayers;
        private int playersInRoom;
        private int myNumberInRoom;

        // private GameObject module;
        // private Vector3 moduleLocation = Vector3.zero;

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            photonPlayers = PhotonNetwork.PlayerList;
            playersInRoom++;
        }

        private void Awake()
        {
            if (Room == null)
            {
                Room = this;
            }
            else
            {
                if (Room != this)
                {
                    Destroy(Room.gameObject);
                    Room = this;
                }
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.AddCallbackTarget(this);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        private void Start()
        {
            // pv = GetComponent<PhotonView>();

            // Allow prefabs not in a Resources folder
            if (PhotonNetwork.PrefabPool is DefaultPool pool)
            {
                if (playerWhite != null) pool.ResourceCache.Add(playerWhite.name, playerWhite);
                if (playerBlack != null) pool.ResourceCache.Add(playerBlack.name, playerBlack);
                if (spectator != null) pool.ResourceCache.Add(spectator.name, spectator);
                foreach (GameObject prefab in gameMaster.GetComponent<PieceCreator>().GetPrefabs())
                {
                    //Vector3 scaleChange = new Vector3(-0.9f, -0.9f, -0.9f);
                    pool.ResourceCache.Add(prefab.name, prefab);
                }
            }
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            photonPlayers = PhotonNetwork.PlayerList;
            playersInRoom = photonPlayers.Length;
            myNumberInRoom = playersInRoom;
            PhotonNetwork.NickName = myNumberInRoom.ToString();
            CreatePlayer();
            if (!PhotonNetwork.IsMasterClient) return;
            //if (playersInRoom == 2)
            StartGame();
        }

        private void StartGame()
        {
            gameMaster.GetComponent<ChessGameController>().StartNetworkGame();
        }


        private void CreatePlayer()
        {
            if (playersInRoom == 1)
            {
                PhotonNetwork.Instantiate(playerWhite.name, Vector3.zero, Quaternion.identity);
            }
            else if (playersInRoom == 2)
            {
                PhotonNetwork.Instantiate(playerBlack.name, Vector3.zero, Quaternion.identity);
            }
            else PhotonNetwork.Instantiate(spectator.name, Vector3.zero, Quaternion.identity);

        }

        /**
        private void CreateInteractableObjects()
        {
            var position = roverExplorerLocation.position;
            var positionOnTopOfSurface = new Vector3(position.x, position.y + roverExplorerLocation.localScale.y / 2,
                position.z);

            var go = PhotonNetwork.Instantiate(roverExplorerPrefab.name, positionOnTopOfSurface,
                roverExplorerLocation.rotation);
        }
         */

        // private void CreateMainLunarModule()
        // {
        //     module = PhotonNetwork.Instantiate(roverExplorerPrefab.name, Vector3.zero, Quaternion.identity);
        //     pv.RPC("Rpc_SetModuleParent", RpcTarget.AllBuffered);
        // }
        //
        // [PunRPC]
        // private void Rpc_SetModuleParent()
        // {
        //     Debug.Log("Rpc_SetModuleParent- RPC Called");
        //     module.transform.parent = TableAnchor.Instance.transform;
        //     module.transform.localPosition = moduleLocation;
        // }
    }
}
