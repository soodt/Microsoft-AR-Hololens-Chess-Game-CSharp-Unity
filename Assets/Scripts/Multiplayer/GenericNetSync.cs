using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

// Modified file from MRTK.Tutorials.MultiUserCapabilities.
public class GenericNetSync : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private bool isUser = default;

    private Camera mainCamera;

    private Vector3 networkLocalPosition;
    private Quaternion networkLocalRotation;

    private Vector3 startingLocalPosition;
    private Quaternion startingLocalRotation;

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.localPosition);
            stream.SendNext(transform.localRotation);
        }
        else
        {
            networkLocalPosition = (Vector3)stream.ReceiveNext();
            networkLocalRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    private void Start()
    {
        if (!photonView.IsMine) return;

        mainCamera = Camera.main;

        if (isUser)
        {
            if (photonView.IsMine) GenericNetworkManager.Instance.localUser = photonView;
        }

        var trans = transform;

        if (gameObject.name == "User1")
        {
            trans.localPosition = new Vector3(-0.08f, 0.4f, -1.4f);
            mainCamera.transform.position = trans.localPosition;
        }
        else if (gameObject.name == "User2")
        {
            trans.localPosition = new Vector3(-0.08f, 0.4f, 0.8f);
            mainCamera.transform.position = trans.localPosition;

            trans.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
            mainCamera.transform.rotation = trans.localRotation;
        }
        else
        {

        }

        startingLocalPosition = trans.localPosition;
        startingLocalRotation = trans.localRotation;

        networkLocalPosition = startingLocalPosition;
        networkLocalRotation = startingLocalRotation;
    }

    // private void FixedUpdate()
    private void Update()
    {
        if (!photonView.IsMine)
        {
            var trans = transform;
            trans.localPosition = networkLocalPosition;
            trans.localRotation = networkLocalRotation;
        }

        if (photonView.IsMine && isUser)
        {
            var trans = transform;
            var mainCameraTransform = mainCamera.transform;
            trans.position = mainCameraTransform.position;
            trans.rotation = mainCameraTransform.rotation;
        }
    }
}
