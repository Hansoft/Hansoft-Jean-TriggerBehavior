using HPMSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hansoft.ObjectWrapper;
using Hansoft.ObjectWrapper.CustomColumnValues;

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
            return new ExpressionValue(ExpressionValueType.STRING, task.GetType().GetProperty(property).GetValue(task));
        }


        public void AddAffectedBy(ref List<ListenerData> list)
        {
            list.Add(new ListenerData(taskField));
        }

        /*
         * An object that implements this class can be assigned values.
         * This function will be called when an item is on the left hand side of an
         * assignment.
         */                      
        public void SetValue(Task task, ExpressionValue value)
        {
            task.GetType().GetProperty(property).SetValue(task, value.ToFloat());
        }
    }
}