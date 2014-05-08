using HPMSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hansoft.ObjectWrapper;
using Hansoft.ObjectWrapper.CustomColumnValues;
using Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Value;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Tokens
{
    class PriorityToken : IExpressionItem, IAssignable
    {
        /*
         * Returns the value of this token in the incoming task. 
         */
        public ExpressionValue Evaluate(Task task)
        {
            return new StringExpressionValue(task.Priority.ToString());
        }
        

        /// <summary>
        /// Adds the field this token is listening to to the incoming list.
        /// </summary>
        /// <param name="list">the list to add the field to</param>
        public void AddAffectedBy(ref List<ListenerData> list)
        {
            list.AddRange(GetAssignmentFields());
        }

        /// <summary>
        /// Returns the field that this assignment will affect.
        /// </summary>
        /// <returns>the field that this assignment will affect</returns>
        public List<ListenerData> GetAssignmentFields()
        {
            List<ListenerData> list = new List<ListenerData>();
            list.Add(new ListenerData(EHPMTaskField.BacklogPriority));
            list.Add(new ListenerData(EHPMTaskField.BugPriority));
            list.Add(new ListenerData(EHPMTaskField.SprintPriority));
            return list;
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