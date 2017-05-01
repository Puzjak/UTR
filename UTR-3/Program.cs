using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace UTR_3
{
    public class Program
    {
        public static List<string[]> Inputs = new List<string[]>();
        public static string[] States;
        public static string[] Letters;
        public static string[] StackLetters;
        public static string[] AcceptableStates;
        public static string InitialState;
        public static string InitialLetter;
        public static LinkedList<string> Stack = new LinkedList<string>();

        public static Dictionary<KeyValuePair<string, KeyValuePair<string, string>>, KeyValuePair<string, string>>
            Transitions
                = new Dictionary<KeyValuePair<string, KeyValuePair<string, string>>, KeyValuePair<string, string>>();

        public static void Main(string[] args)
        {
            var line = Console.ReadLine();
            var tmp = line.Split('|');
            foreach (var s in tmp)
            {
                Inputs.Add(s.Split(','));
            }

            line = Console.ReadLine();
            States = line.Split(',');

            line = Console.ReadLine();
            Letters = line.Split(',');

            line = Console.ReadLine();
            StackLetters = line.Split(',');

            AcceptableStates = Console.ReadLine().Split(',');
            InitialState = Console.ReadLine();
            InitialLetter = Console.ReadLine();

            while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
            {
                tmp = line.Split(new[] {"->"}, StringSplitOptions.None);
                var tmp2 = tmp[0].Split(',');
                var tmp3 = tmp[1].Split(',');
                Transitions.Add(
                    new KeyValuePair<string, KeyValuePair<string, string>>(tmp2[0],
                        new KeyValuePair<string, string>(tmp2[1], tmp2[2])),
                    new KeyValuePair<string, string>(tmp3[0], tmp3[1]));
            }

            //Solution
            var sb = new StringBuilder();
            foreach (var input in Inputs)
            {
                var currentState = InitialState;
                sb.Clear();
                Stack.Clear();
                Stack.AddFirst(InitialLetter);
                int counter = 0;
                sb.AppendFormat("{0}#{1}|", InitialState, InitialLetter);

                foreach (var letter in input)
                {
                    counter++;

                    try
                    {
                        while (true)
                        {
                            var transition = Transitions[
                                new KeyValuePair<string, KeyValuePair<string, string>>(currentState,
                                    new KeyValuePair<string, string>("$", Stack.First.Value))];
                            currentState = transition.Key;
                            if (transition.Value == "$")
                                Stack.RemoveFirst();
                            else
                            {
                                var tranValArray = transition.Value.ToCharArray().Reverse().ToArray();
                                if(tranValArray.Length > 1)
                                    for (int i = 0; i < tranValArray.Length; i++)
                                    {
                                        if (i == 0)
                                            if (tranValArray[0].ToString() != Stack.First.Value)
                                                Stack.RemoveFirst();
                                            else
                                                i++;
                                        if (i >= tranValArray.Length) break;
                                        Stack.AddFirst(tranValArray[i].ToString());
                                    }
                            }
                            sb.AppendFormat("{0}#", currentState);
                            if (Stack.Count == 0)
                                sb.Append("$");
                            else
                                foreach (var stackLetter in Stack)
                                    sb.AppendFormat("{0}", stackLetter);
                            sb.Append('|');
                        }
                    }
                    catch (Exception){}

                    try
                    {
                        var transition =
                            Transitions[
                                new KeyValuePair<string, KeyValuePair<string, string>>(currentState,
                                    new KeyValuePair<string, string>(letter, Stack.First.Value))];
                        currentState = transition.Key;
                        if (transition.Value == "$")
                            Stack.RemoveFirst();
                        else
                        {
                            var tranValArray = transition.Value.ToCharArray().Reverse().ToArray();

                            if (tranValArray.Length > 1)
                                for (int i = 0; i < tranValArray.Length; i++)
                                {
                                    if (i == 0)
                                        if (tranValArray[0].ToString() != Stack.First.Value)
                                            Stack.RemoveFirst();
                                        else
                                            i++;
                                    if (i >= tranValArray.Length) break;
                                    Stack.AddFirst(tranValArray[i].ToString());
                                }
                        }
                        sb.AppendFormat("{0}#", currentState);
                        if (Stack.Count == 0)
                            sb.Append("$");
                        else
                            foreach (var stackLetter in Stack)
                                sb.AppendFormat("{0}", stackLetter);
                        sb.Append('|');
                    }
                    catch (Exception)
                    {
                        sb.Append("fail|");
                        break;
                    }
                }
                if (sb.ToString().EndsWith("fail|") || counter < input.Length) sb.Append("0");
                else 
                {
                    try
                    {
                        while (!AcceptableStates.Contains(currentState))
                        {
                            var transition = Transitions[
                                new KeyValuePair<string, KeyValuePair<string, string>>(currentState,
                                    new KeyValuePair<string, string>("$", Stack.First.Value))];
                            currentState = transition.Key;
                            if (transition.Value == "$")
                                Stack.RemoveFirst();
                            else
                            {
                                var tranValArray = transition.Value.ToCharArray().Reverse().ToArray();

                                if (tranValArray.Length > 1)
                                    for (int i = 0; i < tranValArray.Length; i++)
                                    {
                                        if (i == 0) 
                                            if(tranValArray[0].ToString() != Stack.First.Value)
                                                Stack.RemoveFirst();
                                            else
                                                i++;
                                            
                                        if(i >= tranValArray.Length) break;
                                        Stack.AddFirst(tranValArray[i].ToString());
                                    }
                            }
                            sb.AppendFormat("{0}#", currentState);
                            if (Stack.Count == 0)
                                sb.Append("$");
                            else
                                foreach (var stackLetter in Stack)
                                    sb.AppendFormat("{0}", stackLetter);
                            sb.Append('|');  
                        }
                    }
                    catch{}
                    sb.Append(AcceptableStates.Contains(currentState) ? "1" : "0");
                }
                Console.WriteLine(sb);
            }
        }
    }
}