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
    class CustomColumnToken : IExpressionItem, IAssignable
    {
        private string name;

        public CustomColumnToken(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Returns true if the prefered operation type is int
        /// </summary>
        private static bool PreferedTypeInt(EHPMProjectCustomColumnsColumnType type)
        {
            return type == EHPMProjectCustomColumnsColumnType.IntegerNumber;
        }

        /// <summary>
        /// Returns true if the prefered operation type is double
        /// </summary>
        private static bool PreferedTypeDouble(EHPMProjectCustomColumnsColumnType type)
        {
            return type == EHPMProjectCustomColumnsColumnType.FloatNumber || 
                type == EHPMProjectCustomColumnsColumnType.AccumulatedTime;
        }

        /// <summary>
        /// Returns true if the prefered operation type is string
        /// </summary>
        private static bool PreferedTypeString(EHPMProjectCustomColumnsColumnType type)
        {
            return type == EHPMProjectCustomColumnsColumnType.DropList || 
                type == EHPMProjectCustomColumnsColumnType.MultiLineText ||
                type == EHPMProjectCustomColumnsColumnType.Text ||
                type == EHPMProjectCustomColumnsColumnType.Hyperlink;
        }

        /// <summary>
        /// Returns true if the prefered operation type is string list
        /// </summary>
        private static bool PreferedTypeStringList(EHPMProjectCustomColumnsColumnType type)
        {
            return type == EHPMProjectCustomColumnsColumnType.MultiSelectionDropList ||
                type == EHPMProjectCustomColumnsColumnType.Resources;
        }

        /// <summary>
        /// Returns true if the prefered operation type is string list
        /// </summary>
        private static bool PreferedTypeDate(EHPMProjectCustomColumnsColumnType type)
        {
            return type == EHPMProjectCustomColumnsColumnType.DateTime || 
                type == EHPMProjectCustomColumnsColumnType.DateTimeWithTime; 
        }

        /// <summary>
        /// Evaluates the value in the custom column and returns an expression value of the prefered type.
        /// </summary>
        /// <param name="task">task to get the value from</param>
        /// <returns>an expression value of the prefered type</returns>
        public ExpressionValue Evaluate(Task task)
        {
            HPMProjectCustomColumnsColumn customColumn = task.ProjectView.GetCustomColumn(name);
            if (customColumn == null)
                throw new ArgumentException("No such custom column: " + name);
            CustomColumnValue value = task.GetCustomColumnValue(customColumn);
            if (customColumn.m_Type == EHPMProjectCustomColumnsColumnType.NewVersionOfSDKRequired)
            {
                throw new ArgumentException("Cannot get the value for custom column: " + name +". You have to update the SDK to handle this column type.");
            }
            if (PreferedTypeDate(customColumn.m_Type))
                return new DateExpressionValue(value.ToDateTime(null));
            else if (PreferedTypeInt(customColumn.m_Type))
                return new IntExpressionValue((int)value.ToInt());
            else if (PreferedTypeDouble(customColumn.m_Type))
                return new DoubleExpressionValue(value.ToDouble());
            else if (PreferedTypeString(customColumn.m_Type))
                return new StringExpressionValue(value.ToString());
            else if (PreferedTypeStringList(customColumn.m_Type))
                return new ListExpressionValue(value.ToStringList());
            return null;
        }


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
            list.Add(new ListenerData(EHPMTaskField.CustomColumnData, name));
            return list;
        }

        /// <summary>
        /// Will assign the appropriate value to the custom column
        /// </summary>
        /// <param name="task">the task to assign the column value to</param>
        /// <param name="value">the new value to assign</param>
        public void SetValue(Task task, ExpressionValue value)
        {
            HPMProjectCustomColumnsColumn customColumn = task.ProjectView.GetCustomColumn(name);
            if (customColumn == null)
                throw new ArgumentException("No such custom column: " + name);
            if (customColumn.m_Type == EHPMProjectCustomColumnsColumnType.NewVersionOfSDKRequired)
            {
                throw new ArgumentException("Cannot set the value for custom column: " + name +". You have to update the SDK to handle this column type.");
            }
            if (PreferedTypeDate(customColumn.m_Type))
                task.SetCustomColumnValue(name, CustomColumnValue.FromEndUserString(task, customColumn, value.ToString())); 
            else if (PreferedTypeInt(customColumn.m_Type))
                task.SetCustomColumnValue(name, value.ToInt());
            else if (PreferedTypeDouble(customColumn.m_Type))
                task.SetCustomColumnValue(name, value.ToDouble());
            else if (PreferedTypeString(customColumn.m_Type))
                task.SetCustomColumnValue(name, CustomColumnValue.FromEndUserString(task, customColumn, value.ToString()));
            else if (PreferedTypeStringList(customColumn.m_Type))
                task.SetCustomColumnValue(name, CustomColumnValue.FromStringList(task, customColumn, value.ToStringList()));
        }

    }
}
