using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HPMSdk;
using Hansoft.ObjectWrapper;


namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Tokens
{
    class Tokenizer
    {
        public static IExpressionItem Compile(IntermediateToken token)
        {
            int value;
            bool isInt = int.TryParse(token.Token, out value);
            if (isInt)
                return new IntToken(value);
            bool state;
            bool isBool = bool.TryParse(token.Token, out state);
            if (isBool)
                return new BoolToken(state);
            float fVal;
            bool isFloat = float.TryParse(token.Token, out fVal);
            if (isFloat)
                return new FloatToken(fVal);

            if (token.Token.StartsWith("$"))
            {
                string columnName = token.Token.Substring(1, token.Token.Length-1);
                switch(columnName)
                {
                    case ("ComplexityPoints"):
                        return new ComplexityPointsToken();
                    case ("WorkflowStatus"):
                        return new WorkflowStatusToken();
                    case ("Confidence"):
                        return new DefaultEnumColumnToken(EHPMTaskField.Confidence, EHPMProjectDefaultColumn.Confidence, "Confidence");
                    case ("Priority"):
                        return new PriorityToken();
                    case ("Risk"):
                        return new DefaultEnumColumnToken(EHPMTaskField.Risk, EHPMProjectDefaultColumn.Risk, "Risk");
                    case ("Severity"):
                        return new DefaultEnumColumnToken(EHPMTaskField.Severity, EHPMProjectDefaultColumn.Severity, "Severity");
                    case ("EstimatedIdealDays"):
                        return new DefaultFloatColumnToken(EHPMTaskField.EstimatedIdealDays, EHPMProjectDefaultColumn.EstimatedIdealDays, "EstimatedIdealDays");                        
                    case ("Duration"):
                        return new DefaultIntColumnToken(EHPMTaskField.Duration, EHPMProjectDefaultColumn.Duration, "Duration");                        
                    case ("Status"):
                        return new DefaultEnumColumnToken(EHPMTaskField.Status, EHPMProjectDefaultColumn.Status, "Status");
                    case ("Hyperlink"):
                        return new DefaultStringColumnToken(EHPMTaskField.Hyperlink, EHPMProjectDefaultColumn.Hyperlink, "Hyperlink");
                    case ("StepsToReproduce"):
                        return new DefaultStringColumnToken(EHPMTaskField.StepsToReproduce, EHPMProjectDefaultColumn.StepsToReproduce, "StepsToReproduce");
                    case ("DetailedDescription"):
                        return new DefaultStringColumnToken(EHPMTaskField.DetailedDescription, EHPMProjectDefaultColumn.DetailedDescription, "DetailedDescription");
                    case ("WorkRemaining"):
                        return new DefaultFloatColumnToken(EHPMTaskField.WorkRemaining, EHPMProjectDefaultColumn.WorkRemaining, "WorkRemaining");                        

                    case ("SprintResourceAllocation"):
                    case ("LastUpdatedTime"):
                    case ("UserStoryFlag"):
                    case ("Completed"):
                    case ("TimeZones"):
                    case ("VisibleTo"):
                    case ("LinkedTo"):
                    case ("TimeZoneStart"):
                    case ("TimeZoneEnd"):
                    case ("Workflow"):
                    case ("TotalDuration"):
                    case ("LinkedToPipelineTask"):
                    case ("CreatedPipelineTasks"):
                    case ("StartOffset"):
                    case ("BudgetedWork"):
                    case ("PercentComplete"):
                    case ("Type"):
                    case ("Comment"):
                    case ("Description"):
                    case ("LinkedToMilestone"):
                    case ("LinkedToSprint"):
                    case ("AttachedDocuments"):
                    case ("DelegateTo"):
                    case ("SprintAllocatedResources"):

                        throw new ArgumentException("Not implemented yet");
                    default:
                        return new CustomColumnToken(columnName);
                }
            }
            else // Constant tokens
            {
                switch(token.Token)
                {
                    case ("TIMENOW"):
                        return new TimeNowToken();
                    default:
                        return new StringToken(token.Token);
                }
            }
        }
    }
}
