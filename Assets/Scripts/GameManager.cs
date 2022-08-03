using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Camera cam;

    public GameState state;

    public Resource resourceSelected;

    public void TriggerCaseClick(Case inputCase)
    {
        Debug.Log($"case = {inputCase.coordinate.x}, {inputCase.coordinate.y}");
        if (state == GameState.ResourceMove)
        {
            if (inputCase.occupiedResource == null)
            {
                GridManager.Instance.TransferResourceFromCase(resourceSelected.occupiedCase.coordinate, inputCase.coordinate);
            }
            else
            {
                GridManager.Instance.ResetResourcePosition(resourceSelected);
            }
            UpdateGameState(GameState.Neutral);
        }
    }

    public void TriggerResourceClick(Resource resource)
    {
        Debug.Log($"resource = {resource.occupiedCase.coordinate.x}, {resource.occupiedCase.coordinate.y}");
        if (state == GameState.Neutral)
        {
            resource.Select();
            UpdateGameState(GameState.ResourceMove);
        }
        else if (state == GameState.ResourceMove)
        {
            UpdateGameState(GameState.Neutral);
        }
    }

    public void UpdateGameState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.Neutral:
                HandleNeutralState();
                break;
            case GameState.ResourceMove:
                HandleResourceMoveState();
                break;
            default:
                throw new Exception($"Incorrect state mentionned: {newState}");
        }
    }

    void HandleNeutralState()
    {
        if (resourceSelected != null)
        {
            resourceSelected.Unselect();
        }
        if (GridManager.Instance.highlightedCase != null)
        {
            GridManager.Instance.highlightedCase.StopHighlight();
        }
    }

    void HandleResourceMoveState()
    {
        return;
    }

    void Awake()
    {
        Instance = this;
        cam = Camera.main;
    }

    public enum GameState
    {
        Neutral,
        ResourceMove
    }
}
