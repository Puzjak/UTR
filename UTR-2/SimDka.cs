using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UTR_2
{
    class SimDka
    {
        private static ISet<State> _states = new SortedSet<State>();
        private static ISet<string> _inputValues = new SortedSet<string>();
        private static State _initialState;

        static void Main(string[] args)
        {
            string line;

            //Reading all states
            line = Console.ReadLine();
            foreach (var state in line.Split(','))
            {
                _states.Add(new State(state.Trim()));
            }

            //Reading all input signs
            line = Console.ReadLine();
            foreach (var input in line.Split(','))
            {
                _inputValues.Add(input.Trim());
            }

            //Reading all acceptable states
            line = Console.ReadLine();
            foreach (var acceptableState in line.Split(','))
            {
                if(!string.IsNullOrWhiteSpace(acceptableState))
                _states.First(s => s.Name == acceptableState).IsAcceptable = true;
            }

            //Reading initial state
            var tmp = Console.ReadLine().Trim();
            _initialState = _states.First(s => s.Name == tmp);

            //Reading all transitions
            while (true)
            {
                line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) break;
                var currentState = line.Split(',')[0];
                var sign = line.Split(',')[1].Split(new[] {"->"}, StringSplitOptions.None)[0];
                var nextState = line.Split(',')[1].Split(new[] { "->" }, StringSplitOptions.None)[1];
                _states.First(s => s.Name == currentState).AddTransition(sign, 
                    _states.First(s => s.Name == nextState));
            }

            //Solution
            //Finding reachable states
            var reachableStatesTmp = new ArrayList();
            reachableStatesTmp.Add(_initialState);
            for (int i = 0; i < reachableStatesTmp.Count; i++)
            {
                foreach (var transition in ((State)reachableStatesTmp[i]).Transitions)
                {
                    if (!reachableStatesTmp.Contains(transition.Value))
                    {
                        reachableStatesTmp.Add(transition.Value);
                    } 
                }
            }
            _states.IntersectWith(reachableStatesTmp.Cast<State>());

            //Finding same states
            var groups = new ArrayList();
            groups.Add(new SortedSet<State>());
            groups.Add(new SortedSet<State>());

            foreach (var state in _states.Where(s => s.IsAcceptable))
            {
                ((SortedSet<State>) groups[0]).Add(state);
            }

            foreach (var state in _states.Where(s => !s.IsAcceptable))
            {
                ((SortedSet<State>)groups[1]).Add(state);
            }

            for (int i = 0; i < groups.Count; i++)
            {
                var groupArray = ((SortedSet<State>) groups[i]).ToArray();
                var first = true;
                var newGroup = new SortedSet<State>();
                for (int k = 1; k < groupArray.Length; k++)
                {
                    foreach (var inputValue in _inputValues)
                    {
                        var flag = false;
                        for (int j = 0; j < groups.Count; j++)
                        {
                            if (((SortedSet<State>) groups[j]).Contains(groupArray[0].Transitions[inputValue]) &&
                                ((SortedSet<State>) groups[j]).Contains(groupArray[k].Transitions[inputValue]))
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            if (first)
                            {
                                groups.Add(newGroup);
                                first = false;
                            }
                            ((SortedSet<State>)groups[i]).Remove(groupArray[k]);
                            newGroup.Add(groupArray[k]);
                            break;
                        }
                    }
                }
            }
            foreach (var group in groups)
            {
                var groupArray = ((SortedSet<State>) group).ToArray();
                for (var i = 1; i < groupArray.Length; i++)
                {
                    foreach (var state in _states)
                    {
                        var keysToBeRemoved = new List<string>();
                        foreach (var transition in state.Transitions)
                        {
                            if (transition.Value == groupArray[i])
                            {
                                keysToBeRemoved.Add(transition.Key);
                            }
                                
                        }
                        foreach (var key in keysToBeRemoved)
                        {
                            state.Transitions.Remove(key);
                            state.Transitions.Add(key, groupArray[0]);
                        }
                    }
                    _states.Remove(groupArray[i]);
                }
                if (groupArray.Contains(_initialState))
                    _initialState = groupArray[0];
            }

            //Writing solution
            var sb = new StringBuilder();

            foreach (var state in _states)
            {
                sb.AppendFormat("{0},", state);
                //sb.Append($"{state},");
            }
            if(_states.Any())
            sb.Remove(sb.Length - 1, 1);
            sb.Append("\n");

            foreach (var inputValue in _inputValues)
            {
                sb.AppendFormat("{0},", inputValue);
                //sb.Append($"{inputValue},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("\n");


            foreach (var state in _states.Where(s => s.IsAcceptable))
            {
                sb.AppendFormat("{0},", state);
                //sb.Append($"{state},");
            }
            if(_states.Any(s => s.IsAcceptable))
            sb.Remove(sb.Length - 1, 1);
            sb.Append("\n");
            sb.AppendFormat("{0}\n", _initialState);
            //sb.Append($"{_initialState}\n");

            foreach (var state in _states)
            {
                sb.Append(state.TransitionsToString());
            }
            Console.WriteLine(sb);
        }
    }
}
