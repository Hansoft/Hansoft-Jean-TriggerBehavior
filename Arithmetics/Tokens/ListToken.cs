using Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Value;
using Hansoft.ObjectWrapper;
using HPMSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Tokens
{
    class ListToken : IExpressionItem
    {
        private List<IExpressionItem> value;

        /// <summary>
        /// Creates a list token that contains expression. The expressions needs to be of the same type.
        /// </summary>
        public ListToken()
        {
            this.value = new List<IExpressionItem>();
        }

        /// <summary>
        /// Adds a new expression to the list.
        /// </summary>
        /// <param name="expression">The expression to add to this token.</param>
        public void AddItem(IExpressionItem expression)
        {
            value.Add(expression);
        }

        /// <summary>
        /// Evaluates all expressions in the list token and returns an expression value of type list.
        /// Will throw an error if there are different type of items in the list.
        /// </summary>
        /// <param name="task">The task to evaluate this list on.</param>
        /// <returns>A list of containing the evaluated results of all the elements in the list</returns>
        public ExpressionValue Evaluate(Task task)
        {
            List<object> values = new List<object>();
            foreach (IExpressionItem expression in value)
            {
                ExpressionValue eValue = expression.Evaluate(task);
                values.Add(eValue);
            }
            return new ListExpressionValue(values);
        }

        /// <summary>
        /// Adds anything that items in the list is affected by to the incoming list
        /// </summary>
        /// <param name="list">The list of items that this expression is affected by</param>
        public void AddAffectedBy(ref List<ListenerData> list)
        {
            foreach(IExpressionItem expression in value)
            {
                expression.AddAffectedBy(ref list);
            }
        }

    }
}
