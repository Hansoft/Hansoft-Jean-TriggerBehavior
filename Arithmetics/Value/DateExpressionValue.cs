using Hansoft.ObjectWrapper.CustomColumnValues;
using HPMSdk;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Value
{

    class DateExpressionValue: ExpressionValue
    {
        private DateTime value;

        public DateExpressionValue(DateTime value)
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
            if (other is StringExpressionValue)
                return new StringExpressionValue(ToString() + other.ToString());
            return new DateExpressionValue(value.AddTicks(other.ToInt()));
        }


        /// <summary>
        /// Subtraction function for expression values to override to handle arithmetic operations
        /// </summary>
        /// <param name="other">the value to subtract to this value</param>
        /// <returns>an expression value</returns>
        protected override ExpressionValue Subtract(ExpressionValue other)
        {
            return new DateExpressionValue(value.AddTicks(-other.ToInt()));
        }

        /// <summary>
        /// Multiplication function for expression values to override to handle arithmetic operations (throws NotImplementedException)
        /// </summary>
        /// <param name="other">the value to multiply this value by</param>
        /// <returns>an expression value</returns>
        protected override ExpressionValue Multiply(ExpressionValue other)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Division function for expression values to override to handle arithmetic operations (throws NotImplementedException)
        /// </summary>
        /// <param name="other">the value to  divide this value by</param>
        /// <returns>an expression value</returns>
        protected override ExpressionValue DivideBy(ExpressionValue other)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Implementation of IComparable
        /// </summary>
        /// <param name="obj">The other object to compare with.</param>
        /// <returns>The result of the comparison</returns>
        public override int CompareTo(object obj)
        {
            return value.CompareTo(obj);
        }

        /// <summary>
        /// Implementation of IComparable
        /// </summary>
        /// <param name="obj">The other object to compare with.</param>
        /// <returns>The result of the comparison</returns>
        public override bool Equals(object obj)
        {
            return value.Equals(obj);
        }

        /// <summary>
        /// Convert (if needed) the underlying value to an integer value.
        /// </summary>
        /// <returns>The integer value.</returns>
        public override int ToInt()
        {
            return (int)value.Ticks;
        }

        /// <summary>
        /// Convert (if needed) the underlying value to a double value.
        /// </summary>
        /// <returns>The double value</returns>
        public override double ToDouble()
        {
            return (double)value.Ticks;
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
            return value;
        }

        /// <summary>
        /// Converts a value to a boolean. (throws NotImplementedException)
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public override bool ToBoolean()
        {
            throw new NotImplementedException();
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
