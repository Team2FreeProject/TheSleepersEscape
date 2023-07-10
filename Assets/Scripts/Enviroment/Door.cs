using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public static bool IsOpen;
    public ParticleSystem LightVFX;
    private Animator m_animator;
    private Vector3 m_closedPosition;

    private void Awake()
    {
        m_closedPosition = transform.position;
        m_animator = GetComponent<Animator>();
        GameManager.Instance.EventManager.Register(Enumerators.Events.OpenDoor, OpenDoor);
        GameManager.Instance.EventManager.Register(Enumerators.Events.OpenDoorOnGameOver, OpenDoor);
        GameManager.Instance.EventManager.Register(Enumerators.Events.CloseDoor, CloseDoor);
        GameManager.Instance.EventManager.Register(Enumerators.Events.EnableDials, TurnOnDoorLight);
        GameManager.Instance.EventManager.Register(Enumerators.Events.EnableDials, OpenGlass);
    }

    public void OpenDoor()
    {
        IsOpen = true;
        GameManager.Instance.SoundEventManager.TriggerEvent(Enumerators.MusicEvents.PlaySoundDoor, GameManager.Instance.SoundManager.DoorOpen);
        Debug.Log("Door opening");
        m_animator.Play("OpenDoor");
    }

    public void CloseDoor()
    {
        IsOpen = false;
        Debug.Log("Door closing");
        m_animator.Play("Closed");
        transform.position = m_closedPosition;
        TurnOffDoorLight();
    }

    public void OpenGlass()
    {
        m_animator.Play("OpenDialGlass");
    }

    public void TurnOnDoorLight() => LightVFX.gameObject.SetActive(true);
    public void TurnOffDoorLight() => LightVFX.gameObject.SetActive(false);
}
