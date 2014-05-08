using Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Value;
using Hansoft.ObjectWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics
{
    interface IAssignable
    {
        /// <summary>
        /// Sets the value of the assignable field to the incoming expression value on the task.
        /// </summary>
        /// <param name="task">the task to modify</param>
        /// <param name="value">the new value</param>
        void SetValue(Task task, ExpressionValue value);

        /// <summary>
        /// Returns the fields that this assignment will affect.
        /// </summary>
        /// <returns>the field that this assignment will affect</returns>
        List<ListenerData> GetAssignmentFields();
    }
}
