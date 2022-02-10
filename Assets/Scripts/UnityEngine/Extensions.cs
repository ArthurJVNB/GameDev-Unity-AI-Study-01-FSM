using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
	public static class Extensions
	{
		public static bool MousePositionInWorld(out RaycastHit hitInfo, int layerMask)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			return Physics.Raycast(ray, out hitInfo, 1000f, layerMask);
        }
	}
}
