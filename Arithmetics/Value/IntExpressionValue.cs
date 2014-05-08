using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Value
{
    class IntExpressionValue: ExpressionValue
    {
        private int value; 

        public IntExpressionValue(int value)
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
            if (other is DoubleExpressionValue)
                return new DoubleExpressionValue((double)value+other.ToDouble()) ;
            if (other is StringExpressionValue)
                return new StringExpressionValue(ToString() + other.ToString());
            return new IntExpressionValue(value + other.ToInt());
        }


        /// <summary>
        /// Subtraction function for expression values to override to handle arithmetic operations
        /// </summary>
        /// <param name="other">the value to subtract to this value</param>
        /// <returns>an expression value</returns>
        protected override ExpressionValue Subtract(ExpressionValue other)
        {
            if (other is DoubleExpressionValue)
                return new DoubleExpressionValue((double)value - other.ToDouble());
            return new IntExpressionValue(value - other.ToInt());
        }

        /// <summary>
        /// Multiplication function for expression values to override to handle arithmetic operations
        /// </summary>
        /// <param name="other">the value to multiply this value by</param>
        /// <returns>an expression value</returns>
        protected override ExpressionValue Multiply(ExpressionValue other)
        {
            if (other is DoubleExpressionValue)
                return new DoubleExpressionValue((double)value * other.ToDouble());
            return new IntExpressionValue(value * other.ToInt());
        }

        /// <summary>
        /// Division function for expression values to override to handle arithmetic operations
        /// </summary>
        /// <param name="other">the value to  divide this value by</param>
        /// <returns>an expression value</returns>
        protected override ExpressionValue DivideBy(ExpressionValue other)
        {
            return new DoubleExpressionValue((double)value / other.ToDouble());
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
            if (val is DoubleExpressionValue)
            {
                return (int)Math.Round((double)value - val.ToDouble(), MidpointRounding.AwayFromZero);
            }
            return value - val.ToInt();
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
            if (val is DoubleExpressionValue)
            {
                return (double)value == val.ToDouble();
            }
            return value == val.ToInt();
        }

        /// <summary>
        /// Convert (if needed) the underlying value to an integer value.
        /// </summary>
        /// <returns>The integer value.</returns>
        public override int ToInt()
        {
            return value;
        }

        /// <summary>
        /// Convert (if needed) the underlying value to a double value.
        /// </summary>
        /// <returns>The double value</returns>
        public override double ToDouble()
        {
            return (double)value;
        }

        /// <summary>
        /// Convert (if needed) the underlying value to a string value.
        /// </summary>
        /// <returns>The double value</returns>
        public override string ToString()
        {
            return value.ToString();
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
            list.Add(value.ToString());
            return list;
        }
        /// <summary>
        /// IConvertible override
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public override DateTime ToDateTime(IFormatProvider provider)
        {
            return new DateTime(value);
        }

        /// <summary>
        /// Converts a value to a boolean.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public override bool ToBoolean()
        {
            return value != 0;
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
