using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private int _width, _height;

    [SerializeField] private Case _casePrefab;

    [SerializeField] private Transform _cam;

    public Case highlightedCase;

    private Dictionary<Vector2, Case> _cases;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        _cases = new Dictionary<Vector2, Case>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedCase = Instantiate(_casePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedCase.name = $"Case {x} {y}";

                var isOffset = ((x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0));

                if ((x == 2 && y == 2) || (x == 6 && y == 6) || (x == 3 && y == 4) || (x == 1 && y == 1))
                {
                    spawnedCase.Init(new Vector2 (x, y), isOffset, Resource.Type.Wood);
                } else
                {
                    spawnedCase.Init(new Vector2(x, y), isOffset);
                }

                _cases[new Vector2(x, y)] = spawnedCase;
            }
        }
        _cam.transform.position = new Vector3((float)_width/2 -0.5f, (float)_height/2 - 0.5f, -10);
    }

    public Case GetCaseAtPosition(Vector2 position)
    {
        if(_cases.TryGetValue(position, out var outputCase))
        {
            return outputCase;
        }

        return null;
    }

    public void ResetResourcePosition(Resource resource)
    {
        resource.RegenerateResource(resource.occupiedCase);
    }

    public bool TransferResourceFromCase(Vector2 fromPos, Vector2 toPos)
    {
        var fromCase = GetCaseAtPosition(fromPos);
        var toCase = GetCaseAtPosition(toPos);

        if(fromCase != null && toCase != null && fromCase != toCase)
        {
            if (fromCase.occupiedResource != null && toCase.occupiedResource == null)
            {
                toCase.AddResource(fromCase.occupiedResource);
                fromCase.RemoveResource();
                return true;
            } else
            {
                Debug.Log("Can't transfer resource because there is a resource that is blocking");
                return false;
            }
        } else
        {
            Debug.Log("Can't transfer resource because the fromCase or the toCase is null");
            return false;
        }
    }

    public Case GetCaseAtMousePos()
    {
        var mousePos = GetMousePos();
        RaycastHit2D hitData = Physics2D.Raycast(new Vector2(mousePos.x, mousePos.y), Vector2.zero, 0, LayerMask.GetMask("Case"));

        if (hitData)
        {
            var outCase = hitData.transform.gameObject.GetComponent<Case>();
            if (outCase != null)
            {
                return outCase;
            }
            else
            {
                Debug.LogError($"Object hit by Raycast is not a Case {hitData.transform.gameObject}");
                return null;
            }
        }
        else
        {
            Debug.LogError($"Not Able to get a case with Raycast at mouse postion {mousePos}");
            return null;
        }
    }

    public Vector3 GetMousePos()
    {
        var mousePos = GameManager.Instance.cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }

    private void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.ResourceMove)
        {
            var outCase = GetCaseAtMousePos();

            if(highlightedCase == outCase)
            {
                return;
            }
            else
            {
                if (highlightedCase != null)
                {
                    highlightedCase.StopHighlight();
                }
                if (outCase.occupiedResource == null)
                {
                    outCase.Highlight();
                }
            }
        }
    }
}
