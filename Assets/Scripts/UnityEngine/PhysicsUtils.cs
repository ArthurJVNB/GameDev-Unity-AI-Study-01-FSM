namespace UnityEngine
{
    public static class PhysicsUtils
    {
        public static Vector2 ScreenCenterPoint => new Vector2(Screen.width * .5f, Screen.height * .5f);
        public static Ray MouseRay => Camera.main.ScreenPointToRay(Input.mousePosition);

        private const float defaultMaxDistance = 1000f;

        public static bool RaycastMouse(out RaycastHit hitInfo, int layerMask = 1)
        {
            return Physics.Raycast(MouseRay, out hitInfo, defaultMaxDistance, layerMask);
        }

        public static Vector3 RaycastScreenCenter(out RaycastHit hitInfo, int layerMask = 1)
        {
            Ray ray = Camera.main.ScreenPointToRay(ScreenCenterPoint);
            if (Physics.Raycast(ray, out hitInfo, defaultMaxDistance, layerMask))
                return hitInfo.point;
            else
                return ray.GetPoint(defaultMaxDistance);
        }
    }
}