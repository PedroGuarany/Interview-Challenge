using SmartVault.Program.QueryLayer;
using System;
using System.Linq;
using Xunit;

namespace SmartVault.Test
{
    public class SolutionTest
    {
        [Fact]
        public void CheckIfReturnedLengthIsBiggerThanZero()
        {
            ///Arrange
            var dal = new FileDAL();

            ///Act
            var totalFileSize = dal.GetTotalFileSize();

            //Assert
            Assert.True(totalFileSize > 0);
        }

        [Fact]
        public void CheckIfAllReturnedDocumentsHaveTheProperty()
        {
            ///Arrange
            var dal = new FileDAL();
            var expectedAccountId = "1";
            var expectedProperty = "Smith Property";

            ///Act
            var documents = dal.GetThirdDocumentsWithProperty(expectedAccountId, expectedProperty);
            var hasAnyDocumentWithoutProperty = documents
                .Any(d => !d.Contains(expectedProperty, StringComparison.InvariantCultureIgnoreCase));

            //Assert
            Assert.True(!hasAnyDocumentWithoutProperty);
        }

        [Fact]
        public void ShouldNotHaveInvalidPropertyText()
        {
            ///Arrange
            var dal = new FileDAL();
            var expectedAccountId = "1";
            var expectedProperty = "Inexisting Propertyyy";

            ///Act
            var documents = dal.GetThirdDocumentsWithProperty(expectedAccountId, expectedProperty);
            var hasAnyDocument = documents.Count > 0;

            //Assert
            Assert.True(!hasAnyDocument);
        }
    }
}
