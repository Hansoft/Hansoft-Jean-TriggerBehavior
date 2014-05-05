using Hansoft.ObjectWrapper;
using HPMSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics
{
    enum OperatorType
        {
            OP_ADD,
            OP_SUB,
            OP_DIV,
            OP_MUL,
            OP_AND,
            OP_OR,
            OP_LT,
            OP_LTE,
            OP_GT,
            OP_GTE,
            OP_EQ,
            OP_NEQ,
        };
    //TODO: Add +=, -=

    class Operator: IExpressionItem
    {
        struct OperatorDefintion
        {
            public string str;
            public OperatorType operatorID;
            public int precedence;

            public OperatorDefintion(string str, OperatorType operatorID, int precedence)
            {
                this.str = str;
                this.operatorID = operatorID;
                this.precedence = precedence;
            }
        }

        private static OperatorDefintion[] operatorDefitions = 
        {
            new OperatorDefintion("+", OperatorType.OP_ADD,4),
            new OperatorDefintion("-", OperatorType.OP_SUB,4),
            new OperatorDefintion("*", OperatorType.OP_MUL,3),
            new OperatorDefintion("/", OperatorType.OP_DIV,3),
            new OperatorDefintion("AND", OperatorType.OP_AND,11),
            new OperatorDefintion("OR", OperatorType.OP_OR,12),
            new OperatorDefintion("<", OperatorType.OP_LT,6),
            new OperatorDefintion("<=", OperatorType.OP_LTE,6),
            new OperatorDefintion(">", OperatorType.OP_GT,6),
            new OperatorDefintion(">=", OperatorType.OP_GTE,6),
            new OperatorDefintion("==", OperatorType.OP_EQ,7),
            new OperatorDefintion("!=", OperatorType.OP_NEQ,7)
        };

        private OperatorType operatorID;
        private IExpressionItem left;
        private IExpressionItem right;
        private int precedence;

        public OperatorType OpID
        {
            get { return operatorID; }
            set { this.operatorID = value; }
        }


        /*
         * Returns an operator if the string starts with that otherwise null.
         */
        private static Operator GetOperator(string expression, int currentPosition, ref int endPostion)
        {
            for(int i = 0; i<operatorDefitions.Length; ++i)
            {
                if (expression.Length >= (currentPosition + operatorDefitions[i].str.Length) && expression.Substring(currentPosition, operatorDefitions[i].str.Length).Equals(operatorDefitions[i].str))
                {
                    endPostion = currentPosition + operatorDefitions[i].str.Length;
                    Operator op = new Operator();
                    op.OpID = operatorDefitions[i].operatorID;
                    op.Precendence = operatorDefitions[i].precedence;
                    return op;
                }
            }
            return null;
        }


        public static Operator Find(string expression, ref int startPos, ref int endPos)
        {
            Operator op;
            for (int i = startPos; i < expression.Length; ++i)
            {
                op = GetOperator(expression, i, ref endPos);
                if (op != null)
                {
                    startPos = i;
                    return op;
                }
            }
            endPos = expression.Length - 1;
            return null;
        }

        public IExpressionItem Left
        {
            get { return left; }
            set { left = value; }
        }

        public IExpressionItem Right
        {
            get { return right; }
            set { right = value; }
        }

        public int Precendence
        {
            get { return precedence; }
            set { this.precedence = value; }
        }

        public ExpressionValue Evaluate(Task task)
        {
            switch (operatorID)
            {
                    //Todo: Handle INT and FLOAT differently
                case (OperatorType.OP_ADD):
                    {
                        return left.Evaluate(task) + right.Evaluate(task);
                    }
                case (OperatorType.OP_SUB):
                    {
                        return left.Evaluate(task) - right.Evaluate(task);
                    }
                case (OperatorType.OP_DIV):
                    {
                        return left.Evaluate(task) / right.Evaluate(task);
                    }
                case (OperatorType.OP_MUL):
                    {
                        return left.Evaluate(task) * right.Evaluate(task);
                    }
                case (OperatorType.OP_AND):
                    {
                        return new ExpressionValue(ExpressionValueType.BOOL, left.Evaluate(task).ToBoolean() && right.Evaluate(task).ToBoolean());
                    }
                case (OperatorType.OP_OR):
                    {
                        return new ExpressionValue(ExpressionValueType.BOOL, left.Evaluate(task).ToBoolean() || right.Evaluate(task).ToBoolean());
                    }
                case (OperatorType.OP_LT):
                    {
                        return left.Evaluate(task) < right.Evaluate(task);
                    }
                case (OperatorType.OP_LTE):
                    {
                        return left.Evaluate(task) <= right.Evaluate(task);
                    }
                case (OperatorType.OP_GT):
                    {
                        return left.Evaluate(task) > right.Evaluate(task);
                    }
                case (OperatorType.OP_GTE):
                    {
                        return left.Evaluate(task) >= right.Evaluate(task);
                    }
                case (OperatorType.OP_EQ):
                    {
                        return left.Evaluate(task) == right.Evaluate(task);
                    }
                case (OperatorType.OP_NEQ):
                    {
                        return left.Evaluate(task) != right.Evaluate(task);
                    }
                default:
                    throw new ArgumentException("Unknown operator " + operatorID);

            }
        }

        public void AddAffectedBy(ref List<ListenerData> list)
        {
            if (left != null)
                left.AddAffectedBy(ref list);
            if (right != null)
                right.AddAffectedBy(ref list);
        }

    
    }
}
