using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class TouchDragControl : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

    private Vector2 lastPoint;
    private bool isDragging = false;
    private Vector2 currentDelta;

    CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis;
    CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis;

    public string horizontalAxisName = "Mouse X";
    public string verticalAxisName = "Mouse Y";
    public float sensitivity = 0.1f; // Lower sensitivity usually works better for raw pixels

    void OnEnable() { CreateVirtualAxes(); }

    void CreateVirtualAxes()
    {
        m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
        CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);

        m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
        CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
    }

    void Update()
    {
        if (isDragging)
        {
            // Apply the delta stored from the last OnDrag call
            m_HorizontalVirtualAxis.Update(currentDelta.x * sensitivity);
            m_VerticalVirtualAxis.Update(currentDelta.y * sensitivity);

            // CRITICAL: Reset delta after applying so it doesn't spin when finger stays still
            currentDelta = Vector2.zero;
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        lastPoint = eventData.position;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        // Calculate movement since the last frame
        currentDelta = eventData.position - lastPoint;
        lastPoint = eventData.position;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        currentDelta = Vector2.zero;
        m_HorizontalVirtualAxis.Update(0);
        m_VerticalVirtualAxis.Update(0);
    }

    void OnDisable()
    {
        m_HorizontalVirtualAxis.Remove();
        m_VerticalVirtualAxis.Remove();
    }
}