using HPMSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hansoft.ObjectWrapper;
using Hansoft.ObjectWrapper.CustomColumnValues;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Tokens
{
    class ComplexityPointsToken : IExpressionItem, IAssignable
    {
        /*
         * Returns the value of this token in the incoming task. 
         */
        public ExpressionValue Evaluate(Task task)
        {
            return new ExpressionValue(ExpressionValueType.INT, task.Points);
        }
        
        
        public void AddAffectedBy(ref List<ListenerData> list)
        {
            list.Add(new ListenerData(EHPMTaskField.ComplexityPoints));
        }

        /*
         * An object that implements this class can be assigned values.
         * This function will be called when an item is on the left hand side of an
         * assignment.
         */
        public void SetValue(Task task, ExpressionValue value)
        {
            task.Points = value.ToInt();
        }

    }
}