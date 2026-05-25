using System.Collections.Generic;
using UnityEngine;
using VContainer;
using Geostorm.Core.CharacterSystem;

namespace Geostorm.Infrastructure.CharacterSystem
{
    [RequireComponent(typeof(CharacterController))]
    public class Pawn : MonoBehaviour, IPawn
    {
        private PossessionManager _possessionManager;
        private CharacterController _characterController;
        private Vector3 _velocity;
        private Vector3 _currentMoveIntent;

        private readonly List<ICommand> _commandBuffer = new();

        [SerializeField]
        private PawnData _data;

        public ICharacterBrain CurrentBrain { get; private set; }
        public Vector3 Position => transform.position;

        [Inject]
        public void Construct(PossessionManager possessionManager)
        {
            _possessionManager = possessionManager;
            if (isActiveAndEnabled)
            {
                _possessionManager.RegisterPawn(this);
            }
        }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            if (_possessionManager != null)
            {
                _possessionManager.RegisterPawn(this);
            }
        }

        private void OnDisable()
        {
            if (_possessionManager != null)
            {
                _possessionManager.UnregisterPawn(this);
            }
        }

        private void Update()
        {
            if (CurrentBrain != null)
            {
                CurrentBrain.GenerateCommands(_commandBuffer);
            }

            _currentMoveIntent = Vector3.zero;

            for (int i = _commandBuffer.Count - 1; i >= 0; i--)
            {
                ICommand cmd = _commandBuffer[i];

                if (cmd.Execute(this) || Time.time > cmd.ExpirationTime)
                {
                    cmd.Release();
                    _commandBuffer.RemoveAt(i);
                }
            }

            ApplyGravity();
            ApplyMovement();
        }

        public void SetBrain(ICharacterBrain newBrain)
        {
            CurrentBrain = newBrain;
            _commandBuffer.Clear();
        }

        public void Move(Vector3 direction)
        {
            _currentMoveIntent = direction;
        }

        private void ApplyMovement()
        {
            Vector3 movement = _currentMoveIntent * _data.MoveSpeed;
            _characterController.Move((movement + _velocity) * Time.deltaTime);

            if (_currentMoveIntent != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_currentMoveIntent);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10.0f);
            }
        }

        private void ApplyGravity()
        {
            if (_characterController.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2.0f;
            }
            _velocity.y += _data.Gravity * Time.deltaTime;
        }
    }
}