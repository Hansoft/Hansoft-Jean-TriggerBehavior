using Hansoft.ObjectWrapper;
using Hansoft.ObjectWrapper.CustomColumnValues;
using HPMSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Tokens
{
    class TimeNowToken : IExpressionItem
    {

        /*
         * Returns the value of this token in the incoming task. 
         */
        public ExpressionValue Evaluate(Task task)
        {
            return new ExpressionValue(ExpressionValueType.CUSTOMVALUE, DateTimeValue.FromHpmDateTime(task, null, HPMUtilities.HPMNow()));
        }

        public void AddAffectedBy(ref List<ListenerData> list)
        {
        }

    }
}
