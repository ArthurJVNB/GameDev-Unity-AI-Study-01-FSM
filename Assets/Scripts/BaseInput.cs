using System;
using System.Collections;
using UnityEngine;

namespace Project
{
    public abstract class BaseInput : MonoBehaviour
    {
        public Action<Vector2> OnInputMovement;
    }
}