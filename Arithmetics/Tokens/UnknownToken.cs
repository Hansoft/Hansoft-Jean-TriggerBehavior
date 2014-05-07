using Hansoft.ObjectWrapper;
using HPMSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Tokens
{
    /// <summary>
    ///  An holder class that only should be used during expression parsing
    /// </summary>
    class UnknownToken : IExpressionItem
    {
        /// <summary>
        /// Shouldn't be called
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public ExpressionValue Evaluate(Task task)
        {
            throw new ArgumentException("Evaluate should not be called on an UnknownToken");
        }

        /// <summary>
        /// Shouldn't be called.
        /// </summary>
        /// <param name="list"></param>
        public void AddAffectedBy(ref List<ListenerData> list)
        {
            throw new ArgumentException("AddAffectedBy should not be called on an UnknownToken");
        }
    }
}