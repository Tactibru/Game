using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// 
/// Darryl Sterne
/// </summary>

public class MiniMapGridBehaviour : MonoBehaviour 
{
    public MiniMapPointBehaviour[] theMiniMap;
    public GridBehavior theGrid;
    public MiniMapPointBehaviour miniPointPrefab;

    public int miniMapWidth;
    public int miniMapLength;

    public void CreateMiniMap()
    {
        //theMiniMap = new MiniMapPointBehaviour[theGrid.theMapLength * theGrid.theMapWidth];

        //miniMapWidth = theGrid.theMapWidth;
        //miniMapLength = theGrid.theMapLength;

        //float xPositionOffset = -(miniMapWidth / 2.0f);
        //float yPositionOffset = -(miniMapLength / 2.0f);
        //float currentXPosition = 0.0f;
        //float currentYPosition = 0.0f;

        //for (int x = 0; x < miniMapLength; x++)
        //{
        //    currentXPosition = xPositionOffset;
        //    currentYPosition = (yPositionOffset + x) * 0.1f;

        //    for (int z = 0; z < miniMapWidth; z++)
        //    {
        //        MiniMapPointBehaviour newMiniMapPoint = null;

        //        newMiniMapPoint = (MiniMapPointBehaviour)Instantiate(miniPointPrefab, new Vector3(currentXPosition, 0.0f, currentYPosition), Quaternion.identity);
        //        newMiniMapPoint.transform.parent = transform;
        //        newMiniMapPoint.transform.localPosition = new Vector3(currentXPosition, currentYPosition);
        //        currentXPosition = (xPositionOffset + z + 1) * 0.1f;

        //    }
        //}
    }

    void Start() 
    {
        theMiniMap = new MiniMapPointBehaviour[theGrid.theMapLength * theGrid.theMapWidth];

        miniMapWidth = theGrid.theMapWidth;
        miniMapLength = theGrid.theMapLength;

        float xPositionOffset = -(miniMapWidth / 2.0f);
        float yPositionOffset = -(miniMapLength / 2.0f);
        float currentXPosition = 0.0f;
        float currentYPosition = 0.0f;

        for (int x = 0; x < miniMapLength; x++)
        {
            currentXPosition = xPositionOffset;
            currentYPosition = (yPositionOffset + x + 0.5f) * 0.1f;

            for (int z = 0; z < miniMapWidth; z++)
            {
                MiniMapPointBehaviour newMiniMapPoint = null;

                newMiniMapPoint = (MiniMapPointBehaviour)Instantiate(miniPointPrefab, new Vector3(currentXPosition, 0.0f, currentYPosition), Quaternion.identity);
                newMiniMapPoint.transform.parent = transform;
                newMiniMapPoint.transform.localPosition = new Vector3(currentXPosition, currentYPosition, -0.01f);
                currentXPosition = (xPositionOffset + z + 1) * 0.1f;

            }
        }
	}
}
