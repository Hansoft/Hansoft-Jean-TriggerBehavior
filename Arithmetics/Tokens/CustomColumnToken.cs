using HPMSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hansoft.ObjectWrapper;
using Hansoft.ObjectWrapper.CustomColumnValues;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Tokens
{
    class CustomColumnToken : IExpressionItem, IAssignable
    {
        private string name;

        public CustomColumnToken(string name)
        {
            this.name = name;
        }

        /*
         * Returns the value of this token in the incoming task. 
         */
        public ExpressionValue Evaluate(Task task)
        {
            HPMProjectCustomColumnsColumn customColumn = task.ProjectView.GetCustomColumn(name);
            if (customColumn == null)
                throw new ArgumentException("No such custom column: " + name);
            CustomColumnValue value = task.GetCustomColumnValue(customColumn);
            switch (customColumn.m_Type)
            {
                case(EHPMProjectCustomColumnsColumnType.DateTime):
                case(EHPMProjectCustomColumnsColumnType.DateTimeWithTime):
                case(EHPMProjectCustomColumnsColumnType.DropList):
                case (EHPMProjectCustomColumnsColumnType.Hyperlink):
                case (EHPMProjectCustomColumnsColumnType.MultiSelectionDropList):
                case (EHPMProjectCustomColumnsColumnType.Resources):
                    {
                        return new ExpressionValue(ExpressionValueType.CUSTOMVALUE, value);
                    }
                case (EHPMProjectCustomColumnsColumnType.MultiLineText):
                case (EHPMProjectCustomColumnsColumnType.Text):
                    {
                        return new ExpressionValue(ExpressionValueType.STRING, value.ToString());
                    }
                case (EHPMProjectCustomColumnsColumnType.FloatNumber):
                case (EHPMProjectCustomColumnsColumnType.AccumulatedTime):
                    {
                        return new ExpressionValue(ExpressionValueType.FLOAT, (float)value.ToDouble());
                    }
                case(EHPMProjectCustomColumnsColumnType.IntegerNumber):
                    {
                        return new ExpressionValue(ExpressionValueType.INT, value.ToInt());

                    }
                case(EHPMProjectCustomColumnsColumnType.NewVersionOfSDKRequired):
                    {
                        throw new ArgumentException("Cannot get the value for custom column: " + name);
                    }
            }
            return null;
        }


        public void AddAffectedBy(ref List<ListenerData> list)
        {
            list.Add(new ListenerData(EHPMTaskField.CustomColumnData, name));
        }

        /*
         * An object that implements this class can be assigned values.
         * This function will be called when an item is on the left hand side of an
         * assignment.
         */
        public void SetValue(Task task, ExpressionValue value)
        {
            HPMProjectCustomColumnsColumn customColumn = task.ProjectView.GetCustomColumn(name);
            if (customColumn == null)
                throw new ArgumentException("No such custom column: " + name);
            switch (customColumn.m_Type)
            {
                case (EHPMProjectCustomColumnsColumnType.DateTime):
                case (EHPMProjectCustomColumnsColumnType.DateTimeWithTime):
                case (EHPMProjectCustomColumnsColumnType.DropList):
                case (EHPMProjectCustomColumnsColumnType.Hyperlink):
                case (EHPMProjectCustomColumnsColumnType.MultiSelectionDropList):
                case (EHPMProjectCustomColumnsColumnType.Resources):
                    {
                        task.SetCustomColumnValue(name, value.Value);
                        break;
                    }
                case (EHPMProjectCustomColumnsColumnType.MultiLineText):
                case (EHPMProjectCustomColumnsColumnType.Text):
                    {
                        task.SetCustomColumnValue(name, value.ToString());
                        break;
                    }
                case (EHPMProjectCustomColumnsColumnType.FloatNumber):
                case (EHPMProjectCustomColumnsColumnType.AccumulatedTime):
                    {
                        task.SetCustomColumnValue(name, value.ToFloat());
                        break;
                    }
                case (EHPMProjectCustomColumnsColumnType.IntegerNumber):
                    {
                        task.SetCustomColumnValue(name, value.ToInt());
                        break;
                    }
                case (EHPMProjectCustomColumnsColumnType.NewVersionOfSDKRequired):
                    {
                        throw new ArgumentException("Cannot get the value for custom column: " + name);
                    }
            }
        }

    }
}
