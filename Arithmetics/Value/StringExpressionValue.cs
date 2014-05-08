using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Value
{
    class StringExpressionValue : ExpressionValue
    {
        private string value;

        public StringExpressionValue(string value)
        {
            this.value = value;
        }

        /// <summary>
        /// Addition function for expression values to override to handle arithmetic operations
        /// </summary>
        /// <param name="other">the value to add to this value</param>
        /// <returns>an expression value</returns>
        protected override ExpressionValue Add(ExpressionValue other)
        {
            return new StringExpressionValue(value + other.ToString());
        }


        /// <summary>
        /// Subtraction function for expression values to override to handle arithmetic operations
        /// </summary>
        /// <param name="other">the value to subtract to this value</param>
        /// <returns>an expression value</returns>
        protected override ExpressionValue Subtract(ExpressionValue other)
        {
            // Try to convert the string to a double. Will cast an expression if it doesn't work.
            return new DoubleExpressionValue(ToDouble() - other.ToDouble());
        }

        /// <summary>
        /// Multiplication function for expression values to override to handle arithmetic operations
        /// </summary>
        /// <param name="other">the value to multiply this value by</param>
        /// <returns>an expression value</returns>
        protected override ExpressionValue Multiply(ExpressionValue other)
        {
            // Try to convert the string to a double. Will cast an expression if it doesn't work.
            return new DoubleExpressionValue(ToDouble() * other.ToDouble());
        }

        /// <summary>
        /// Division function for expression values to override to handle arithmetic operations
        /// </summary>
        /// <param name="other">the value to  divide this value by</param>
        /// <returns>an expression value</returns>
        protected override ExpressionValue DivideBy(ExpressionValue other)
        {
            // Try to convert the string to a double. Will cast an expression if it doesn't work.
            return new DoubleExpressionValue(ToDouble() / other.ToDouble());
        }


        /// <summary>
        /// Implementation of IComparable
        /// </summary>
        /// <param name="obj">The other object to compare with.</param>
        /// <returns>The result of the comparison</returns>
        public override int CompareTo(object obj)
        {
            ExpressionValue val = obj as ExpressionValue;
            if(val == null)
                throw new ArgumentException("Cannot compare an ExpressionValue to other types of objects.");
            // Try to convert the string to a double. Will cast an expression if it doesn't work.
            return (int)Math.Round(ToDouble() - val.ToDouble(), MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Implementation of IComparable
        /// </summary>
        /// <param name="obj">The other object to compare with.</param>
        /// <returns>The result of the comparison</returns>
        public override bool Equals(object obj)
        {
            ExpressionValue val = obj as ExpressionValue;
            if(val == null)
                throw new ArgumentException("Cannot compare an ExpressionValue to other types of objects.");
            return value == val.ToString();
        }

        /// <summary>
        /// Convert (if needed) the underlying value to an integer value.
        /// </summary>
        /// <returns>The integer value.</returns>
        public override int ToInt()
        {
            int iVal = 0;
            if(!int.TryParse(value, out iVal))
                throw new ArgumentException("Cannot convert the string: '"+value+"' to a number");
            return iVal;
        }

        /// <summary>
        /// Convert (if needed) the underlying value to a double value.
        /// </summary>
        /// <returns>The double value</returns>
        public override double ToDouble()
        {
            double dVal = 0;
            if (!double.TryParse(value, out dVal))
                throw new ArgumentException("Cannot convert the string: '" + value + "' to a number");
            return dVal;
        }

        /// <summary>
        /// Convert (if needed) the underlying value to a string value.
        /// </summary>
        /// <returns>The double value</returns>
        public override string ToString()
        {
            return value;
        }

        /// <summary>
        /// Convert (if needed) the underlying value to a list value.
        /// </summary>
        /// <returns>The list</returns>
        public override IList ToList()
        {
            IList list = new List<object>();
            list.Add(value);
            return list;
        }

        /// <summary>
        /// Convert (if needed) the underlying value to a list of strings.
        /// </summary>
        /// <returns>The list</returns>
        public override List<string> ToStringList()
        {
            List<string> list = new List<string>();
            list.Add(value);
            return list;
        }

        /// <summary>
        /// IConvertible override
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public override DateTime ToDateTime(IFormatProvider provider)
        {
            return DateTime.Parse(value, provider);
        }

        /// <summary>
        /// Converts a value to a boolean.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public override bool ToBoolean()
        {
            bool bVal;
            if (!bool.TryParse(value, out bVal))
                throw new InvalidOperationException("Cannot cast '" + value + "' to a boolean.");
            return bVal;
        }

        /// <summary>
        /// IConvertible override, will throw NotImplementedException if called.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
