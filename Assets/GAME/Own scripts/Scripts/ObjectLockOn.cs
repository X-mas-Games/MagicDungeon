
using UnityEngine;
namespace Synty.AnimationBaseLocomotion.Samples
{
    public class ObjectLockOn : MonoBehaviour
    {
        public Material _highlightMat;
        public Material _targetMat;
        private Transform _highlightOrb;

        private MeshRenderer _meshRenderer;

       
        private void Start()
        {
            _highlightOrb = transform.Find("TargetHighlight");
            _meshRenderer = _highlightOrb.GetComponent<MeshRenderer>();

            if (_meshRenderer == null)
            {
                Debug.LogError("This script requires a MeshRenderer component on the GameObject.");
            }
        }

        
        private void OnTriggerEnter(Collider otherCollider)
        {
            PlayerAnimationController playerAnimationController = otherCollider.GetComponent<PlayerAnimationController>();

            
            if (playerAnimationController != null)
            {
                playerAnimationController.AddTargetCandidate(gameObject);
            }
        }

        
        private void OnTriggerExit(Collider otherCollider)
        {
            PlayerAnimationController playerAnimationController = otherCollider.GetComponent<PlayerAnimationController>();

            
            if (playerAnimationController != null)
            {
                playerAnimationController.RemoveTarget(gameObject);
                Highlight(false, false);
            }
        }

        
        public void Highlight(bool enable, bool targetLock)
        {
            Material currentMaterial = targetLock ? _targetMat : _highlightMat;

            if (_highlightOrb != null)
            {
                _highlightOrb.gameObject.SetActive(enable);
                if (enable)
                {
                    _meshRenderer.material = currentMaterial;
                }
            }
        }
    }
}
