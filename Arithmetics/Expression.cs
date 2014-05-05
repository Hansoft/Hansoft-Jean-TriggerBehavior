using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Tokens;
using HPMSdk;
using Hansoft.ObjectWrapper;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics
{
    class Expression: IExpressionItem
    {
        IExpressionItem root;

        public Expression(IExpressionItem root)
        {
            this.root = root;
        }

        public ExpressionValue Evaluate(Task task)
        {
            return root.Evaluate(task);
        }

        private static IExpressionItem buildExpression(List < IExpressionItem > tokens)
        {
            //Find the operator with the highest precendence (the least important) and start building the expression from that one
            //If two operators with equal precendence exists the first one will be used as the root.
            int precendence = -1;
            int operatorIndex = -1;
            for (int i = 0; i < tokens.Count; ++i)
            {
                Operator op = tokens.ElementAt(i) as Operator;
                //Check if the token is an operator with higher precendence
                if (op != null && op.Precendence > precendence)
                {
                    precendence = op.Precendence;
                    operatorIndex = i;
                }
            }
            //There was no operator in the token list, so this should be a single token. Convert it to a proper one.
            if(operatorIndex == -1)
            {
                if (tokens.Count > 1)
                    throw new ArgumentException("Malformed expression. Expected an only one token. " + tokens.ToString());
                IntermediateToken token = tokens.ElementAt(0) as IntermediateToken;
                if (token == null)
                    throw new ArgumentException("Malformed expression. Expected an IntermediateToken, but found this: " + tokens.ToString());
                return Tokenizer.Compile(token);
            }
            else
            {
                // Build the two expressions around the operator
                Operator op = tokens.ElementAt(operatorIndex) as Operator;
                op.Left = buildExpression(tokens.GetRange(0, operatorIndex));
                op.Right = buildExpression(tokens.GetRange(operatorIndex + 1, tokens.Count - operatorIndex -1));
                return op;
            }
        }

        /* TODO: Add support for paranthesises in the expressions.
        public static List<IExpressionItem> parenthesise(string expression)
        {
            //Stack of parenthesis that indexes
           List<IExpressionItem> expressions = new List<IExpressionItem>();
           Stack<int> parenthesistStartStack = new Stack<int>();
           for(int i = 0; i < expression.Length-1; ++i)
           {
               char ch = expression.ElementAt(i);
               if (ch == '(')
                   parenthesistStartStack.Push(i);
               else if(ch == ')')
               {
                   if (parenthesistStartStack.Count == 0)
                       throw new ArgumentException("Malformed exception, end parenthesis found without a corresponding start: " + expression);
                   int startParenthesis = parenthesistStartStack.Pop();
                   expressions.Add(new IntermediateToken(expression.Substring(startParenthesis, i - startParenthesis)));
               }
           }
           if (parenthesistStartStack.Count > 0)
               throw new ArgumentException("Malformed exception, missing end parenthesis: " + expression);
           return expressions;
        }*/


        public static Expression parse(string expression)
        {
            int operatorStart = 0;
            int opEnd = 0;
            int prevOpEnd = 0;
            List<IExpressionItem> tokens = new List<IExpressionItem>();
            Operator op;
            //Tokenize the expression string
            do
            {
                prevOpEnd = opEnd;
                op = Operator.Find(expression, ref operatorStart, ref opEnd);
                if (op != null)
                {
                    tokens.Add(new IntermediateToken(expression.Substring(prevOpEnd, operatorStart - prevOpEnd)));
                    tokens.Add(op);
                }
                else
                {
                    tokens.Add(new IntermediateToken(expression.Substring(prevOpEnd, expression.Length - prevOpEnd)));
                }
                operatorStart = opEnd;
            }
            while (op != null);

            return new Expression(buildExpression(tokens));
        }

        public void AddAffectedBy(ref List<ListenerData> list)
        {
            root.AddAffectedBy(ref list);
        }

    }
}
