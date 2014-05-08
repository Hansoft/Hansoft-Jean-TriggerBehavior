using HPMSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hansoft.ObjectWrapper;
using Hansoft.ObjectWrapper.CustomColumnValues;
using System.Collections;
using Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Value;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Tokens
{
    /// <summary>
    /// Manages assignment and evaluation of the assignment column
    /// </summary>
    class AssigneesToken : IExpressionItem, IAssignable
    {
        /// <summary>
        /// Returns a string list in an expression value, where each string is a user name assigned to the task.
        /// </summary>
        /// <param name="task">the task to get assignees for</param>
        /// <returns>a string list in an expression value</returns>
        public ExpressionValue Evaluate(Task task)
        {
            List<String> userNames = new List<string>();
            foreach (User user in task.Assignees)
            {
                if(user.UniqueID.IsValid())
                    userNames.Add(user.Name);
            }
            return new ListExpressionValue(userNames);
        }
        
        /// <summary>
        /// Adds EHPMTaskField.ResourceAllocationFirst to the fields to listen to
        /// </summary>
        /// <param name="list">The list to add listeners to</param>
        public void AddAffectedBy(ref List<ListenerData> list)
        {
            list.AddRange(GetAssignmentFields());
        }
        
        /// <summary>
        /// Returns the field that this assignment will affect.
        /// </summary>
        /// <returns>the field that this assignment will affect</returns>
        public List<ListenerData> GetAssignmentFields()
        {
            List<ListenerData> list = new List<ListenerData>();
            list.Add(new ListenerData(EHPMTaskField.ResourceAllocationFirst));
            return list;
        }

        /*
         * An object that implements this class can be assigned values.
         * This function will be called when an item is on the left hand side of an
         * assignment.
         */
        public void SetValue(Task task, ExpressionValue value)
        {
            List<string> users = new List<string>();
            IList newAssignments = value.ToList();
            foreach (object obj in newAssignments)
            {
                string userName = obj.ToString();
                users.Add(userName);
            }
            task.SetResourceAssignmentsFromUserStrings(users);
        }

    }
}