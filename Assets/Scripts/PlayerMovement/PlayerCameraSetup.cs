// using UnityEngine;
// using Unity.Netcode;
// using Unity.Cinemachine;

// public class PlayerCameraSetup : NetworkBehaviour
// {
//     private CinemachineVirtualCamera vcam;

//     public override void OnNetworkSpawn()
//     {
//         if (!IsOwner)
//         {
//             return;
//         }

//         // Znajdujemy kamerę w scenie
//         vcam = FindObjectOfType<CinemachineVirtualCamera>();

//         // Przypinamy kamerę do lokalnego gracza
//         vcam.Follow = transform;
//         vcam.LookAt = transform;
//     }
// }
