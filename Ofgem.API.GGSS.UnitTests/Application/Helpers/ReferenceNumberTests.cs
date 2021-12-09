using FluentAssertions;
using Ofgem.API.GGSS.Application.Helpers;
using System;
using Xunit;

namespace Ofgem.API.GGSS.UnitTests.Application.Helpers
{
    public class ReferenceNumberTests
    {
        [Fact]
        public void CanGetApplicationReferenceWhenExistingReferenceIsNull()
        {
            // Arrange 
            var applicationGuid = Guid.Parse("4411bdfc-76db-4ddb-9a2c-a675a83c623b");
            string reference = null;

            // Act
            var result = ReferenceNumber.GetApplicationReference(applicationGuid, reference);

            // Assert
            result.Should().Be("GGSS-4411B");
        }

        [Fact]
        public void CanGetApplicationReferenceWhenExistingReferenceLengthIsGuidLength()
        {
            // Arrange 
            var applicationGuid = Guid.Parse("4411bdfc-76db-4ddb-9a2c-a675a83c623b");
            string reference = applicationGuid.ToString();

            // Act
            var result = ReferenceNumber.GetApplicationReference(applicationGuid, reference);

            // Assert
            result.Should().Be("GGSS-4411B");
        }

        [Fact]
        public void CanGetApplicationReferenceWhenExistingReferenceFormatIsCorrect()
        {
            // Arrange 
            var applicationGuid = Guid.Parse("4411bdfc-76db-4ddb-9a2c-a675a83c623b");
            string reference = "GGSS-4411B";

            // Act
            var result = ReferenceNumber.GetApplicationReference(applicationGuid, reference);

            // Assert
            result.Should().Be("GGSS-4411B");
        }

    }

}
