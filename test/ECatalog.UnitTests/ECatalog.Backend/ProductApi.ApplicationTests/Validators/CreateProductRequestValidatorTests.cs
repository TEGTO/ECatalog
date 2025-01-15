using FluentValidation.TestHelper;
using ProductApi.Core.Dtos.Endpoints.CreateProduct;

namespace ProductApi.Application.Validators.Tests
{
    [TestFixture]
    internal class CreateProductRequestValidatorTests
    {
        private CreateProductRequestValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new CreateProductRequestValidator();
        }

        private static IEnumerable<TestCaseData> ValidationTestCases()
        {
            yield return new TestCaseData(null, "Valid description", 10.00m, false, "Name")
                .SetDescription("Name cannot be null.");

            yield return new TestCaseData(string.Empty, "Valid description", 10.00m, false, "Name")
                .SetDescription("Name cannot be empty.");

            yield return new TestCaseData("A", "Valid description", 10.00m, false, "Name")
                .SetDescription("Name must have at least 3 characters.");

            yield return new TestCaseData(new string('A', 101), "Valid description", 10.00m, false, "Name")
                .SetDescription("Name cannot exceed 100 characters.");

            yield return new TestCaseData("Valid Name", new string('A', 501), 10.00m, false, "Description")
                .SetDescription("Description cannot exceed 500 characters.");

            yield return new TestCaseData("Valid Name", "Valid description", -1.00m, false, "Price")
                .SetDescription("Price must be greater than 0.");

            yield return new TestCaseData("Valid Name", "Valid description", 10.123m, false, "Price")
                .SetDescription("Price must have up to two decimal places.");

            yield return new TestCaseData("Valid Name", "Valid description", 10.00m, true, null)
                .SetDescription("Request is valid.");
        }

        [Test]
        [TestCaseSource(nameof(ValidationTestCases))]
        public void Validate_ValidationTestCases(string name, string description, decimal price, bool isValid, string? invalidPropertyName)
        {
            // Arrange
            var request = new CreateProductRequest { Name = name, Description = description, Price = price };

            // Act
            var result = validator.TestValidate(request);

            // Assert
            if (isValid)
            {
                result.ShouldNotHaveAnyValidationErrors();
            }
            else
            {
                result.ShouldHaveValidationErrorFor(invalidPropertyName);
            }
        }
    }
}