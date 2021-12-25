using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ItemSystem
{
    public class WorldIconUI : MonoBehaviour
    {
        private enum ImageState
        {
            idle, hold, interact
        }

        private ImageState currentState;

        private BehaviorStateManager item;
        private InteractController interactController;
        [SerializeField] private AnimationCurve fadeCurve;

        [SerializeField] private Image idleImage;
        [SerializeField] private Image[] holdImage = new Image[2];
        [SerializeField] private Image interactImage;

        private Transform cameraTr;
        private Transform playerTr;

        private float multiplier = 0.15f;

        private float minDistance = 1.25f;

        private void Awake()
        {
            item = GetComponentInParent<BehaviorStateManager>();
        }

        private void Start()
        {
            interactController = InteractController.Controller;
            cameraTr = InteractController.camTr;
            playerTr = PlayerController.AdvancedWalkerController.PlayerTR;
            SetIdle();
        }


        private void LateUpdate()
        {
            if (interactController.currentInteraction != item && !idleImage.enabled) SetIdle();

            float _distance = Vector3.Distance(transform.position, playerTr.position) * multiplier;

            if (_distance <= minDistance + 0.24f) FaceCamera();

            switch (currentState)
            {
                case ImageState.idle:
                    if (_distance <= minDistance && idleImage.enabled)
                    { 
                        FadeOpacity(_distance);
                    }
                    break;
                case ImageState.hold:
                    FillBar();
                    break;
                case ImageState.interact:
                    break;
            }
        }

        private void FaceCamera()
        {
            transform.rotation = Quaternion.LookRotation(cameraTr.position - transform.position);
        }
        private void FillBar()
        {
            holdImage[1].fillAmount = interactController.holdPercent;
        }

        private void FadeOpacity(float _distance)
        {
            Color _alpha = idleImage.color;
            _alpha.a = fadeCurve.Evaluate(minDistance - _distance);

            idleImage.color = _alpha;
        }

        private void SetIdle()
        {
            holdImage[1].fillAmount = 0f;
            interactImage.enabled = false;
            holdImage[0].enabled = false;
            idleImage.enabled = true;
            currentState = ImageState.idle;
        }

        public void SwapImage(bool _isHold)
        {
            if (_isHold)
            {
                holdImage[1].fillAmount = 0f;
                idleImage.enabled = false;
                interactImage.enabled = false;
                holdImage[0].enabled = true;
                currentState = ImageState.hold;
            }
            else
            {
                holdImage[1].fillAmount = 0f;
                holdImage[0].enabled = false;
                idleImage.enabled = false;
                interactImage.enabled = true;
                currentState = ImageState.interact;
            }
        }
    }
}

