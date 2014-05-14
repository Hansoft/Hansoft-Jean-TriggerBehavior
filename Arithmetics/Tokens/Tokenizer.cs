using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HPMSdk;
using Hansoft.ObjectWrapper;
using System.Globalization;


namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Tokens
{
    class Tokenizer
    {
        /// <summary>
        /// Creates a token from the expression.
        /// </summary>
        /// <param name="expression">The expression that will be parsed</param>
        /// <param name="errors">A reference to a list of strings that will be filled up with all the parse errors</param>
        /// <returns>An token that will be able to evaluated on a task. Null if there are any errors in the parsing process.</returns>
        public static IExpressionItem Compile(string token, ref List<string> errors)
        {
            token = token.Trim();
            if (token == "")
                return null;
            int value;
            bool isInt = int.TryParse(token, out value);
            if (isInt)
                return new IntToken(value);
            bool state;
            bool isBool = bool.TryParse(token, out state);
            if (isBool)
                return new BoolToken(state);
            try
            {
                float fVal = float.Parse(token, CultureInfo.InvariantCulture);
                return new FloatToken(fVal);
            }
            catch(Exception)
            {
                //Yes, ugly. But tryParse does not allow for cultural differences
            }

            if (token.StartsWith("$"))
            {
                string columnName = token.Substring(1, token.Length-1);
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
                    case ("ItemName"):
                    case ("Description"):
                    case ("Name"): //Covering all bases on the item name attribute
                        return new DefaultStringColumnToken(EHPMTaskField.Description, EHPMProjectDefaultColumn.ItemName, "Name");
                    case ("StepsToReproduce"):
                        return new DefaultStringColumnToken(EHPMTaskField.StepsToReproduce, EHPMProjectDefaultColumn.StepsToReproduce, "StepsToReproduce");
                    case ("DetailedDescription"):
                        return new DefaultStringColumnToken(EHPMTaskField.DetailedDescription, EHPMProjectDefaultColumn.DetailedDescription, "DetailedDescription");
                    case ("WorkRemaining"):
                        return new DefaultFloatColumnToken(EHPMTaskField.WorkRemaining, EHPMProjectDefaultColumn.WorkRemaining, "WorkRemaining");
                    case ("Assignees"):
                        return new AssigneesToken();               
         
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
                    case ("LinkedToMilestone"):
                    case ("LinkedToSprint"):
                    case ("AttachedDocuments"):
                    case ("DelegateTo"):
                    case ("SprintAllocatedResources"):
                        errors.Add(columnName + " is not yet implemented");
                        return new UnknownToken();
                    default:
                        return new CustomColumnToken(columnName);
                }
            }
            else
            {
                switch(token)
                {
                    case ("TIMENOW"):
                        return new TimeNowToken();
                    default:
                        errors.Add("Unkown token: " + token);
                        return new UnknownToken();
                }
            }
        }
    }
}
