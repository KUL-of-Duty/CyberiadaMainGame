using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSoundManager : NetworkBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip _walkClip;
    [SerializeField] private AudioClip _sprintClip;
    [SerializeField] private AudioClip _jumpClip;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _audioSource.spatialBlend = 1.0f;   // 3D sound
        _audioSource.playOnAwake = false;
    }

    [ClientRpc]
    public void PlayStepSoundClientRpc(bool isSprinting)
    {
        AudioClip clipToPlay = isSprinting ? _sprintClip : _walkClip;
        if (clipToPlay == null) return;

        _audioSource.PlayOneShot(clipToPlay);
    }

    [ClientRpc]
    public void PlayJumpSoundClientRPC()
    {
        if (_jumpClip == null) return;

        _audioSource.PlayOneShot(_jumpClip);
    }
}