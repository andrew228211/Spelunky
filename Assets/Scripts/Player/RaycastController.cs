using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RaycastController;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
    public LayerMask collisionMask;
	
	public const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    [HideInInspector]
    public float horizontalRaySpacing;
    [HideInInspector]
    public float verticalRaySpacing;

    [HideInInspector]
    public new BoxCollider2D collider;
    public RaycastOrigins raycastOrigins;
    public virtual void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }
    public void UpdateRayCastOrigins()
    {
        //Bound dai dien cho 1 Axis Aligned Bounding Box(AABB), chi co vi tri diem trung tam va pham vi moi truc.
        Bounds bounds = collider.bounds;

        /*Neu co 1 AABB o tam moi canh co chieu dai 1 thi no se giong nhu sau
            vi tri (0.0, 0.0, 0.0)
            Mo rong(0.5, 0.5, 0.5)
            Mo rong 1 nua chieu dai 1 axis. Khi do neu goi Bounds.expand(amount) thi mo rong pham vi theo chi so amount/2
            Vi vay neu muon mo rong theo so luong tren ca 2 mat truc am va duong cua truc thi phai nhan 2. -2 la de cac tia o trong .
         */
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }
    public void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
