using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HPMSdk;
using Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Tokens;
using Hansoft.ObjectWrapper;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics
{
    class Assignment
    {
        IAssignable left;
        Expression right;

        private Assignment(IAssignable left, Expression right)
        {
            this.left = left;
            this.right = right;
        }

        public static Assignment parse(string expression)
        {
            int assignmentOpPos = expression.IndexOf("=");
            if (assignmentOpPos == -1)
                throw new ArgumentException("Malformed assignemnt. Expected a = : " + expression);

            IntermediateToken leftToken = new IntermediateToken(expression.Substring(0, assignmentOpPos));
            IAssignable left = Tokenizer.Compile(leftToken) as IAssignable;

            if(leftToken == null)
                throw new ArgumentException("Malformed assignemnt. Expected an IAssignable on the left hand side" + expression);

            return new Assignment(left, Expression.parse(expression.Substring(assignmentOpPos+1)));
        }
        public void Execute(Task task)
        {
            left.SetValue(task, right.Evaluate(task));
        }

    }
}
