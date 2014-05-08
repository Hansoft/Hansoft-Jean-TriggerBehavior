using Hansoft.ObjectWrapper.CustomColumnValues;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Value
{
    public abstract class ExpressionValue : IConvertible, IComparable
    {
        #region Overrides

        /// <summary>
        /// Addition function for expression values to override to handle arithmetic operations
        /// </summary>
        /// <param name="other">the value to add to this value</param>
        /// <returns>an expression value</returns>
        protected abstract ExpressionValue Add(ExpressionValue other);

        /// <summary>
        /// Subtraction function for expression values to override to handle arithmetic operations
        /// </summary>
        /// <param name="other">the value to subtract to this value</param>
        /// <returns>an expression value</returns>
        protected abstract ExpressionValue Subtract(ExpressionValue other);

        /// <summary>
        /// Multiplication function for expression values to override to handle arithmetic operations
        /// </summary>
        /// <param name="other">the value to multiply this value by</param>
        /// <returns>an expression value</returns>
        protected abstract ExpressionValue Multiply(ExpressionValue other);


        /// <summary>
        /// Division function for expression values to override to handle arithmetic operations
        /// </summary>
        /// <param name="other">the value to  divide this value by</param>
        /// <returns>an expression value</returns>
        protected abstract ExpressionValue DivideBy(ExpressionValue other);


        /// <summary>
        /// Implementation of IComparable
        /// </summary>
        /// <param name="obj">The other object to compare with.</param>
        /// <returns>The result of the comparison</returns>
        public abstract int CompareTo(object obj);

        /// <summary>
        /// Implementation of IComparable
        /// </summary>
        /// <param name="obj">The other object to compare with.</param>
        /// <returns>The result of the comparison</returns>
        public override abstract bool Equals(object obj);


        /// <summary>
        /// Convert (if needed) the underlying value to an integer value.
        /// </summary>
        /// <returns>The integer value.</returns>
        public abstract int ToInt();

        /// <summary>
        /// Convert (if needed) the underlying value to a double value.
        /// </summary>
        /// <returns>The double value</returns>
        public abstract double ToDouble();

        /// <summary>
        /// Convert (if needed) the underlying value to a string value.
        /// </summary>
        /// <returns>The string value</returns>
        public override abstract string ToString();

        /// <summary>
        /// Convert (if needed) the underlying value to a list value.
        /// </summary>
        /// <returns>The list</returns>
        public abstract IList ToList();

        /// <summary>
        /// Convert (if needed) the underlying value to a list of strings.
        /// </summary>
        /// <returns>The list</returns>
        public abstract List<string> ToStringList();

        /// <summary>
        /// IConvertible override, delegated to subclasses.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public abstract DateTime ToDateTime(IFormatProvider provider);

        /// <summary>
        /// Converts a value to a boolean.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public abstract bool ToBoolean();

        #endregion //Overrides



        /// <summary>
        /// Overloads addition operator
        /// </summary>
        /// <param name="left">the left hand side of the expression</param>
        /// <param name="right">the right hand side of the expression</param>
        /// <returns>an expression value</returns>
        public static ExpressionValue operator +(ExpressionValue left, ExpressionValue right)
        {
            return left.Add(right);
        }
        /// <summary>
        /// Overloads subtraction operator
        /// </summary>
        /// <param name="left">the left hand side of the expression</param>
        /// <param name="right">the right hand side of the expression</param>
        /// <returns>an expression value</returns>
        public static ExpressionValue operator -(ExpressionValue left, ExpressionValue right)
        {
            return left.Subtract(right);
        }

        /// <summary>
        /// Overloads multiplication operator
        /// </summary>
        /// <param name="left">the left hand side of the expression</param>
        /// <param name="right">the right hand side of the expression</param>
        /// <returns>an expression value</returns>
        public static ExpressionValue operator *(ExpressionValue left, ExpressionValue right)
        {
            return left.Multiply(right);
        }

        /// <summary>
        /// Overloads division operator
        /// </summary>
        /// <param name="left">the left hand side of the expression</param>
        /// <param name="right">the right hand side of the expression</param>
        /// <returns>an expression value</returns>
        public static ExpressionValue operator /(ExpressionValue left, ExpressionValue right)
        {
            return left.DivideBy(right);
        }


        /// <summary>
        /// IConvertible override, returns TypeCode.Object.
        /// </summary>
        /// <returns></returns>
        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
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

        /// <summary>
        /// IConvertible override, will throw NotImplementedException if called.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public bool ToBoolean(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// IConvertible override, will throw NotImplementedException if called.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public char ToChar(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ICovertible override
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public byte ToByte(IFormatProvider provider)
        {
            return (byte)ToInt();
        }

        /// <summary>
        /// ICovertible override
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public decimal ToDecimal(IFormatProvider provider)
        {
            return (decimal)ToDouble();
        }

        /// <summary>
        /// ICovertible override
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public double ToDouble(IFormatProvider provider)
        {
            return ToDouble();
        }

        /// <summary>
        /// ICovertible override
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public short ToInt16(IFormatProvider provider)
        {
            return (short)ToInt();
        }

        /// <summary>
        /// ICovertible override
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public int ToInt32(IFormatProvider provider)
        {
            return (int)ToInt();
        }

        /// <summary>
        /// ICovertible override
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public long ToInt64(IFormatProvider provider)
        {
            return ToInt();
        }

        /// <summary>
        /// ICovertible override
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public sbyte ToSByte(IFormatProvider provider)
        {
            return (sbyte)ToInt();
        }

        /// <summary>
        /// ICovertible override
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public float ToSingle(IFormatProvider provider)
        {
            return (float)ToDouble();
        }

        /// <summary>
        /// ICovertible override
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public string ToString(IFormatProvider provider)
        {
            return ToString();
        }

        /// <summary>
        /// IConvertible override, will throw NotImplementedException if called.
        /// </summary>
        /// <param name="conversionType"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ICovertible override
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public ushort ToUInt16(IFormatProvider provider)
        {
            return (ushort)ToInt();
        }

        /// <summary>
        /// ICovertible override
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public uint ToUInt32(IFormatProvider provider)
        {
            return (uint)ToInt();
        }

        /// <summary>
        /// ICovertible override
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public ulong ToUInt64(IFormatProvider provider)
        {
            return (ulong)ToInt();
        }


    }
}
