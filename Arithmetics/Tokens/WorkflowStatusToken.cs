using HPMSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hansoft.ObjectWrapper;
using Hansoft.ObjectWrapper.CustomColumnValues;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Tokens
{
    class WorkflowStatusToken : IExpressionItem, IAssignable
    {
        /*
         * Returns the value of this token in the incoming task. 
         */
        public ExpressionValue Evaluate(Task task)
        {
            return new ExpressionValue(ExpressionValueType.STRING, task.WorkflowStatusString);
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
            list.Add(new ListenerData(EHPMTaskField.WorkflowStatus));
            return list;
        }

        /*
         * An object that implements this class can be assigned values.
         * This function will be called when an item is on the left hand side of an
         * assignment.
         */
        public void SetValue(Task task, ExpressionValue value)
        {
            //TODO: Fix workflow status setting
        }
    }
}