using UnityEngine;

namespace Synty.AnimationBaseLocomotion.Samples
{
    /// <summary>
    /// Handles lock-on target behavior.
    /// Manages highlight effects and interaction with the player's animation controller.
    /// </summary>
    public class ObjectLockOn : MonoBehaviour
    {
        [Header("Highlight Materials")]
        [Tooltip("Material used when the object is hovered")]
        public Material _highlightMat;

        [Tooltip("Material used when the object is locked on")]
        public Material _targetMat;

        // Reference to the highlight visual object
        private Transform _highlightOrb;

        // Renderer for the highlight material
        private MeshRenderer _meshRenderer;

        private void Start()
        {
            // Find the child object responsible for the highlight
            _highlightOrb = transform.Find("TargetHighlight");
            _meshRenderer = _highlightOrb?.GetComponent<MeshRenderer>();

            // Ensure the MeshRenderer exists
            if (_meshRenderer == null)
            {
                Debug.LogError("MeshRenderer is required on the 'TargetHighlight' object.");
            }
        }

        private void OnTriggerEnter(Collider otherCollider)
        {
            // Try to get the player animation controller from the entering object
            var playerAnimationController = otherCollider.GetComponent<PlayerAnimationController>();

            // Register this object as a lock-on target candidate
            if (playerAnimationController != null)
            {
                playerAnimationController.AddTargetCandidate(gameObject);
            }
        }

        private void OnTriggerExit(Collider otherCollider)
        {
            // Try to get the player animation controller from the exiting object
            var playerAnimationController = otherCollider.GetComponent<PlayerAnimationController>();

            // Remove this object as a target and disable its highlight
            if (playerAnimationController != null)
            {
                playerAnimationController.RemoveTarget(gameObject);
                Highlight(false, false);
            }
        }

        /// <summary>
        /// Toggles the highlight visual on or off.
        /// </summary>
        /// <param name="enable">Whether to enable the highlight</param>
        /// <param name="targetLock">True if the object is currently locked on</param>
        public void Highlight(bool enable, bool targetLock)
        {
            if (_highlightOrb == null) return;

            _highlightOrb.gameObject.SetActive(enable);

            if (enable)
            {
                // Set the appropriate material based on lock-on status
                Material currentMaterial = targetLock ? _targetMat : _highlightMat;
                _meshRenderer.material = currentMaterial;
            }
        }
    }
}
