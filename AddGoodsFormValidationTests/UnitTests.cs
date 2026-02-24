using Goods.Windows;

namespace AddGoodsFormValidationTests
{
    [TestFixture]
    public class Tests
    {
        private FormValidation formValidation;

        [SetUp]
        public void Setup()
        {
            formValidation = new FormValidation();
        }

        // 1 — все пустое
        [Test]
        public void EmptyAllFields_ReturnsFalse()
        {
            var result = formValidation.Validate("", "", "", "");
            Assert.Equals(result, false);
        }

        // 2 — корректные данные
        [Test]
        public void ValidData_ReturnsTrue()
        {
            var result = formValidation.Validate("Молоко", "120.50", "10", "25");
            Assert.Equals(result, true);
        }

        // 3 — пустое имя
        [Test]
        public void EmptyName_ReturnsFalse()
        {
            var result = formValidation.Validate("", "100", "5", "10");
            Assert.Equals(result, false);
        }
    }
}
