using UnityEngine;
using Zenject;

namespace dirox.emotiv.controller
{
    /// <summary>
    /// Board of examples
    /// </summary>
    public class MainBoard : BaseCanvasView
    {
        public override void Activate()
        {
            Debug.Log("ExamplesBoard: Activate");
            base.Activate();
        }
    }
}
