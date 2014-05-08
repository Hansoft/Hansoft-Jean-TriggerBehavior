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
    class DefaultFloatColumnToken : IExpressionItem, IAssignable
    {
        private EHPMTaskField taskField;
        private EHPMProjectDefaultColumn column;
        private string property;


        public DefaultFloatColumnToken(EHPMTaskField taskField, EHPMProjectDefaultColumn column, string property)
        {
            this.taskField = taskField;
            this.column = column;
            this.property = property;
        }


        /*
         * Returns the value of this token in the incoming task. 
         */
        public ExpressionValue Evaluate(Task task)
        {
            return new DoubleExpressionValue((double)GetType().GetProperty(property).GetValue(task));
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
            list.Add(new ListenerData(taskField));
            return list;
        } 
        
       
        /// <summary>
        /// Sets the value of a default column of type float to the expression value.
        /// Since the expression value will be converted to float it might generate
        /// an exception if the expression value cannot be converted.
        /// </summary>
        /// <param name="task">the task to set the column value for</param>
        /// <param name="value">the value to set</param>
        public void SetValue(Task task, ExpressionValue value)
        {
            task.GetType().GetProperty(property).SetValue(task, value.ToDouble());
        }
    }
}