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
        string expression;

        /// <summary>
        /// Parses the string and creates a assignment expression. If it doesn't contain an = an error will be thrown.
        /// </summary>
        /// <param name="expressionStr"> The string to create a assignment for</param>
        /// <param name="errors"> The list of errors that came up during parsing</param>
        public void parse(string expressionStr, ref List<string> errors)
        {
            this.expression = expressionStr.Trim();
            int assignmentOpPos = expression.IndexOf("=");
            if (assignmentOpPos == -1)
                throw new ArgumentException("Malformed assignemnt. Expected a = : " + expression);

            left = Tokenizer.Compile(expression.Substring(0, assignmentOpPos), ref errors) as IAssignable;

            if (left == null)
                errors.Add("Malformed assignemnt. Expected an IAssignable on the left hand side" + expression);

            right =  Expression.parse(expression.Substring(assignmentOpPos+1), ref errors);
        }

        /// <summary>
        /// Executes the assignement.
        /// </summary>
        /// <param name="task">The task to execute the assignment on</param>
        public void Execute(Task task)
        {
            left.SetValue(task, right.Evaluate(task));
        }


        /// <summary>
        /// Returns the list of fields the left hand side of the assignment is affected by. This
        /// can be used to prevent loops between condition and assignments.
        /// </summary>
        /// <returns>the list of fields the left hand side of the assignment is affected b</returns>
        public List<ListenerData> GetAssignmentFields()
        {
            return left.GetAssignmentFields();
        }

    }
}
