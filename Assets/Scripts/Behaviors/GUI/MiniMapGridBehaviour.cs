using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public GameControllerBehaviour gameController;
    public MiniMapActorBehavior miniMapPlayer;
    public MiniMapActorBehavior miniMapEnemy;
    public MiniMapActorBehavior miniMapNeutral;

    public List<MiniMapActorBehavior> playerSquadList = new List<MiniMapActorBehavior>();
    public List<MiniMapActorBehavior> neutralSquadList = new List<MiniMapActorBehavior>();
    public List<MiniMapActorBehavior> enemySquadList = new List<MiniMapActorBehavior>();

    public int miniMapWidth;
    public int miniMapLength;
 
    char[] abc = new char[30] {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd'};

    void Start()
    {
        theMiniMap = new MiniMapPointBehaviour[theGrid.theMapLength * theGrid.theMapWidth];
        gameController = GameObject.FindGameObjectWithTag("Grid").GetComponent<GameControllerBehaviour>();

        miniMapWidth = theGrid.theMapWidth;
        miniMapLength = theGrid.theMapLength;

        float xPositionOffset = -(miniMapWidth / 2.0f);
        float yPositionOffset = -(miniMapLength / 2.0f);
        float currentXPosition = 0.0f;
        float currentYPosition = 0.0f;

        for (int x = 0; x < miniMapLength; x++)
        {
            currentXPosition = xPositionOffset * 0.1f + 0.05f;
            currentYPosition = (yPositionOffset + x + 0.5f) * 0.1f;

            for (int z = 0; z < miniMapWidth; z++)
            {
                MiniMapPointBehaviour newMiniMapPoint = null;
                newMiniMapPoint = (MiniMapPointBehaviour)Instantiate(miniPointPrefab, new Vector3(currentXPosition, 0.0f, currentYPosition), Quaternion.identity);
                newMiniMapPoint.transform.parent = transform;
                newMiniMapPoint.name = abc[z].ToString() + x.ToString();
                newMiniMapPoint.transform.localPosition = new Vector3(currentXPosition, currentYPosition, -0.01f);
                currentXPosition = (xPositionOffset + z + 1) * 0.1f + 0.05f;
                theMiniMap[z + (x * miniMapWidth)] = newMiniMapPoint;
            }
        }

        for (int x = 0; x < miniMapLength; x++)
        {
            for (int z = 0; z < miniMapWidth; z++)
            {
                if (!theGrid.theMap[z + (x * miniMapWidth)])
                {
                    Destroy(theMiniMap[z + (x * miniMapWidth)].gameObject);
                }
            }
        }

        for (int i = 0; i < gameController.playerTeam.Count; i++)
        {
            MiniMapActorBehavior newMiniMapPlayer = null;
            newMiniMapPlayer = (MiniMapActorBehavior)Instantiate(miniMapPlayer, new Vector3(0, 0.0f, 0.5f), Quaternion.identity);
            newMiniMapPlayer.transform.parent = transform;
            newMiniMapPlayer.transform.localPosition = new Vector3(0.0f, 0.0f, 0.1f);

            playerSquadList.Add(newMiniMapPlayer);
        }

        for (int i = 0; i < gameController.enemyTeam.Count; i++)
        {
            MiniMapActorBehavior newMiniMapEnemy = null;
            newMiniMapEnemy = (MiniMapActorBehavior)Instantiate(miniMapEnemy, new Vector3(0, 0.0f, 0.5f), Quaternion.identity);
            newMiniMapEnemy.transform.parent = transform;
            newMiniMapEnemy.transform.localPosition = new Vector3(0.0f, 0.0f, 0.1f);

            enemySquadList.Add(newMiniMapEnemy);
        }

        for (int i = 0; i < gameController.nuetrals.Count; i++)
        {
            MiniMapActorBehavior newMiniMapNuetral = null;
            newMiniMapNuetral = (MiniMapActorBehavior)Instantiate(miniMapNeutral, new Vector3(0, 0.0f, 0.5f), Quaternion.identity);
            newMiniMapNuetral.transform.parent = transform;
            newMiniMapNuetral.transform.localPosition = new Vector3(0.0f, 0.0f, 0.1f);

            neutralSquadList.Add(newMiniMapNuetral);
        }

    }

    public void UpdateMiniMap()
    {
        for (int i = 0; i < playerSquadList.Count; i++)
        {
            playerSquadList[i].currentPosition = null;
            playerSquadList[i].transform.localPosition = new Vector3(0.0f, 0.0f, 0.1f);


        }

        for (int i = 0; i < gameController.playerTeam.Count; i++)
            playerSquadList[i].currentPosition = theMiniMap[gameController.playerTeam[i].currentMovePoint.index];

        for (int i = 0; i < enemySquadList.Count; i++)
        {
            enemySquadList[i].currentPosition = null;
            enemySquadList[i].transform.localPosition = new Vector3(0.0f, 0.0f, 0.1f);
        }

        for (int i = 0; i < gameController.enemyTeam.Count; i++)
            enemySquadList[i].currentPosition = theMiniMap[gameController.enemyTeam[i].currentMovePoint.index];

        for (int i = 0; i < neutralSquadList.Count; i++)
        {
            neutralSquadList[i].currentPosition = null;
            neutralSquadList[i].transform.localPosition = new Vector3(0.0f, 0.0f, 0.1f);


        }

        for (int i = 0; i < gameController.nuetrals.Count; i++)
            neutralSquadList[i].currentPosition = theMiniMap[gameController.nuetrals[i].currentMovePoint.index];
    }

    void Update()
    {
        UpdateMiniMap();
    }
}