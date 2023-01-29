namespace SharpGrad
{
    public class Value
    {
        private readonly double _value;
        private List<Value> _children = new List<Value>();
        private Action _backward = () => { };
        private double _grad = 0;

        public Value(double value)
        {
            _value = value;
        }

        public double Gradient => _grad;

        public static Value operator +(Value left, Value right)
        {
            var result = new Value(left._value + right._value);
            result._children.Add(left);
            result._children.Add(right);

            result._backward = () =>
            {
                // y = x + b
                // dy/dx = 1
                left._grad += result._grad;
                right._grad += result._grad;
            };

            return result;
        }

        public static Value operator -(Value left, Value right)
        {
            return left + (-right);
        }

        public static Value operator *(Value left, Value right)
        {
            var result = new Value(left._value * right._value);
            result._children.Add(left);
            result._children.Add(right);

            result._backward = () =>
            {
                // y = x * b
                // dy/dx = b
                left._grad += right._value * result._grad;
                right._grad += left._value * result._grad;
            };

            return result;
        }

        public static Value operator /(Value left, Value right)
        {
            return left * right.Pow(-1);
        }

        public static Value operator -(Value v)
        {
            return v * -1.0;
        }

        public Value Pow(double exp)
        {
            var result = new Value(Math.Pow(_value, exp));
            result._children.Add(this);

            // y = x^b
            // dy/dx = b * x^(b-1)
            result._backward = () =>
            {
                _grad += exp * Math.Pow(_value, exp - 1) * result._grad;
            };

            return result;
        }

        public void Backward()
        {
            List<Value> topo = TopologicalSort();

            _grad = 1;
            for (int i = topo.Count-1; i >= 0; i--)
            {
                topo[i]._backward();
            }
        }

        public List<Value> TopologicalSort()
        {
            var result = new List<Value>();
            var visited = new HashSet<Value>();
            TopologicalSortVisit(this, result, visited);
            return result;
        }

        static void TopologicalSortVisit(Value v, List<Value> result, HashSet<Value> visited)
        {
            if (visited.Contains(v))
            {
                return;
            }

            visited.Add(v);

            foreach (Value child in v._children)
            {
                TopologicalSortVisit(child, result, visited);
            }

            result.Add(v);
        }

        public override string ToString()
        {
            return $"{_value}";
        }

        public static implicit operator double(Value v)
        {
            return v._value;
        }

        public static implicit operator Value(double d)
        {
            return new Value(d);
        }
    }
}