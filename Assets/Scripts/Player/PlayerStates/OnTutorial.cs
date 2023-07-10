﻿using System.Collections;
using UnityEngine;

public class OnTutorial : PlayerState
{
    private PlayerInput m_inputs;
    private bool m_wPressed, m_sPressed, m_aPressed, m_dPressed;
    public static bool OnTutorialOne, OnTutorialTwo;

    public OnTutorial(Enumerators.PlayerState stateID, StatesMachine<Enumerators.PlayerState> stateMachine = null) : base(stateID, stateMachine)
    {
        m_inputs = new();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        m_inputs.Enable();

        GameManager.Instance.Player.PlayerController.EnableController();

        GameManager.Instance.EventManager.TriggerEvent(Enumerators.Events.ShowHud);
    }

    public override void OnUpdate()
    {

        base.OnUpdate();

        if (m_inputs.Traslation.Forward.IsInProgress() || m_inputs.Traslation.Lateral.IsInProgress()) 
            GameManager.Instance.SoundEventManager.TriggerEvent(Enumerators.MusicEvents.PlaySoundPlayer, GameManager.Instance.SoundManager.PlayerStep);

        HandleInventoryOpening();
        HandlePauseMenu();

        CheckTutorialOneState();
        CheckTutorialTwoState();
    }

    public override void OnExit()
    {
        base.OnExit();

        m_inputs.Disable();

        GameManager.Instance.Player.PlayerController.DisableController();

        GameManager.Instance.EventManager.TriggerEvent(Enumerators.Events.HideHud);
    }

    private void HandleInventoryOpening()
    {
        if (!OnTutorialOne) return;
        if (m_inputs.UI.ToggleInventory.WasReleasedThisFrame())
            m_stateMachine.ChangeState(Enumerators.PlayerState.OnInventory);
    }

    private void HandlePauseMenu()
    {
        if (m_inputs.UI.Pause.WasReleasedThisFrame())
        {
            m_stateMachine.ChangeState(Enumerators.PlayerState.OnPause);
            GameManager.Instance.EventManager.TriggerEvent(Enumerators.Events.PauseGame);
        }
    }

    private void CheckTutorialOneState()
    {
        if (!m_wPressed)
        {
            if (m_inputs.Traslation.Forward.ReadValue<float>() > 0) m_wPressed = true;
        }
        else if (!m_sPressed)
        {
            if (m_inputs.Traslation.Forward.ReadValue<float>() < 0) m_sPressed = true;
        }
        else if (!m_dPressed)
        {
            if (m_inputs.Traslation.Lateral.ReadValue<float>() > 0) m_dPressed = true;
        }
        else if (!m_aPressed)
        {
            if (m_inputs.Traslation.Lateral.ReadValue<float>() < 0)
            {
                m_aPressed = true;
                OnTutorialOne = true;
                GameManager.Instance.EventManager.TriggerEvent(Enumerators.Events.NextTutorial);
            }
        }
    }

    private void CheckTutorialTwoState()
    {
        if (!OnTutorialTwo) return;
        GameManager.Instance.EventManager.TriggerEvent(Enumerators.Events.CloseTutorial);
    }
}