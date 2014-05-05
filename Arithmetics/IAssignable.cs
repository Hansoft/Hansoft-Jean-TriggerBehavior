using Hansoft.ObjectWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics
{
    interface IAssignable
    {
        /*
         * An object that implements this class can be assigned values.
         * This function will be called when an item is on the left hand side of an
         * assignment.
         */
        void SetValue(Task task, ExpressionValue value);
    }
}
