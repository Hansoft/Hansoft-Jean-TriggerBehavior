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

        public Condition()
        {
            assigments = new List<Assignment>();
        }

        /// <summary>
        /// Parses the string and creates a conditional expression.
        /// </summary>
        /// <param name="expressionStr"> The string to create a condition for</param>
        /// <param name="errors"> The list of errors that came up during parsing</param>
        public void parse(string expressionStr, ref List<string> errors)
        {
            this.expressionStr = expressionStr;
            this.expression = Expression.parse(expressionStr, ref errors);
            if (expression == null)
            {
                errors.Add("Failed to parse the expression: " + expressionStr);
                return;
            }
            affectedBy = new List<ListenerData>();
            expression.AddAffectedBy(ref affectedBy);
        }

        public void AddAssigment(Assignment assignment)
        {
            assigments.Add(assignment);
        }

        /// <summary>
        /// Looks through the expression and it's statements to find potential infinite loops.
        /// If a loop is found it will return true and add an error to the list.
        /// </summary>
        /// <param name="errors">errors will be added to the list</param>
        /// <returns></returns>
        public bool FindInfiniteLoops(ref List<string> errors)
        {
            bool foundLoop = false;
            List<ListenerData> assignmentFields = new List<ListenerData>();
            foreach (Assignment assignment in assigments)
                assignmentFields.AddRange(assignment.GetAssignmentFields());

            foreach(ListenerData data in affectedBy)
            {
                if(assignmentFields.Contains(data))
                {
                    foundLoop = true;
                    errors.Add("Found a possible infinite loop in condition (" + expressionStr + "). Where the field: (" + data.TaskField + ", " + data.CustomColumnName + "). Exists in both condition and assignement.");
                }
            }
            return foundLoop;
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
