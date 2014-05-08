using Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Value;
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
        private Expression conditional;
        private string expression; 

        public Condition()
        {
            assigments = new List<Assignment>();
        }

        /// <summary>
        /// Parses the string and creates a conditional expression.
        /// </summary>
        /// <param name="expressionStr"> The string to create a condition for</param>
        /// <param name="errors"> The list of errors that came up during parsing</param>
        public void parse(string expression, ref List<string> errors)
        {
            this.expression = expression;
            this.conditional = Expression.parse(expression, ref errors);
            if (conditional == null)
            {
                errors.Add("Failed to parse the expression: " + ConditionalExpression);
                return;
            }
            affectedBy = new List<ListenerData>();
            conditional.AddAffectedBy(ref affectedBy);
        }

        /// <summary>
        /// Adds a new assignment operation to this condition.
        /// </summary>
        /// <param name="assignment">the assignment to add.</param>
        public void AddAssigment(Assignment assignment)
        {
            assigments.Add(assignment);
        }

        /// <summary>
        /// Looks through the expression and its statements to find potential infinite loops.
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
                    errors.Add("Found a possible infinite loop in condition (" + expression + "). Where the field: (" + data.TaskField + ", " + data.CustomColumnName + "). Exists in both condition and assignement.");
                }
            }
            return foundLoop;
        }

        /// <summary>
        /// Getter for the conditional expression string
        /// </summary>
        public string ConditionalExpression
        {
            get { return expression; }
        }

        /// <summary>
        /// Will evaluate this condition on the incoming task to see if the condition is triggered.
        /// </summary>
        /// <param name="task">The task to match the condition to</param>
        /// <returns></returns>
        public bool Evaluate(Task task)
        {
            ExpressionValue value;
            try 
            { 
                value = conditional.Evaluate(task);
            }
            catch(Exception e)
            {
                throw new Exception("Error evaluating expression: '" + expression + "'\n" + e.Message);
            }

            if (!(value is BoolExpressionValue))
                throw new ArgumentException("Condition exception that doesn't evaluate to bool: " + expression);
            return value.ToInt() != 0;
        }

        /// <summary>
        /// Executes all assignments in this condition.
        /// </summary>
        /// <param name="task">the task to execute the assignments on.</param>
        public void ExcuteAssignments(Task task)
        {
            foreach (Assignment assignment in assigments)
            {
                try
                {
                    assignment.Execute(task);
                }
                catch (Exception e)
                {
                    throw new Exception("Error executing assignment : '" + assignment.AssignmentExpression + "'\n" + e.Message);
                }
            }
        }

        /// <summary>
        /// Returns the list of fields that this condition is affected by.
        /// </summary>
        /// <returns>the list of fields that this condition is affected by</returns>
        public List<ListenerData> AffectedBy()
        {
            return affectedBy;
        }
    }
}
