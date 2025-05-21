using UnityEngine;

namespace Interfaces
{
    public interface IPushable
    {
        void OnPushed(Vector3 pushDirection, float strength);
    }
}
