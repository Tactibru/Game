using UnityEngine;
using System.Collections;

[AddComponentMenu("Tactibru/Movement/Grid Outline")]
public class GridOutLineBehaviour : MonoBehaviour
{

    public GridBehavior theGrid;
    public LineRenderer theLine;

    void Start()
    {
        theGrid = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridBehavior>();
        theLine.SetWidth(0.1f, 0.1f);
        //theLine.SetVertexCount();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (MovePointBehavior movePoint in theGrid.theMap)
        {
 
        }
    }
}
