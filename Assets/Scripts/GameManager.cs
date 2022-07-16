using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Camera cam;

    public GameState state;

    Case highlightedCase;

    //private Vector2 casePosSelected;
    public Resource resourceSelected;

    //TODO implement event

    public void TriggerCaseClick(Case inputCase)
    {
        Debug.Log($"case = {inputCase.coordinate.x}, {inputCase.coordinate.y}");
        if (state == GameState.ResourceMove)
        {
            if (inputCase.occupiedResource == null)
            {
                GridManager.Instance.TransferResourceFromCase(resourceSelected.occupiedCase.coordinate, inputCase.coordinate);
                highlightedCase.StopHighlight();
                highlightedCase = null;
            }
            else
            {
                GridManager.Instance.ResetResourcePosition(resourceSelected);
            }
            resourceSelected.Unselect();
            resourceSelected = null;
            UpdateGameState(GameState.Neutral);
        }
    }

    public void TriggerResourceClick(Resource resource)
    {
        Debug.Log($"resource = {resource.occupiedCase.coordinate.x}, {resource.occupiedCase.coordinate.y}");
        if (state == GameState.Neutral)
        {
            resourceSelected = resource;
            resourceSelected.Select();
            UpdateGameState(GameState.ResourceMove);
        }
        else if (state == GameState.ResourceMove)
        {
            resourceSelected.Unselect();
            resourceSelected = null;
            UpdateGameState(GameState.Neutral);
            highlightedCase.StopHighlight();
            highlightedCase = null;
        }
    }

    public void UpdateGameState(GameState newState)
    {
        state = newState;

        //switch (newState)
        //{
        //    case GameState.Neutral:
        //        break;
        //    case GameState.ResourceMove:
        //        break;
        //    default:
        //        break;
        //}
    }


    private void Update()
    {
        if(state == GameState.ResourceMove)
        {
            var outCase = GridManager.Instance.GetCaseAtMousePos();
            if (highlightedCase == null)
            {
                if(outCase.occupiedResource == null)
                {
                    highlightedCase = outCase;
                    outCase.Highlight();
                }
            } else if(highlightedCase != outCase)
            {
                highlightedCase.StopHighlight();
                if (outCase.occupiedResource == null)
                {
                    highlightedCase = outCase;
                    outCase.Highlight();
                } else
                {
                    highlightedCase = null;
                }
            } else
            {
                return;
            }
        }
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
