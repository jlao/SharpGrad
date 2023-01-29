using FluentAssertions;
using SharpGrad;

namespace UnitTests
{
    public class ValueTests
    {
        [Test]
        public void Casting()
        {
            Value x = 2.0;
            ((double)x).Should().Be(2.0);
        }

        [Test]
        public void Addition()
        {
            Value x1 = 1.0;
            Value x2 = 2.0;
            Value y = x1 + x2;
            ((double)y).Should().Be(3.0);

            y.Backward();
            y.Gradient.Should().Be(1.0);
            x1.Gradient.Should().Be(1.0);
            x2.Gradient.Should().Be(1.0);
        }

        [Test]
        public void AddSelf()
        {
            Value x = 2.0;
            Value y = x + x;
            ((double)y).Should().Be(4.0);

            y.Backward();
            y.Gradient.Should().Be(1.0);

            // Make sure gradient accumulates
            x.Gradient.Should().Be(2.0);
        }

        [Test]
        public void Subtraction()
        {
            Value x1 = 3.0;
            Value x2 = 1.0;
            Value y = x1 - x2;
            ((double)y).Should().Be(2.0);

            y.Backward();
            y.Gradient.Should().Be(1.0);
            x1.Gradient.Should().Be(1.0);
            x2.Gradient.Should().Be(-1.0);
        }

        [Test]
        public void Multiplication()
        {
            Value x1 = 3.0;
            Value x2 = 2.0;
            Value y = x1 * x2;
            ((double)y).Should().Be(6.0);

            y.Backward();
            y.Gradient.Should().Be(1.0);
            x1.Gradient.Should().Be(2.0);
            x2.Gradient.Should().Be(3.0);
        }

        [Test]
        public void Division()
        {
            Value x1 = 6.0;
            Value x2 = 2.0;
            Value y = x1 / x2;
            ((double)y).Should().Be(3.0);

            y.Backward();
            y.Gradient.Should().Be(1.0);
            x1.Gradient.Should().Be(1 / 2.0);

            // y = a / b = a * b^-1
            // dy/db = -a/(b^2)
            x2.Gradient.Should().Be(-6.0 / 4.0);
        }

        [Test]
        public void Negation()
        {
            Value x1 = 3.0;
            Value y = -x1;
            ((double)y).Should().Be(-3.0);

            y.Backward();
            y.Gradient.Should().Be(1.0);
            x1.Gradient.Should().Be(-1.0);
        }

        [Test]
        public void Pow()
        {
            Value x = 3.0;
            Value y = x.Pow(2.0);
            ((double)y).Should().Be(9.0);

            y.Backward();
            y.Gradient.Should().Be(1.0);
            x.Gradient.Should().Be(2.0 * 3.0);
        }

        [Test]
        public void TopologicalSort1()
        {
            Value x1 = 2.0;
            Value x2 = 3.0;
            Value y = x1 + x2;

            List<Value> topo = y.TopologicalSort();

            topo.Should().Equal(x1, x2, y);
        }

        [Test]
        public void TopologicalSort2()
        {
            Value x1 = 2.0;
            Value x2 = 3.0;
            Value x3 = x1 + x2;
            Value y = x3.Pow(2.0);

            y.TopologicalSort().Should().Equal(x1, x2, x3, y);
        }
    }
}