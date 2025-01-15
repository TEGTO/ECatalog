using FluentValidation.TestHelper;
using ProductApi.Core.Dtos.Endpoints.GetProducts;

namespace ProductApi.Application.Validators.Tests
{
    [TestFixture]
    internal class GetProductsRequestValidatorTests
    {
        private GetProductsRequestValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new GetProductsRequestValidator();
        }

        private static IEnumerable<TestCaseData> ValidationTestCases()
        {
            // Invalid PageNumber
            yield return new TestCaseData(null, null, 0, 10, false, "PageNumber")
                .SetDescription("Page number must be greater than 0.");

            yield return new TestCaseData(null, null, -1, 10, false, "PageNumber")
                .SetDescription("Page number cannot be negative.");

            // Invalid PageSize
            yield return new TestCaseData(null, null, 1, 0, false, "PageSize")
                .SetDescription("Page size must be greater than 0.");

            yield return new TestCaseData(null, null, 1, -5, false, "PageSize")
                .SetDescription("Page size cannot be negative.");

            // Invalid SortBy
            yield return new TestCaseData(null, new string('A', 51), 1, 10, false, "SortBy")
                .SetDescription("SortBy cannot exceed 50 characters.");

            // Valid Request
            yield return new TestCaseData("search", "SortBy", 1, 10, true, null)
                .SetDescription("Valid request with all valid properties.");
        }

        [Test]
        [TestCaseSource(nameof(ValidationTestCases))]
        public void Validate_ValidationTestCases(
            string? search, string? sortBy, int pageNumber, int pageSize, bool isValid, string? invalidPropertyName)
        {
            // Arrange
            var request = new GetProductsRequest
            {
                Search = search,
                SortBy = sortBy,
                PageNumber = pageNumber,
                PageSize = pageSize
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

        [Test]
        public void Validate_Descending_IsOptional()
        {
            // Arrange
            var request = new GetProductsRequest
            {
                Descending = true,
                PageNumber = 1,
                PageSize = 10
            };

            // Act
            var result = validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(r => r.Descending);
        }
    }
}