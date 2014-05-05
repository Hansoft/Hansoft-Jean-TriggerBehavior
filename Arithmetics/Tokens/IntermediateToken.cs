using Hansoft.ObjectWrapper;
using HPMSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Tokens
{
    class IntermediateToken : IExpressionItem
    {
        private string token;

        public IntermediateToken(string token)
        {
            this.token = token.Trim();
        }

        public string Token
        {
            get { return token; }
        }

        /*
         * Intermediate token cannot be evaluated
         */
        public ExpressionValue Evaluate(Task task)
        {
            throw new ArgumentException("Evaluate should not be called on IntermediateToken");
        }

        public void AddAffectedBy(ref List<ListenerData> list)
        {
            throw new ArgumentException("AddAffectedBy should not be called on IntermediateToken");
        }
    }
}
