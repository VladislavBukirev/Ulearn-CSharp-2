namespace SQ4
{
    class Task
    {
        public static void Main()
        {
            var str = "";
            foreach (var abc in Translator("5*6+(2-9)"))
            {
                str += abc;
            }
            Console.WriteLine(str);
            Console.WriteLine(Solver(str));
        }

        private static int Solver(string postfixString)
        {
            var stack = new Stack<int>();
            foreach (var symbol in postfixString)
            {
                if (char.IsDigit(symbol))
                {
                    stack.Push(int.Parse(symbol.ToString()));
                    continue;
                }

                switch (symbol)
                {
                    case '+':
                        stack.Push(stack.Pop() + stack.Pop());
                        break;
                    case '-':
                        stack.Push(stack.Pop() - stack.Pop());
                        break;
                    case '*':
                        stack.Push(stack.Pop() * stack.Pop());
                        break;
                    case '/':
                        stack.Push(stack.Pop() / stack.Pop());
                        break;
                }
            }
            return stack.Last();
        }

        private static Queue<string> Translator(string infixString)
        {
            var priorityDict = new Dictionary<char, int>()
            {
                { '-', 0 },
                { '+', 0 },
                { '*', 1 },
                { '/', 1 }
            };
            var stack = new Stack<char>();
            var queue = new Queue<string>();
            foreach (var symbol in infixString)
            {
                if (char.IsDigit(symbol))
                {
                    queue.Enqueue(symbol.ToString());
                    continue;
                }

                if (priorityDict.ContainsKey(symbol))
                {   
                    if (stack.Count == 0 || stack.Last() == '(')
                    {
                        stack.Push(symbol);
                        continue;
                    }

                    if (priorityDict[symbol] > priorityDict[stack.Last()])
                    {
                        stack.Push(symbol);
                        continue;
                    }

                    if (priorityDict[symbol] <= priorityDict[stack.Last()])
                    {
                        if (priorityDict.ContainsKey(stack.LastOrDefault()))
                        {
                            // for (var i = 0; i < stack.Count; i++)
                            // {
                                while (stack.Count != 0 
                                       && priorityDict[symbol] < priorityDict[stack.Last()]
                                       && stack.Last() != '(')
                                {
                                    queue.Enqueue(stack.Pop().ToString());
                                }
                        }
                        stack.Push(symbol);
                    }
                }

                if (symbol == '(')
                {
                    stack.Push(symbol);
                    continue;
                }

                if (symbol == ')')
                {
                    for (var i = 0; i < stack.Count; i++)
                    {
                        if (stack.First().ToString() == "(")
                            break;
                        queue.Enqueue(stack.Pop().ToString());
                    }

                    stack.Pop();
                }
            }

            while (stack.Count != 0)
            {
                queue.Enqueue(stack.Pop().ToString());
            }

            return queue;
        }
    }
}
