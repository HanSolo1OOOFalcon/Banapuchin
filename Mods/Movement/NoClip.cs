using Banapuchin.Classes;
using Banapuchin.Libraries;
using UnityEngine;

namespace Banapuchin.Mods.Movement
{
    public class NoClip : ModBase
    {
        public override string Text => "No Clip";
        
        private static Dictionary<MeshCollider, bool> _meshColliderStates = new Dictionary<MeshCollider, bool>();

        public override void Update()
        {
            if (ControllerInput.instance.GetInputDown(ControllerInput.InputType.RightTrigger))
            {
                MeshCollider[] meshColliders = Resources.FindObjectsOfTypeAll<MeshCollider>();
                foreach (MeshCollider meshCollider in meshColliders)
                {
                    _meshColliderStates[meshCollider] = meshCollider.enabled;
                    meshCollider.enabled = false;
                }
            }
            else if (ControllerInput.instance.GetInputUp(ControllerInput.InputType.RightTrigger))
            {
                if (_meshColliderStates.Count > 0)
                {
                    foreach (KeyValuePair<MeshCollider, bool> meshColliderState in _meshColliderStates)
                        meshColliderState.Key.enabled = meshColliderState.Value;
                    _meshColliderStates.Clear();
                }
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (_meshColliderStates.Count > 0)
            {
                foreach (KeyValuePair<MeshCollider, bool> meshColliderState in _meshColliderStates)
                    meshColliderState.Key.enabled = meshColliderState.Value;
                _meshColliderStates.Clear();
            }
        }
    }
}