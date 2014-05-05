using Hansoft.ObjectWrapper;
using HPMSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics
{
    class Condition
    {
        List<ListenerData> affectedBy;
        List<Assignment> assigments;
        private Expression expression;
        private string expressionStr; 

        public Condition(string str)
        {
            expressionStr = str;
            this.expression = Expression.parse(str);
            affectedBy = new List<ListenerData>();
            expression.AddAffectedBy(ref affectedBy);
            assigments = new List<Assignment>();
        }

        public void AddAssigment(Assignment assignment)
        {
            assigments.Add(assignment);
        }

        public string ExpressionStr
        {
            get { return expressionStr; }
        }

        public bool Evaluate(Task task)
        {
            ExpressionValue value = expression.Evaluate(task);
            if (value.Type != ExpressionValueType.BOOL)
                throw new ArgumentException("Condition exception that doesn't evaluate to bool: " + expressionStr);
            return value.ToBoolean();
        }

        public void ExcuteAssignments(Task task)
        {
            foreach (Assignment assignment in assigments)
                assignment.Execute(task);
        }

        /*
         * Returns the list of fields this expression is affected by.
         */
        public List<ListenerData> AffectedBy()
        {
            return affectedBy;
        }
    }
}
