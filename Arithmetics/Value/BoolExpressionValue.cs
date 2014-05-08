using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Value
{
    class BoolExpressionValue : ExpressionValue
    {
        private bool value;

        public BoolExpressionValue(bool value)
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
            throw new InvalidOperationException("Cannot perform addition on a boolean");
        }


        /// <summary>
        /// Subtraction function for expression values to override to handle arithmetic operations
        /// </summary>
        /// <param name="other">the value to subtract to this value</param>
        /// <returns>an expression value</returns>
        protected override ExpressionValue Subtract(ExpressionValue other)
        {
            throw new InvalidOperationException("Cannot perform subtraction on a boolean");
        }

        /// <summary>
        /// Multiplication function for expression values to override to handle arithmetic operations
        /// </summary>
        /// <param name="other">the value to multiply this value by</param>
        /// <returns>an expression value</returns>
        protected override ExpressionValue Multiply(ExpressionValue other)
        {
            throw new InvalidOperationException("Cannot perform multiplication on a boolean");
        }

        /// <summary>
        /// Division function for expression values to override to handle arithmetic operations
        /// </summary>
        /// <param name="other">the value to  divide this value by</param>
        /// <returns>an expression value</returns>
        protected override ExpressionValue DivideBy(ExpressionValue other)
        {
            throw new InvalidOperationException("Cannot perform division on a boolean");
        }


        /// <summary>
        /// Implementation of IComparable
        /// </summary>
        /// <param name="obj">The other object to compare with.</param>
        /// <returns>The result of the comparison</returns>
        public override int CompareTo(object obj)
        {
            throw new InvalidOperationException("Cannot compare boolean ExpressionValue to other object.");
        }

        /// <summary>
        /// Implementation of IComparable
        /// </summary>
        /// <param name="obj">The other object to compare with.</param>
        /// <returns>The result of the comparison</returns>
        public override bool Equals(object obj)
        {
            BoolExpressionValue val = obj as BoolExpressionValue;
            if (val == null)
                throw new ArgumentException("Cannot compare an ExpressionValue to other things than BoolExpressionValue.");
            return ToInt() == val.ToInt();
        }

        /// <summary>
        /// Convert (if needed) the underlying value to an integer value.
        /// </summary>
        /// <returns>The integer value.</returns>
        public override int ToInt()
        {
            return value?1:0;
        }

        /// <summary>
        /// Convert (if needed) the underlying value to a double value.
        /// </summary>
        /// <returns>The double value</returns>
        public override double ToDouble()
        {
            throw new InvalidOperationException("Cannot convert a boolean ExpressionValue to a double.");
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
            throw new InvalidOperationException("Cannot convert a boolean ExpressionValue to a date.");
        }

        /// <summary>
        /// Converts a value to a boolean.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public override bool ToBoolean()
        {
            return value;
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
