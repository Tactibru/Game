// Generated with Antares LogicBlock Wizard at Wednesday, February 09, 2011  7:06:05 AM
//Created by Neodrop
using Antares.Vizio.Runtime;
using UnityEngine;

[VisualLogicBlockDescription("LB for dragging area inside GUI Group. For example - for maps inside Window")]
[VisualLogicBlock("Easy Map Dragging", "GUI", ParentName = "Map")]
public class EasyMap : LogicBlock
{
    [Parameter(VariableType.In, typeof(Rect), Name = "Main Rect (Not changed)")]
    public Variable rectContainerIn;

    [Parameter(VariableType.In, typeof(Rect), Name = "Rect Inside")]
    public Variable rectInsideIn;

    [Parameter(VariableType.In, typeof(float), Name = "Zoom Value")]
    public Variable zoomInValue;

    [Parameter(VariableType.In, typeof(Vector2), Name = "Zoom Limits")]
    public Variable zoomInLimits;

    [Parameter(VariableType.In, typeof(float), Name = "Zoom Speed")]
    public Variable zoomInSpeed;

    [Parameter(VariableType.Out, typeof(Rect), Name = "Main Rect")]
    public Variable rectContainerOut;

    [Parameter(VariableType.Out, typeof(Rect), Name = "Rect Inside")]
    public Variable rectInsideOut;

    [Parameter(VariableType.Out, typeof(float), Name = "Zoom")]
    public Variable zoomOut;

    [Parameter(VariableType.Out, typeof(Vector2), Name = "Mouse Relative Position")]
    public Variable mousePositionOut;

    [Parameter(VariableType.Out, typeof(bool), Name = "Mouse Over Main Rect")]
    public Variable mouseOverOut;

    private int _mouseButton = 0;
    private Vector2 _clickPosition;
    private Vector2 _shift;
    private float _zoom = 1;
    private bool _onDrag;

    [EntryTrigger("Call only inside OnGUI !")]
    public void In()
    {
        _zoom = (float)zoomInValue.Value;
        if (_onDrag && Input.GetMouseButtonUp(_mouseButton))
            _onDrag = false;

        Rect rectCont = (Rect) rectContainerIn.Value;
        Rect rectInside = (Rect) rectInsideIn.Value;
        rectInside.width *= _zoom;
        rectInside.height *= _zoom;

        bool over = rectCont.Contains(Event.current.mousePosition);
        mouseOverOut.Value = over;
        if (over && Input.GetMouseButtonDown(_mouseButton))
        {
            _clickPosition = Event.current.mousePosition + _shift;
            _onDrag = true;
        }

        if (_onDrag && Input.GetMouseButton(_mouseButton))
        {
            mousePositionOut.Value = Event.current.mousePosition;
            _shift.x = Event.current.mousePosition.x - _clickPosition.x;
            _shift.y = Event.current.mousePosition.y - _clickPosition.y;
        }

        Vector2 zoomLimits = (Vector2)zoomInLimits.Value;
        if (over && zoomLimits != Vector2.zero && Event.current.type == EventType.layout && Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float zoomBefore = _zoom;
            _zoom += Input.GetAxis("Mouse ScrollWheel") * (float)zoomInSpeed.Value;
            _zoom = Mathf.Clamp(_zoom, zoomLimits.x, zoomLimits.y);  

            float delta = (_zoom - zoomBefore)/zoomBefore;
            _shift += (_shift - Event.current.mousePosition)*delta;
        }
        
        zoomOut.Value = _zoom;
        rectInside.x += _shift.x;
        rectInside.y += _shift.y;
        //if (rectInside.x > 0)
        //{
        //    rectInside.x = 0;
        //    _shift.x = 0;
        //}

        //if (rectInside.y > 0)
        //{
        //    rectInside.y = 0;
        //    _shift.y = 0;
        //}

        rectInsideOut.Value = rectInside;
        rectContainerOut.Value = rectContainerIn.Value;

        ActivateTrigger();
        ActivateCustomTriggers();
    }

    public override void OnInitializeDefaultData()
    {
        RegisterOutputTrigger("Out");
        zoomInLimits.Value = new Vector2(.1f, 2);
        zoomInSpeed.Value = .1f;
    }

    public override bool GetUseCustomTriggers()
    {
        return true;
    }

    public override bool GetIsCoroutine()
    {
        return false;
    }
}
