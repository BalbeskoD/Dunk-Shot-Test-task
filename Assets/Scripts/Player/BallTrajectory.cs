using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BallTrajectory : MonoBehaviour
{

    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float simulateForDuration = 1f;
    [SerializeField] float simulationStep = 0.1f;

    
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

    // Update is called once per frame
    void Update()
    {
        if (_playerController.IsControlable)
        {
            if (Input.GetMouseButton(0))
            {
                if (_playerController.TotalScale > 1.4f)
                {
                    lineRenderer.positionCount = SimulateArc().Count;

                    for (int a = 0; a < lineRenderer.positionCount; a++)
                    {
                        lineRenderer.SetPosition(a, SimulateArc()[a]);
                    }
                }
                else if (_playerController.TotalScale >= 1.8f)
                {

                }
                else
                {
                    lineRenderer.positionCount = 0;
                }
            }
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            lineRenderer.positionCount = 0;
        }



    }

    private List<Vector2>  SimulateArc()
    {


        int steps = (int)(simulateForDuration / simulationStep);//50 in this example
        Vector2 directionVector;
        Vector2 calculatedPosition;
        if (_playerController.TotalScale >= 1.8)
        {
             directionVector = Vector2.ClampMagnitude(new Vector2(_playerController.StartMousePos.x - _playerController.MousePos.x, _playerController.StartMousePos.y - _playerController.MousePos.y) * 1.8f / 330, 100f);//You plug you own direction here this is just an example
            
        }
        else
        {
            directionVector = Vector2.ClampMagnitude(new Vector2(_playerController.StartMousePos.x - _playerController.MousePos.x, _playerController.StartMousePos.y - _playerController.MousePos.y) * _playerController.TotalScale / 330, 100f); ;//You plug you own direction here this is just an example

        }
        Vector2 launchPosition = _ball.gameObject.transform.position;
        float launchSpeed = 5f;
        List<Vector2> lineRendererPoints = new List<Vector2>();
        for (int i = 0; i < steps; ++i)
        {calculatedPosition = launchPosition + (directionVector * (launchSpeed * i * simulationStep));
            calculatedPosition.y += Physics2D.gravity.y * (i * simulationStep) * (i * simulationStep);
           lineRendererPoints.Add(calculatedPosition);
            
        }
        return lineRendererPoints;

    }
}
