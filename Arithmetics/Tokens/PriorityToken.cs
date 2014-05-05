using HPMSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hansoft.ObjectWrapper;
using Hansoft.ObjectWrapper.CustomColumnValues;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Tokens
{
    class PriorityToken : IExpressionItem, IAssignable
    {
        /*
         * Returns the value of this token in the incoming task. 
         */
        public ExpressionValue Evaluate(Task task)
        {
            return new ExpressionValue(ExpressionValueType.STRING, task.Priority.ToString());
        }
        
        
        public void AddAffectedBy(ref List<ListenerData> list)
        {
            list.Add(new ListenerData(EHPMTaskField.BacklogPriority));
            list.Add(new ListenerData(EHPMTaskField.BugPriority));
            list.Add(new ListenerData(EHPMTaskField.SprintPriority));
        }

        /*
         * An object that implements this class can be assigned values.
         * This function will be called when an item is on the left hand side of an
         * assignment.
         */
        public void SetValue(Task task, ExpressionValue value)
        {
            if((task as ProductBacklogItem)  != null)   
                task.Priority = HansoftEnumValue.FromString(task.ProjectID, EHPMProjectDefaultColumn.BacklogPriority, value.ToString());
            else if ((task as SprintBacklogItem) != null) 
                task.Confidence = HansoftEnumValue.FromString(task.ProjectID, EHPMProjectDefaultColumn.SprintPriority, value.ToString());
            else if ((task as Bug) != null)
                task.Confidence = HansoftEnumValue.FromString(task.ProjectID, EHPMProjectDefaultColumn.BugPriority, value.ToString());
        }
    }
}