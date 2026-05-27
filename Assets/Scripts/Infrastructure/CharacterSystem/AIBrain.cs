using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Geostorm.Core.CharacterSystem;

namespace Geostorm.Infrastructure.CharacterSystem
{
    public class AIBrain : ICharacterBrain
    {
        private IPawn _controlledPawn;
        private IPawn _targetPawn;
        private NavMeshPath _path;

        private bool _forceApplyRotationMode;

        public AIBrain()
        {
            _path = new NavMeshPath();
        }

        public void OnPossess(IPawn targetPawn, object context = null)
        {
            _controlledPawn = targetPawn;
            _targetPawn = context as IPawn;
            _forceApplyRotationMode = true;
        }

        public void OnUnpossess()
        {
            _controlledPawn = null;
            _targetPawn = null;
            _forceApplyRotationMode = false;
        }

        public void GenerateCommands(List<ICommand> commandBuffer)
        {
            if (_controlledPawn == null || _targetPawn == null)
            {
                return;
            }

            if (_forceApplyRotationMode)
            {
                _forceApplyRotationMode = false;
                commandBuffer.Add(SetRotationModeCommand.Get(PawnRotationMode.MovementDirection));
            }

            if (NavMesh.SamplePosition(_controlledPawn.Position, out NavMeshHit startHit, 2.0f, NavMesh.AllAreas) &&
                NavMesh.SamplePosition(_targetPawn.Position, out NavMeshHit targetHit, 2.0f, NavMesh.AllAreas))
            {
                NavMesh.CalculatePath(_controlledPawn.Position, _targetPawn.Position, NavMesh.AllAreas, _path);

                if (_path.status == NavMeshPathStatus.PathComplete && _path.corners.Length > 1)
                {
                    Vector3 direction = _path.corners[1] - _controlledPawn.Position;
                    direction.y = 0.0f;
                    if (direction.sqrMagnitude > 0.01f)
                    {
                        commandBuffer.Add(MoveCommand.Get(direction.normalized));
                    }
                }
            }
        }
    }
}