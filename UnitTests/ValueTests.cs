using FluentAssertions;
using SharpGrad;

namespace UnitTests
{
    public class ValueTests
    {
        [SetUp]
        public void Setup()
        {
        }

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
        }

        [Test]
        public void Subtraction()
        {
            Value x1 = 3.0;
            Value x2 = 1.0;
            Value y = x1 - x2;
            ((double)y).Should().Be(2.0);
        }

        [Test]
        public void Multiplication()
        {
            Value x1 = 3.0;
            Value x2 = 2.0;
            Value y = x1 * x2;
            ((double)y).Should().Be(6.0);
        }

        [Test]
        public void Division()
        {
            Value x1 = 6.0;
            Value x2 = 2.0;
            Value y = x1 / x2;
            ((double)y).Should().Be(3.0);
        }

        [Test]
        public void Negation()
        {
            Value x1 = 3.0;
            Value y = -x1;
            ((double)y).Should().Be(-3.0);
        }

        [Test]
        public void Pow()
        {
            Value x = 3.0;
            Value y = x.Pow(2.0);
            ((double)y).Should().Be(9.0);
        }
    }
}