namespace Model.UnitTests
{
    [TestFixture]
    public class MalletParametersTests
    {
        private MalletParameters _malletParameters;

        [SetUp]
        public void InitializeMalletParameters()
        {
            _malletParameters = new MalletParameters();
        }

        [Test(Description = "Позитивный тест конструктора без параметров")]
        public void TestMalletParameters()
        {
            // Assert.
            for (var i = 0; i < 100; ++i)
            {
                Assert.DoesNotThrow(() => new MalletParameters());
            }
        }

        [Test(Description = "Позитивный тест на возвращение корректной формы бойка")]
        public void TestHeadType_Get_CorrectValue()
        {
            // Setup.
            _malletParameters.HeadType = HeadType.Cylinder;
            var expected = HeadType.Cylinder;

            // Act.
            var actual = _malletParameters.HeadType;

            // Assert.
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test(Description = "Позитивный тест на возвращение корректного диаметра бойка")]
        public void TestHeadDiameter_Get_CorrectValue()
        {
            // Setup.
            _malletParameters.HeadDiameter = 65;
            double expected = 65;

            // Act.
            var actual = _malletParameters.HeadDiameter;

            // Assert.
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test(
            Description =
                "Негативный тест на присвоение некорректного диаметра бойка, ожидается выброс исключения")]
        public void TestHeadDiameter_SetIncorrectValue_ThrowException()
        {
            // Assert.
            Assert.Throws<Exception>(() => _malletParameters.HeadDiameter = 500);
        }

        [Test(Description = "Позитивный тест на возвращение корректной ширины бойка")]
        public void TestHeadWidth_Get_CorrectValue()
        {
            // Setup.
            _malletParameters.HeadWidth = 75;
            double expected = 75;

            // Act.
            var actual = _malletParameters.HeadWidth;

            // Assert.
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test(
            Description =
                "Негативный тест на присвоение некорректной ширины бойка, ожидается выброс исключения")]
        public void TestHeadWidth_SetIncorrectValue_ThrowException()
        {
            // Assert.
            Assert.Throws<Exception>(() => _malletParameters.HeadWidth = 0);
        }

        [Test(Description = "Позитивный тест на возвращение корректной высоты бойка")]
        public void TestHeadHeight_Get_CorrectValue()
        {
            // Setup.
            _malletParameters.HeadHeight = 100;
            double expected = 100;

            // Act.
            var actual = _malletParameters.HeadHeight;

            // Assert.
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test(
            Description =
                "Негативный тест на присвоение некорректной высоты бойка, ожидается выброс исключения")]
        public void TestHeadHeight_SetIncorrectValue_ThrowException()
        {
            // Assert.
            Assert.Throws<Exception>(() => _malletParameters.HeadHeight = 1000);
        }

        [Test(Description = "Позитивный тест на возвращение корректной длины бойка")]
        public void TestHeadLength_Get_CorrectValue()
        {
            // Setup.
            _malletParameters.HeadLength = 150;
            double expected = 150;

            // Act.
            var actual = _malletParameters.HeadLength;

            // Assert.
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test(
            Description =
                "Негативный тест на присвоение некорректной длины бойка, ожидается выброс исключения")]
        public void TestHeadLength_SetIncorrectValue_ThrowException()
        {
            // Assert.
            Assert.Throws<Exception>(() => _malletParameters.HeadHeight = -75);
        }

        [Test(Description = "Позитивный тест на возвращение корректной высоты рукоятки")]
        public void TestHandleHeight_Get_CorrectValue()
        {
            // Setup.
            _malletParameters.HandleHeight = 125;
            double expected = 125;

            // Act.
            var actual = _malletParameters.HandleHeight;

            // Assert.
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test(
            Description =
                "Негативный тест на присвоение некорректной высоты рукоятки, ожидается выброс исключения")]
        public void TestHandleHeight_SetIncorrectValue_ThrowException()
        {
            // Assert.
            Assert.Throws<Exception>(() => _malletParameters.HandleHeight = -75);
        }

        [Test(Description = "Позитивный тест на возвращение корректного диаметра рукоятки")]
        public void TestHandleDiameter_Get_CorrectValue()
        {
            // Setup.
            _malletParameters.HandleDiameter = 30;
            double expected = 30;

            // Act.
            var actual = _malletParameters.HandleDiameter;

            // Assert.
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test(
            Description =
                "Негативный тест на присвоение некорректного диаметра рукоятки, ожидается выброс исключения")]
        public void TestHandleDiameter_SetIncorrectValue_ThrowException()
        {
            // Assert.
            Assert.Throws<Exception>(() => _malletParameters.HandleDiameter = 0);
        }

        [Test(
            Description =
                "Негативный тест ширина бойка должна быть больше диаметра рукоятки, ожидается выброс исключения")]
        public void TestHeadWidth_LessThanHandleDiameter_ThrowException()
        {
            // Setup.
            _malletParameters.HandleDiameter = 70;

            // Assert.
            Assert.Throws<Exception>(
                () => _malletParameters.HeadWidth = 60,
                "К ширине бойка было присвоено значение меньше диаметра рукоятки.");
        }

        [Test(
            Description =
                "Негативный тест заданная длина бойка должна быть больше его ширины, ожидается выброс исключения")]
        public void TestHeadLength_LessThanHandleWidth_ThrowException()
        {
            // Setup.
            _malletParameters.HeadWidth = 100;

            // Assert.
            Assert.Throws<Exception>(
                () => _malletParameters.HeadLength = 80,
                "К длине бойка было присвоено значение меньшее чем его ширина");
        }
    }
}