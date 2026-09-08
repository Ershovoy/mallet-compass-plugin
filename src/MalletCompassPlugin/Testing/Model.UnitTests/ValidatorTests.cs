namespace Model.UnitTests
{
    [TestFixture]
    public class ValidatorTests
    {
        [TestCase(
            100,
            -100,
            100,
            TestName = "Проверка вхождения максимального числа в заданный диапазон")]
        [TestCase(
            -100,
            -100,
            100,
            TestName = "Проверка вхождения минимального числа в заданный диапазон")]
        [TestCase(0, -100, 100, TestName = "Проверка вхождения нуля в заданный диапазон")]
        [TestCase(
            100,
            100,
            100,
            TestName = "Проверка вхождения максимального и минимального числа в заданный диапазон")]
        public void TestIsValueInRange_ValueInRange_ResultEqual(
            double value,
            double min,
            double max)
        {
            // Setup
            var expected = true;

            // Act
            var actual = Validator.IsValueInRange(value, min, max);

            // Assert
            Assert.That(
                actual,
                Is.EqualTo(expected),
                "Число не входит в заданный диапазон, хотя должно");
        }

        [TestCase(
            101,
            -100,
            100,
            TestName = "Проверка выхода заданного числа за максимальный диапазон")]
        [TestCase(
            -100.01,
            -100,
            100,
            TestName = "Проверка выхода заданного числа за минимальный диапазон")]
        public void TestIsValueInRange_ValueNotInRange_ResultEqual(
            double value,
            double min,
            double max)
        {
            // Setup
            var expected = false;

            // Act
            var actual = Validator.IsValueInRange(value, min, max);

            // Assert
            Assert.That(
                actual,
                Is.EqualTo(expected),
                "Число входит в заданный диапазон, хотя не должно");
        }
    }
}