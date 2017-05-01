using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UTR_2
{
    public class State : IComparable<State>, IEquatable<State>
    {
        public string Name { get; set; }

        public bool IsAcceptable { get; set; }

        public IDictionary<string, State> Transitions { get; private set; }

        public State(string name)
        {
            //Name = name ?? throw new ArgumentNullException(nameof(name), "Name of state cannot be null");
            Name = name;
            IsAcceptable = false;
            Transitions = new SortedDictionary<string, State>();
        }
        


        public void AddTransition(string input, State nextState)
        {
            //if (input == null)
            //{
            //    throw new ArgumentNullException(nameof(input), "Input value cannot be null");
            //}
            //if (nextState == null)
            //{
            //    throw new ArgumentNullException(nameof(nextState), "Set of next states cannot be null");
            //}

            Transitions.Remove(input);
            Transitions.Add(input, nextState);
        }

        public bool RemoveTransition(string input)
        {
            //if (input == null)
            //{
            //    throw new ArgumentNullException(nameof(input), "Input value cannot be null");
            //}
            return Transitions.ContainsKey(input) && Transitions.Remove(input);
        }

        public void RemoveAllTransitions()
        {
            Transitions = new SortedDictionary<string, State>();
        }

        public string TransitionsToString()
        {
            var sb = new StringBuilder();
            foreach (var transition in Transitions)
            {
                sb.AppendFormat("{0},{1}->{2}\n", Name, transition.Key, transition.Value.Name);
                //sb.Append($"{Name},{transition.Key}->{transition.Value.Name}\n");
            }
            return sb.ToString();
        }

        public int CompareTo(State other)
        {
            //if (other == null)
            //{
            //    throw new ArgumentNullException(nameof(other));
            //}
            return this.Name.CompareTo(other.Name);
        }

        public bool Equals(State other)
        {
            //if (other == null)
            //{
            //    throw new ArgumentNullException(nameof(other));
            //}
            return this.Name.Equals(other.Name);
        }

        public override bool Equals(object obj)
        {
            var state = obj as State;
            return state != null && this.Equals(state);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}