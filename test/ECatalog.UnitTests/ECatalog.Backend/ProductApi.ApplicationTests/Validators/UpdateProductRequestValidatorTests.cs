using FluentValidation.TestHelper;
using ProductApi.Core.Dtos.Endpoints.UpdateProduct;

namespace ProductApi.Application.Validators.Tests
{
    [TestFixture]
    internal class UpdateProductRequestValidatorTests
    {
        private UpdateProductRequestValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new UpdateProductRequestValidator();
        }

        private static IEnumerable<TestCaseData> ValidationTestCases()
        {
            // Code field
            yield return new TestCaseData(null, "Valid Name", "Valid Description", 10m, false, "Code")
                .SetDescription("Code cannot be null.");

            yield return new TestCaseData(string.Empty, "Valid Name", "Valid Description", 10m, false, "Code")
                .SetDescription("Code cannot be empty.");

            yield return new TestCaseData("1234-567", "Valid Name", "Valid Description", 10m, false, "Code")
                .SetDescription("Code must follow the format {XXXX-XXXX}, where X is a digit (missing a digit).");

            yield return new TestCaseData("1234-5678", null, "Valid Description", 10m, false, "Name")
                .SetDescription("Name cannot be null.");

            yield return new TestCaseData("1234-5678", string.Empty, "Valid Description", 10m, false, "Name")
                .SetDescription("Name cannot be empty.");

            yield return new TestCaseData("1234-5678", "No", "Valid Description", 10m, false, "Name")
                .SetDescription("Name must be between 3 and 100 characters (too short).");

            yield return new TestCaseData("1234-5678", new string('A', 101), "Valid Description", 10m, false, "Name")
                .SetDescription("Name must be between 3 and 100 characters (too long).");

            yield return new TestCaseData("1234-5678", "Valid Name", new string('B', 501), 10m, false, "Description")
                .SetDescription("Description cannot exceed 500 characters.");

            yield return new TestCaseData("1234-5678", "Valid Name", "Valid Description", -1m, false, "Price")
                .SetDescription("Price must be greater than 0.");

            yield return new TestCaseData("1234-5678", "Valid Name", "Valid Description", 10.123m, false, "Price")
                .SetDescription("Price must have up to two decimal places (too many decimals).");

            yield return new TestCaseData("1234-5678", "Valid Name", "Valid Description", 10m, true, null)
                .SetDescription("Valid request.");
        }

        [Test]
        [TestCaseSource(nameof(ValidationTestCases))]
        public void Validate_ValidationTestCases(string code, string name, string description, decimal price, bool isValid, string? invalidPropertyName)
        {
            // Arrange
            var request = new UpdateProductRequest
            {
                Code = code,
                Name = name,
                Description = description,
                Price = price
            };

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