using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UTR_1
{
    public class SimEnka
    {
        private static IDictionary<string, State> _states = new SortedDictionary<string, State>();
        private static State _initialState;
        private static ISet<State> _currentStates;
        private static ISet<State> _nextStates;
        private static IList<string> _inputValues = new List<string>();
        private static ISet<string> _possibleInputs = new SortedSet<string>();

        public static void Main(string[] args)
        {
            _states.Add(State.DefaultState.Name, State.DefaultState);

            //Reading input values
            var line = Console.ReadLine();
            line = line.Replace(" ", "");
            foreach (var input in line.Split('|'))
            {
                _inputValues.Add(input);
            }

            //Reading states
            line = Console.ReadLine();
            line = line.Replace(" ", "");
            var tmp = line.Split(',');
            foreach (var state in tmp)
            {
                _states.Add(state, new State(state));
            }

            //Reading all possible inputs
            line = Console.ReadLine();
            line = line.Replace(" ", "");
            foreach (var input in line.Split(','))
            {
                _possibleInputs.Add(input);
            }

            //Initializing transitions for every input to "# state"
            foreach (var state in _states)
            {
                state.Value.InitializeTransitions(_possibleInputs);
            }

            //Reading acceptable states
            line = Console.ReadLine();
            line = line.Replace(" ", "");
            foreach (var state in line.Split(','))
            {
                _states[state].IsAcceptable = true;
            }

            //Reading initial state
            line = Console.ReadLine();
            line = line.Replace(" ", "");
            _initialState = _states[line];

            //Reading transitions
            while (true)
            {
                line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) break;
                tmp = line.Split(new[] { "->" }, StringSplitOptions.None);
                var initialState = tmp[0].Split(',')[0];
                var input = tmp[0].Split(',')[1];
                var nextStatesString = tmp[1].Split(',');
                var nextStates = new SortedSet<State>();
                foreach (var state in nextStatesString)
                {
                    nextStates.Add(_states[state]);
                }
                _states[initialState].AddTransition(input, nextStates);
            }

            //Solution
            foreach (var inputValue in _inputValues)
            {
                var result = new StringBuilder();
                result.Append(_initialState + "|");
                _currentStates = new SortedSet<State> { _initialState };
                _nextStates = new SortedSet<State>();
                var eStates = new SortedSet<State>();
                if (_initialState.Transitions.ContainsKey("$"))
                {
                    _nextStates.UnionWith(_initialState.Transitions["$"]);
                }
                foreach (var input in inputValue.Split(','))
                {
                    foreach (var state in _currentStates)
                    {
                        _nextStates.UnionWith(state.Transitions[input]);
                    }

                    foreach (var state in _nextStates)
                    {
                        if (state.Transitions.ContainsKey("$"))
                        {
                            eStates.UnionWith(state.GetEpsilonTransitionsIterative());
                        }
                    }
                    _nextStates.UnionWith(eStates);

                    if (_nextStates.Count() > 1 && _nextStates.Contains(State.DefaultState))
                    {
                        _nextStates.Remove(State.DefaultState);
                    }

                    foreach (var state in _nextStates)
                    {
                        result.Append(state + ",");
                    }
                    result.Remove(result.Length - 1, 1);
                    result.Append("|");

                    eStates = new SortedSet<State>();
                    _currentStates = new SortedSet<State>();
                    _currentStates.UnionWith(_nextStates);
                    _nextStates = new SortedSet<State>();

                }
                result.Remove(result.Length - 1, 1);
                Console.WriteLine(result);
            }
        }
    }
}
