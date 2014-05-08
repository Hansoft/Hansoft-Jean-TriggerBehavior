using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Tokens;
using HPMSdk;
using Hansoft.ObjectWrapper;
using Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics.Value;

namespace Hansoft.Jean.Behavior.TriggerBehavior.Arithmetics
{
    class Expression: IExpressionItem
    {
        IExpressionItem root;
        private const char STRING_ENCAPSULATOR = '\'';
        private const char PARENTHESIS_START = '(';
        private const char PARENTHESIS_END = ')';
        private const char LIST_START = '[';
        private const char LIST_END = ']';
        private const char LIST_DELIMINATOR = ',';


        public Expression(IExpressionItem root)
        {
            this.root = root;
        }

        /// <summary>
        /// Evaluates this expression on the task and returns the expression value
        /// </summary>
        /// <param name="task">the task to evaluate the expression on</param>
        /// <returns>the value this expression returns.</returns>
        public ExpressionValue Evaluate(Task task)
        {
            return root.Evaluate(task);
        }


        /// <summary>
        /// Keeps track of the state of the parser.
        /// </summary>
        class ParserState
        {
            // To handle paranthesises we need to store all expression as a stack of expressions lists.
            private Stack<List<IExpressionItem>> expressionStack;
            private bool inString;
            private ListToken currentList;
            private string currentToken;

            /// <summary>
            /// Creates a new empty parser state
            /// </summary>
            public ParserState()
            {
                expressionStack = new Stack<List<IExpressionItem>>();
                inString = false;
                currentList = null;
                currentToken = "";
            }

            /// <summary>
            /// Getter and setter for whether the parser is in a string.
            /// </summary>
            public bool InString
            {
                get { return inString; }
                set { inString = value;  }
            }

            /// <summary>
            /// Getter for whether the parser is in a list.
            /// </summary>
            public bool InList
            {
                get { return currentList != null; }
            }

            /// <summary>
            /// Getter for number of expression layers in the parser
            /// </summary>
            public int NumLayers
            {
                get { return expressionStack.Count; }
            }

            /// <summary>
            /// Pushes a character to the current token
            /// </summary>
            /// <param name="ch">character to push</param>
            public void PushChar(char ch)
            {
                currentToken += ch;
            }

            /// <summary>
            /// Pushes an item to the current list. If not in a list it will fail silently.
            /// </summary>
            /// <param name="expression">the expression to push into the list</param>
            public void PushListItem(IExpressionItem expression)
            {
                if(InList) 
                    currentList.AddItem(expression);
            }

            /// <summary>
            /// Creates a new list that expressions can be added to. Will also create another expression layer
            /// where any list items will end up.
            /// </summary>
            public void PushList()
            {
                currentList = new ListToken();
                PushExpressionLayer();
            }

            /// <summary>
            /// Returns the current list that has been build and clears it. Cleaning up expressions layers needs to be managed separetely.
            /// </summary>
            /// <returns>A list token that is the list that has been built</returns>
            public ListToken PopList()
            {
                ListToken token = currentList;
                currentList = null;
                return token; 
            }

            /// <summary>
            /// Will take the current token string and compile it and move it to the current expression layer.
            /// </summary>
            /// <param name="errors">Any errors in the parsing of the token will be added to this list</param>
            public void MoveCurrentTokenToExpressionStack(ref List<string> errors)
            {
                string tokenStr = currentToken.Trim();
                IExpressionItem token = Tokenizer.Compile(tokenStr, ref errors);
                if (token != null)
                    PushExpression(token);
                currentToken = "";
            }

            /// <summary>
            /// Will return the current token string and clear it.
            /// </summary>
            /// <returns>The current token</returns>
            public string PopCurrentToken()
            {
                string tokenStr = currentToken;
                currentToken = "";
                return tokenStr;
            }

            /// <summary>
            /// Pushes an expression into the current top expression layer
            /// </summary>
            /// <param name="expression">expression to push in</param>
            public void PushExpression(IExpressionItem expression)
            {
                expressionStack.Peek().Add(expression);
            }

            /// <summary>
            /// Adds a new expression layer to the parser
            /// </summary>
            public void PushExpressionLayer()
            {
                expressionStack.Push(new List<IExpressionItem>());
            }

            /// <summary>
            /// Removes the top expression layer from the stack
            /// </summary>
            /// <returns>The list of expressions in the top layer</returns>
            public List<IExpressionItem> PopExpressionLayer()
            {
                return expressionStack.Pop();
            }
        }

        /// <summary>
        /// Builds an expression of the list of items and adds any errors to the list.
        /// </summary>
        /// <param name="tokens">Tokens to create an complex expression for</param>
        /// <param name="errors">List to add errors to</param>
        /// <returns></returns>
        private static IExpressionItem buildExpression(List<IExpressionItem> tokens, ref List<string> errors)
        {
            //If the list is empty we return null
            if (tokens.Count == 0)
                return null;
            //Find the operator with the highest precendence (the least important) and start building the expression from that one
            //If two operators with equal precendence exists the first one will be used as the root.
            int precendence = -1;
            int operatorIndex = -1;
            for (int i = 0; i < tokens.Count; ++i)
            {
                Operator op = tokens.ElementAt(i) as Operator;
                //Check if the token is an operator with higher precendence
                if (op != null && !op.IsComplete && op.Precendence > precendence)
                {
                    precendence = op.Precendence;
                    operatorIndex = i;
                }
            }
            //There was no operator in the token list, so this should be a single token. Convert it to a proper one.
            if (operatorIndex == -1)
            {
                if (tokens.Count > 1)
                    errors.Add("Malformed expression. Expected only one token. " + tokens.ToString());
                if (tokens.Count == 0)
                    return null;
                return tokens.ElementAt(0);
            }
            else
            {
                // Build the two expressions around the operator
                Operator op = tokens.ElementAt(operatorIndex) as Operator;
                if (!op.IsComplete)
                {
                    op.Left = buildExpression(tokens.GetRange(0, operatorIndex), ref errors);
                    op.Right = buildExpression(tokens.GetRange(operatorIndex + 1, tokens.Count - operatorIndex - 1), ref errors);
                }
                return op;
            }
        }


        /// <summary>
        /// Parses the string expression and returns an expression that can be evaluated.
        /// </summary>
        /// <param name="expression">The expression that will be parsed</param>
        /// <param name="errors">A reference to a list of strings that will be filled up with all the parse errors</param>
        /// <returns>An Expression that will be able to evaluated on a task. Null if there are any errors in the parsing process.</returns>
        public static Expression parse(string expression, ref List<string> errors)
        {
            // Clean out whitespaces at the start and end of the expression
            expression = expression.Trim();

            ParserState state = new ParserState();
            Operator op;
            int operatorEnd = 0;

            state.PushExpressionLayer();

            for(int i = 0; i < expression.Length; ++i)
            {
                char currentChar = expression.ElementAt(i);
                //This is a but ugly, but let's go with it. First we need to check if we are at an operator (as long as we're not in a string).
                if (!state.InString)
                {
                    op = Operator.GetOperator(expression, i, ref operatorEnd);
                    if (op != null)
                    {
                        // We need to put the current token (if it's exists) on the stack.
                        state.MoveCurrentTokenToExpressionStack(ref errors);
                        state.PushExpression(op);
                        i = operatorEnd-1;
                        //We found an operator, so we'll just push the loop past the operator.
                        continue;
                    }
                }
                switch(currentChar)
                {
                    case STRING_ENCAPSULATOR:
                        {
                            if (state.InString)
                            {
                                state.PushExpression(new StringToken(state.PopCurrentToken()));
                                state.InString = false;
                            }
                            else
                            {
                                state.InString = true;
                                // We need to put the current token (if it's exists) on the stack.
                                state.MoveCurrentTokenToExpressionStack(ref errors);
                            }
                            break;
                        }
                    case PARENTHESIS_START:
                        {
                            // We need to put the current token (if it's exists) on the stack.
                            state.MoveCurrentTokenToExpressionStack(ref errors);
                            // Add a new layer to start adding expression to.
                            state.PushExpressionLayer();
                            break;
                        }
                    case PARENTHESIS_END:
                        {
                            //An end of a paranthesis has been discovered. Now we need to convert the whole expression list to an expression
                            //and push down to the expression list in the previous layer. But first we need to ensure that there was a start to this
                            //paranthesis.
                            if (state.NumLayers < 2) // There has to be at least two layers (base + 1 start paranthesis) in order for the expression to be wellformed.
                            {
                                errors.Add("Paranthesis mismatch. Found a paranthesis end without a matching start: " + expression);
                                //This is not an error we can recover from.
                                return null;
                            }
                            // We need to put the current token (if it's exists) on the stack.
                            state.MoveCurrentTokenToExpressionStack(ref errors);

                            IExpressionItem innerExpression = buildExpression(state.PopExpressionLayer(), ref errors);
                            // We'll add the innerexpression to the layer below.
                            if (innerExpression != null) //() is allowed, thus there could be no expressions in the layer.
                                state.PushExpression(innerExpression);
                            break;
                        }
                    case LIST_START:
                        {
                            // We need to put the current token (if it's exists) on the stack.
                            state.MoveCurrentTokenToExpressionStack(ref errors);
                            if (state.InList)
                            {
                                errors.Add("List mismatch. Lists within lists are not allowed: " + expression);
                                //This is not an error we can recover from.
                                return null;
                            }
                            //Push a new list to the state. This will also add a new expression layer which needs to be removed once the list is completed.
                            state.PushList();
                            break;
                        }
                    case LIST_END:
                        {
                            if (!state.InList)
                            {
                                errors.Add("List mismatch. Found a list end without a start: " + expression);
                                //This is not an error we can recover from.
                                return null;
                            }
                            //We're done with the list, add the last item.
                            state.PushListItem(buildExpression(state.PopExpressionLayer(), ref errors));
                            state.PushExpression(state.PopList());
                            break;
                        }
                    case LIST_DELIMINATOR:
                        {
                            if (!state.InList)
                            {
                                errors.Add("Found a list deliminator(" + LIST_DELIMINATOR + ") outside of a list and string: " + expression);
                                //This is not an error we can recover from.
                                return null;
                            }
                            state.PushListItem(buildExpression(state.PopExpressionLayer(), ref errors));
                            // For each list deliminator we need to remove the old layer and add a new expression layer.
                            state.PushExpressionLayer();
                            break;
                        }
                    default:
                        {
                            state.PushChar(currentChar);
                            break;
                        }
                    }
            }
            //Push the remainder on to the stack
            state.MoveCurrentTokenToExpressionStack(ref errors);

            //We're done looping through the expression, now we just got to make sure the expression was welformed.
            if (state.InString)
            {
                errors.Add("String mismatch. There is a missing end of string in the expression: " + expression);
                return null;
            }
            if (state.InList)
            {
                errors.Add("List mismatch. There is a missing end of list in the expression: " + expression);
                return null;
            }
            if (state.NumLayers > 1)
            {
                errors.Add("Paranthesis mismatch. There is at least one missing end of paranthesis in the expression: " + expression);
                return null;
            }

            // Let's pop the base layer and return that as an expression
            return new Expression(buildExpression(state.PopExpressionLayer(), ref errors));
        }

        /// <summary>
        /// Adds all the expressions listeners
        /// </summary>
        /// <param name="list">the list to add listeners to</param>
        public void AddAffectedBy(ref List<ListenerData> list)
        {
            root.AddAffectedBy(ref list);
        }

    }
}
