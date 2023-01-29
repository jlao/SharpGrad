namespace SharpGrad
{
    public class Value
    {
        private readonly double _value;
        private List<Value> _children = new List<Value>();
        private Action _backward = () => { };
        private double _grad;

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
                // derivative is 1.0. chain rule -> 1.0 * result._grad
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

            result._backward = () =>
            {
                _grad += exp * Math.Pow(_value, exp - 1) * result._grad;
            };

            return result;
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