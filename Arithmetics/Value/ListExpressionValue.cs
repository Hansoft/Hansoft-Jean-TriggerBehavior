using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Value
{
    class ListExpressionValue:ExpressionValue
    {
        private IList value;

        public ListExpressionValue(IList value)
        {
            this.value = value;
        }

        /// <summary>
        /// Clear the list of dupllicate elements.
        /// </summary>
        private static IList RemoveDuplicates(IList list)
        {
            List<object> clearedList = new List<object>();
            foreach (object o in list)
            {
                if(!clearedList.Contains(o))
                    clearedList.Add(o);
            }
            return clearedList;
        }

        /// <summary>
        /// Addition function for expression values to override to handle arithmetic operations
        /// </summary>
        /// <param name="other">the value to add to this value</param>
        /// <returns>an expression value</returns>
        protected override ExpressionValue Add(ExpressionValue other)
        {
            IList otherList = other.ToList();
            IList newList = value; // we don't want to modify this list
            foreach(object o in otherList)
                newList.Add(o);
            return new ListExpressionValue(RemoveDuplicates(newList));
        }


        /// <summary>
        /// Subtraction function for expression values to override to handle arithmetic operations
        /// </summary>
        /// <param name="other">the value to subtract to this value</param>
        /// <returns>an expression value</returns>
        protected override ExpressionValue Subtract(ExpressionValue other)
        {
            throw new InvalidOperationException("Cannot apply subtraction on a list expression value. List: " + ToString());
        }

        /// <summary>
        /// Multiplication function for expression values to override to handle arithmetic operations
        /// </summary>
        /// <param name="other">the value to multiply this value by</param>
        /// <returns>an expression value</returns>
        protected override ExpressionValue Multiply(ExpressionValue other)
        {
            throw new InvalidOperationException("Cannot apply multiplication on a list expression value. List: " + ToString());
        }

        /// <summary>
        /// Division function for expression values to override to handle arithmetic operations
        /// </summary>
        /// <param name="other">the value to  divide this value by</param>
        /// <returns>an expression value</returns>
        protected override ExpressionValue DivideBy(ExpressionValue other)
        {
            throw new InvalidOperationException("Cannot apply division on a list expression value. List: " + ToString());
        }


        /// <summary>
        /// Implementation of IComparable
        /// </summary>
        /// <param name="obj">The other object to compare with.</param>
        /// <returns>The result of the comparison</returns>
        public override int CompareTo(object obj)
        {
            throw new InvalidOperationException("Cannot compare a list expression value to another object. List: " + ToString());
        }

        /// <summary>
        /// Implementation of IComparable
        /// </summary>
        /// <param name="obj">The other object to compare with.</param>
        /// <returns>The result of the comparison</returns>
        public override bool Equals(object obj)
        {
            throw new InvalidOperationException("Cannot compare a list expression value to another object. List: " + ToString());
        }

        /// <summary>
        /// Convert (if needed) the underlying value to an integer value.
        /// </summary>
        /// <returns>The integer value.</returns>
        public override int ToInt()
        {
            throw new ArgumentException("Cannot convert the list: '" + ToString() + "' to a number");
        }

        /// <summary>
        /// Convert (if needed) the underlying value to a double value.
        /// </summary>
        /// <returns>The double value</returns>
        public override double ToDouble()
        {
            throw new ArgumentException("Cannot convert the list: '" + ToString() + "' to a number");
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
            return value;
        }

        /// <summary>
        /// Convert (if needed) the underlying value to a list of strings.
        /// </summary>
        /// <returns>The list</returns>
        public override List<string> ToStringList()
        {
            List<string> list = new List<string>();
            foreach(object o in value)
                list.Add(o.ToString());
            return list;
        }

        /// <summary>
        /// IConvertible override
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public override DateTime ToDateTime(IFormatProvider provider)
        {
            throw new ArgumentException("Cannot convert the list: '" + ToString() + "' to a date");
        }

        /// <summary>
        /// Converts a value to a boolean.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public override bool ToBoolean()
        {
            throw new ArgumentException("Cannot convert the list: '" + ToString() + "' to a boolean");
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
