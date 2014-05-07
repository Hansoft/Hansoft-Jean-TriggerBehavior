using Hansoft.ObjectWrapper.CustomColumnValues;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics
{
    public enum ExpressionValueType
    {
        UNKNOWN,
        INT,
        FLOAT,
        BOOL,
        STRING,
        LIST, // Remember that a expression value of type list will contain expression values once it is evaluated.
        CUSTOMVALUE
    }

    class ExpressionValue
    {
        private ExpressionValueType type;
        private object value;

        public ExpressionValue(ExpressionValueType type, Object value)
        {
            this.value = value;
            this.type = type;
        }

        public ExpressionValueType Type
        {
            get { return type; }
            set { type = value; }
        }

        public object Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        
        public static ExpressionValue operator +(ExpressionValue left, ExpressionValue right)
        {
            // Always convert to string if one of the sides is a string expression
            if (left.Type == ExpressionValueType.STRING || right.Type == ExpressionValueType.STRING)
                return new ExpressionValue(ExpressionValueType.STRING, left.ToString() + right.ToString());
            else if (left.Type == ExpressionValueType.FLOAT)
                return new ExpressionValue(ExpressionValueType.FLOAT, left.ToFloat() + right.ToFloat());
            else if (left.Type == ExpressionValueType.INT)
            {
                if (left.Type == ExpressionValueType.FLOAT)
                    return new ExpressionValue(ExpressionValueType.FLOAT, left.ToFloat() + right.ToFloat());
                else
                    return new ExpressionValue(ExpressionValueType.INT, left.ToInt() + right.ToInt());
            }
            else if (left.Type == ExpressionValueType.LIST)
            {
                ((IList)left.Value).Add(right.Value);
            }
            return new ExpressionValue(ExpressionValueType.STRING, left.ToString() + right.ToString());
        }

        public static ExpressionValue operator -(ExpressionValue left, ExpressionValue right)
        {
            if (left.Type == ExpressionValueType.FLOAT || right.Type == ExpressionValueType.FLOAT)
                return new ExpressionValue(ExpressionValueType.FLOAT, left.ToFloat() * right.ToFloat());
            else if (left.Type == ExpressionValueType.LIST)
            {
                ((IList)left.Value).Add(right);
            }
            return new ExpressionValue(ExpressionValueType.INT, left.ToInt() * right.ToInt());
        }

        public static ExpressionValue operator*(ExpressionValue left, ExpressionValue right)
        {
            if (left.Type == ExpressionValueType.FLOAT || right.Type == ExpressionValueType.FLOAT)
                return new ExpressionValue(ExpressionValueType.FLOAT, left.ToFloat() + right.ToFloat());
            return new ExpressionValue(ExpressionValueType.INT, left.ToInt() * right.ToInt());
        }

        public static ExpressionValue operator /(ExpressionValue left, ExpressionValue right)
        {
            return new ExpressionValue(ExpressionValueType.FLOAT, left.ToFloat() / right.ToFloat());
        }

        public static ExpressionValue operator <(ExpressionValue left, ExpressionValue right)
        {
            if (left.Type == ExpressionValueType.FLOAT)
                return new ExpressionValue(ExpressionValueType.BOOL, left.ToFloat() < right.ToFloat());
            else
                return new ExpressionValue(ExpressionValueType.BOOL, left.ToInt() < right.ToInt());
        }
        
        public static ExpressionValue operator <=(ExpressionValue left, ExpressionValue right)
        {
            if (left.Type == ExpressionValueType.FLOAT)
                return new ExpressionValue(ExpressionValueType.BOOL, left.ToFloat() <= right.ToFloat());
            else
                return new ExpressionValue(ExpressionValueType.BOOL, left.ToInt() <= right.ToInt());
        }

        public static ExpressionValue operator >=(ExpressionValue left, ExpressionValue right)
        {
            if (left.Type == ExpressionValueType.FLOAT)
                return new ExpressionValue(ExpressionValueType.BOOL, left.ToFloat() >= right.ToFloat());
            else
                return new ExpressionValue(ExpressionValueType.BOOL, left.ToInt() >= right.ToInt());
        }

        public static ExpressionValue operator >(ExpressionValue left, ExpressionValue right)
        {
            if (left.Type == ExpressionValueType.FLOAT)
                return new ExpressionValue(ExpressionValueType.BOOL, left.ToFloat() > right.ToFloat());
            else
                return new ExpressionValue(ExpressionValueType.BOOL, left.ToInt() > right.ToInt());
        }

        public static ExpressionValue operator ==(ExpressionValue left, ExpressionValue right)
        {
            if (left.Type == ExpressionValueType.CUSTOMVALUE || left.Type == ExpressionValueType.STRING)
                return new ExpressionValue(ExpressionValueType.BOOL, left.ToString().Equals(right.ToString()));
            else if (left.Type == ExpressionValueType.FLOAT)
                return new ExpressionValue(ExpressionValueType.BOOL, left.ToFloat() == right.ToFloat());
            else
                return new ExpressionValue(ExpressionValueType.BOOL, left.ToInt() == right.ToInt());
        }

        public static ExpressionValue operator !=(ExpressionValue left, ExpressionValue right)
        {
            if (left.Type == ExpressionValueType.CUSTOMVALUE || left.Type == ExpressionValueType.STRING)
                return new ExpressionValue(ExpressionValueType.BOOL, !left.ToString().Equals(right.ToString()));
            else if (left.Type == ExpressionValueType.FLOAT)
                return new ExpressionValue(ExpressionValueType.BOOL, left.ToFloat() != right.ToFloat());
            else
                return new ExpressionValue(ExpressionValueType.BOOL, left.ToInt() != right.ToInt());
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public IList ToStringList()
        {
            List<String> values = new List<String>();
            if (Type == ExpressionValueType.LIST)
            {
                IList eList = Value as IList;
                foreach (object obj in eList)
                {
                    if (!(obj is ExpressionValue))
                    {
                        throw new ArgumentException("Something went bad in a list. Should be filled with expression values.");
                    }
                    string str = (obj as ExpressionValue).Value as string;
                    if (str == null)
                    {
                        throw new ArgumentException("Cannot convert to a string list unless all expression values are strings");
                    }
                    values.Add(str);
                }
            }
            else
            {
                values.Add(ToString());
            }
            return values;
        }

        public bool ToBoolean()
        {
            switch (type)
            {
                case (ExpressionValueType.UNKNOWN):
                    throw new ArgumentException("Can't cast unkowns to booleans.");
                case (ExpressionValueType.LIST):
                    throw new ArgumentException("Can't cast lists to booleans.");
                case (ExpressionValueType.BOOL):
                    {
                        return Convert.ToBoolean(value);
                    }
                case (ExpressionValueType.STRING):
                    {
                        bool state;
                        bool isBool = bool.TryParse(value.ToString(), out state);
                        if (isBool)
                            return state;
                        throw new ArgumentException("Can't cast " + value + " to bool");
                    }
                case (ExpressionValueType.INT):
                case (ExpressionValueType.FLOAT):
                    {
                        return ToInt() != 0;
                    }
                case (ExpressionValueType.CUSTOMVALUE):
                    {
                        CustomColumnValue columnValue = value as CustomColumnValue;
                        if(value == null)
                            throw new ArgumentException("Can't cast " + value + " to bool");
                        return columnValue.ToInt()!=0?true:false;
                    }
            }
            return false;
        }

        public int ToInt()
        {
            switch (type)
            {
                case (ExpressionValueType.LIST):
                    throw new ArgumentException("Can't cast lists to int.");
                case (ExpressionValueType.UNKNOWN):
                    throw new ArgumentException("Can't cast unkowns to int.");
                case (ExpressionValueType.BOOL):
                    {
                        if (ToBoolean())
                            return 1;
                        return 0;
                    }                       
                case (ExpressionValueType.STRING):
                    {
                        int number;
                        bool isInt = int.TryParse(value.ToString(), out number);
                        if (isInt)
                            return number;
                        throw new ArgumentException("Can't cast " + value + " to int");
                    }
                case (ExpressionValueType.INT):
                case (ExpressionValueType.FLOAT):
                    return Convert.ToInt32(value);
                case (ExpressionValueType.CUSTOMVALUE):
                    {
                        CustomColumnValue columnValue = value as CustomColumnValue;
                        if (value == null)
                            throw new ArgumentException("Can't cast " + value + " to int");
                        return (int)columnValue.ToInt();
                    }
            }
            return 0;
        }

        public float ToFloat()
        {
            switch (type)
            {
                case (ExpressionValueType.LIST):
                    throw new ArgumentException("Can't cast lists to float.");
                case (ExpressionValueType.UNKNOWN):
                    throw new ArgumentException("Can't cast unkowns to float.");
                case (ExpressionValueType.BOOL):
                    {
                        if (ToBoolean())
                            return 1f;
                        return 0f;
                    }
                case (ExpressionValueType.STRING):
                    {
                        float number;
                        bool isFloat = float.TryParse(value.ToString(), out number);
                        if (isFloat)
                            return number;
                        throw new ArgumentException("Can't cast " + value + " to float");
                    }
                case (ExpressionValueType.INT):
                case (ExpressionValueType.FLOAT):
                    return (float)Convert.ToDecimal(value);
                case (ExpressionValueType.CUSTOMVALUE):
                    {
                        CustomColumnValue columnValue = value as CustomColumnValue;
                        if (value == null)
                            throw new ArgumentException("Can't cast " + value + " to float");
                        return (float)columnValue.ToDouble();
                    }
            }
            return 0;
        }

    }
}
