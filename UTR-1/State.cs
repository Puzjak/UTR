using System;
using System.Collections.Generic;
using System.Linq;

namespace UTR_1
{
    public class State : IComparable<State>, IEquatable<State>
    {
        public string Name { get; set; }

        public bool IsAcceptable { get; set; }

        public IDictionary<string, SortedSet<State>> Transitions { get; private set; }

        public static readonly State DefaultState = new State("#");


        public State(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name), "Name of state cannot be null");
            }
            Name = name;
            IsAcceptable = false;
            Transitions = new SortedDictionary<string, SortedSet<State>>();
        }
        /// <summary>
        /// Every state of machine must be initiated for every input value the machine can take in.
        /// After initialization transition for every input value ends in default state "#"
        /// </summary>
        /// <param name="inputValues">List of input values machine supports</param>
        public void InitializeTransitions(ICollection<string> inputValues)
        {
            if (inputValues == null)
            {
                throw new ArgumentNullException(nameof(inputValues), "List of input values cannot be null");
            }

            foreach (var input in inputValues)
            {
                var set = new SortedSet<State>();
                set.Add(DefaultState);
                Transitions.Add(input, set);
            }
        }
        /// <summary>
        /// Every state of machine must be initiated for every input value the machine can take in.
        /// After initialization transition for every input value ends in default state "#"
        /// </summary>
        /// <param name="inputValues">String array of input values machine supports</param>
        public void InitializeTransitions(string[] inputValues)
        {
            if (inputValues == null)
            {
                throw new ArgumentNullException(nameof(inputValues), "Array of input values cannot be null");
            }

            foreach (var input in inputValues)
            {
                var set = new SortedSet<State>();
                set.Add(DefaultState);
                Transitions.Add(input, set);
            }
        }

        public void AddTransition(string input, SortedSet<State> nextStates)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input), "Input value cannot be null");
            }
            if (nextStates == null)
            {
                throw new ArgumentNullException(nameof(nextStates), "Set of next states cannot be null");
            }

            Transitions.Remove(input);
            Transitions.Add(input, nextStates);
        }

        public bool RemoveTransition(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input), "Input value cannot be null");
            }
            return Transitions.ContainsKey(input) && Transitions.Remove(input);
        }

        public void RemoveAllTransitions()
        {
            Transitions = new SortedDictionary<string, SortedSet<State>>();
        }

        public ISet<State> GetEpsilonTransitionsIterative()
        {
            var transitions = new SortedSet<State>();
            var stateArrayStack = new Stack<State[]>();
            var indexStack = new Stack<int>();
            if (!Transitions.ContainsKey("$")) return transitions;
            var transitionsArray = Transitions["$"].ToArray();
            for (int i = 0; i < transitionsArray.Length; i++)
            {
                if (!transitions.Contains(transitionsArray[i]))
                {
                    transitions.Add(transitionsArray[i]);
                    if (transitionsArray[i].Transitions.ContainsKey("$"))
                    {
                        indexStack.Push(i);
                        stateArrayStack.Push(transitionsArray);
                        transitionsArray = transitionsArray[i].Transitions["$"].ToArray();
                        i = -1;
                    }
                }

                if (i >= transitionsArray.Length - 1 && indexStack.Count > 0)
                {
                    i = indexStack.Pop();
                    transitionsArray = stateArrayStack.Pop();
                }

            }
            return transitions;
        }

        ////NOT WORKING 
        //public ISet<State> GetEpsilonTransitionsRecursive()
        //{
        //    var transitions = new SortedSet<State>();
        //    if(!Transitions.ContainsKey("$")) return transitions;
        //    foreach (var state in Transitions["$"])
        //    {
        //        transitions.Add(state);
        //        transitions.UnionWith(state.GetEpsilonTransitionsRecursive());
        //    }
        //    return transitions;
        //}

        public int CompareTo(State other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            return this.Name.CompareTo(other.Name);
        }

        public bool Equals(State other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            return this.Name.Equals(other.Name);
        }

        public override bool Equals(object obj)
        {
            var state = obj as State;
            return state != null && this.Equals(state);
        }

        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}