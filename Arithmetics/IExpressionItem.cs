using Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Value;
using Hansoft.ObjectWrapper;
using HPMSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics
{
    interface IExpressionItem
    {
        ExpressionValue Evaluate(Task task);

        /*
         * Returns the list of fields this expression is affected by.
         */
        void AddAffectedBy(ref List<ListenerData> list);

    }
}
