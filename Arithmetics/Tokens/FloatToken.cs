﻿using Hansoft.ObjectWrapper;
using HPMSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Tokens
{
    class FloatToken : IExpressionItem
    {
        private float value;

        public FloatToken(float value)
        {
            this.value = value;
        }

        /*
         * Returns the value of this token in the incoming task. 
         */
        public ExpressionValue Evaluate(Task task)
        {
            return new ExpressionValue(ExpressionValueType.FLOAT, value);
        }

        public void AddAffectedBy(ref List<ListenerData> list)
        {
        }
    }
}
