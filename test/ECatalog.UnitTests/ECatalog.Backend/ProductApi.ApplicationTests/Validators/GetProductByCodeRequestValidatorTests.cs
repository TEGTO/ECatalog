using FluentValidation.TestHelper;
using ProductApi.Core.Dtos.Endpoints.GetProductByCode;

namespace ProductApi.Application.Validators.Tests
{
    [TestFixture]
    internal class GetProductByCodeRequestValidatorTests
    {
        private GetProductByCodeRequestValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new GetProductByCodeRequestValidator();
        }

        private static IEnumerable<TestCaseData> ValidationTestCases()
        {
            yield return new TestCaseData(null, false, "Code")
                .SetDescription("Code cannot be null.");

            yield return new TestCaseData(string.Empty, false, "Code")
                .SetDescription("Code cannot be empty.");

            yield return new TestCaseData("1234-567", false, "Code")
                .SetDescription("Code must follow the format {XXXX-XXXX}, where X is a digit (missing a digit).");

            yield return new TestCaseData("12345-6789", false, "Code")
                .SetDescription("Code must follow the format {XXXX-XXXX}, where X is a digit (too many digits).");

            yield return new TestCaseData("1234-567A", false, "Code")
                .SetDescription("Code must follow the format {XXXX-XXXX}, where X is a digit (contains a non-digit character).");

            yield return new TestCaseData("abcd-efgh", false, "Code")
                .SetDescription("Code must follow the format {XXXX-XXXX}, where X is a digit (letters instead of digits).");

            yield return new TestCaseData("1234-5678", true, null)
                .SetDescription("Valid code.");
        }

        [Test]
        [TestCaseSource(nameof(ValidationTestCases))]
        public void Validate_ValidationTestCases(string code, bool isValid, string? invalidPropertyName)
        {
            // Arrange
            var request = new GetProductByCodeRequest { Code = code };

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