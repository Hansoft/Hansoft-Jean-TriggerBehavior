using HPMSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics
{
    class ListenerData : IEquatable<ListenerData>
    {
        private EHPMTaskField taskField;
        private string customColumnName;

        public ListenerData(EHPMTaskField taskField)
        {
            this.taskField = taskField;
            this.customColumnName = "";
        }

        public ListenerData(EHPMTaskField taskField, string customColumnName)
        {
            this.taskField = taskField;
            this.customColumnName = customColumnName;
        }

        public EHPMTaskField TaskField
        {
            get { return taskField; }
        }
        public string CustomColumnName
        {
            get { return customColumnName; }
        }

        public bool Equals(ListenerData other)
        {
            return this.taskField == other.taskField &&
                   this.customColumnName == other.customColumnName;
        }
    
    }
}
