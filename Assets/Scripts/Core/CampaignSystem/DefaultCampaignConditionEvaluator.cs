using System;

namespace Geostorm.Core.CampaignSystem
{
    public sealed class DefaultCampaignConditionEvaluator : ICampaignConditionEvaluator
    {
        public bool IsSatisfied(CampaignConditionDefinition condition, CampaignState state)
        {
            if (condition == null)
            {
                return true;
            }

            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            return condition switch
            {
                StepCompletedConditionDefinition stepCompletedCondition => state.IsStepCompleted(stepCompletedCondition.StepId),

                FlagConditionDefinition flagCondition => state.GetFlag(flagCondition.FlagId) == flagCondition.ExpectedValue,

                IntVariableConditionDefinition intVariableCondition => IsIntVariableConditionSatisfied(intVariableCondition, state),

                _ => throw new NotSupportedException($"Campaign condition type '{condition.GetType().Name}' is not supported.")
            };
        }

        private static bool IsIntVariableConditionSatisfied(IntVariableConditionDefinition condition, CampaignState state)
        {
            int currentValue = state.GetIntVariable(condition.VariableId);
            int expectedValue = condition.Value;
            return condition.ComparisonOperator switch
            {
                IntVariableComparisonOperator.Equals => currentValue == expectedValue,
                IntVariableComparisonOperator.NotEquals => currentValue != expectedValue,
                IntVariableComparisonOperator.GreaterThan => currentValue > expectedValue,
                IntVariableComparisonOperator.GreaterThanOrEqual => currentValue >= expectedValue,
                IntVariableComparisonOperator.LessThan => currentValue < expectedValue,
                IntVariableComparisonOperator.LessThanOrEqual => currentValue <= expectedValue,
                _ => false
            };
        }
    }
}
