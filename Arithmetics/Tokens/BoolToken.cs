﻿using Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Value;
using Hansoft.ObjectWrapper;
using HPMSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Tokens
{
    class BoolToken : IExpressionItem
    {
        private bool value;

        public BoolToken(bool value)
        {
            this.value = value;
        }

        /*
         * Returns the value of this token in the incoming task. 
         */
        public ExpressionValue Evaluate(Task task)
        {
            return new BoolExpressionValue(value);
        }

        public void AddAffectedBy(ref List<ListenerData> list)
        {
        }
    }
}
