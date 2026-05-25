using System.Collections.Generic;

namespace Geostorm.Core.CharacterSystem
{
    public interface ICharacterBrain
    {
        void GenerateCommands(List<ICommand> commandBuffer);
        void OnPossess(IPawn targetPawn, object context = null);
        void OnUnpossess();
    }
}