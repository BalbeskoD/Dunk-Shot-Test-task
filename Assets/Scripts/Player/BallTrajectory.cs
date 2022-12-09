using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BallTrajectory : MonoBehaviour
{

    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float simulateForDuration = 1f;
    [SerializeField] float simulationStep = 0.1f;
    [SerializeField] private float maxDist = 5.0f;

    private static readonly string leftBorderTag = "LeftBorder";
    private static readonly string rightBorderTag = "RightBorder";
    
    private PlayerController _playerController;
    private Ball _ball;

    [Inject]
    public void Constructor(Ball ball, PlayerController playerController)
    {
        _ball = ball;
        _playerController = playerController;
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.textureMode = LineTextureMode.RepeatPerSegment;
    }
    
    private void LateUpdate()
    {
        if (Input.GetMouseButtonUp(0))
            lineRenderer.positionCount = 0;

        if (!_playerController.IsControlable) return;
        
        if (Input.GetMouseButton(0))
        {
            lineRenderer.positionCount = _playerController.TotalScale > 1.1f ? 
                lineRenderer.positionCount = SimulateArc().Count : lineRenderer.positionCount = 0;
            
            for (int a = 0; a < lineRenderer.positionCount; a++)
                lineRenderer.SetPosition(a, SimulateArc()[a]);
        }
    }

    private List<Vector2> SimulateArc()
    {
        var steps = (int)(simulateForDuration / simulationStep);
        Vector2 directionVector;
        Vector2 calculatedPosition;
        if (_playerController.TotalScale >= 1.8)
        {
            directionVector =
                Vector2.ClampMagnitude(
                    new Vector2(_playerController.StartMousePos.x - _playerController.MousePos.x,
                        _playerController.StartMousePos.y - _playerController.MousePos.y), _playerController.TotalScale); 
        }
        else
        {
            directionVector =
                Vector2.ClampMagnitude(
                    new Vector2(_playerController.StartMousePos.x - _playerController.MousePos.x,
                        _playerController.StartMousePos.y - _playerController.MousePos.y) , _playerController.TotalScale );
        }

        Vector2 launchPosition = _ball.gameObject.transform.position;
        var launchSpeed = 5f;

        var lineRendererPoints = new List<Vector2>();
        for (int i = 0; i < steps; ++i)
        {
            calculatedPosition = launchPosition + (directionVector * (launchSpeed * i * simulationStep));
            calculatedPosition.y += Physics2D.gravity.y * (i * simulationStep) * (i * simulationStep);
            RaycastHit2D[] hit =
                Physics2D.RaycastAll(launchPosition, directionVector, maxDist * simulationStep * i);
            for (int j = 0; j < hit.Length; j++)
            {
                bool isSide = false;
                Vector2 rightVector;
                if (hit[j].collider.gameObject.CompareTag(leftBorderTag))
                {
                    if (calculatedPosition.x < hit[j].point.x)
                    {
                        rightVector = new Vector2(hit[j].point.x + (hit[j].point.x - calculatedPosition.x) , calculatedPosition.y);
                    }
                    else
                    { 
                        rightVector = new Vector2(calculatedPosition.x , calculatedPosition.y);
                    }
                    
                    lineRendererPoints.Add(rightVector);
                    isSide = true;

                }
                else if (hit[j].collider.gameObject.CompareTag(rightBorderTag))
                {
                    if (calculatedPosition.x > hit[j].point.x)
                    {
                        rightVector = new Vector2(hit[j].point.x - (calculatedPosition.x- hit[j].point.x ) , calculatedPosition.y);
                    }
                    else
                    { 
                        rightVector = new Vector2(calculatedPosition.x , calculatedPosition.y);
                    }
                    
                    lineRendererPoints.Add(rightVector);
                    isSide = true;

                }
                else if(!isSide && j==hit.Length-1)
                {
                    lineRendererPoints.Add(calculatedPosition);
                }
               
            }
            
        }
        return  lineRendererPoints;
    }
}

