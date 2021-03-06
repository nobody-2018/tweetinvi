﻿using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Search;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Testinvi.TweetinviControllers.SearchTests
{
    [TestClass]
    public class SearchQueryValidatorTests
    {
        private FakeClassBuilder<SearchQueryValidator> _fakeBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<SearchQueryValidator>();
        }

        #region IsSearchParameterValid
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsSearchParameterValid_IsNull_Throws()
        {
            // Arrange
            var queryValidator = CreateSearchQueryValidator();

            // Act
            queryValidator.ThrowIfSearchParametersIsNotValid(null);
        }

        [TestMethod]
        public void IsSearchParameterValid_SearchQueryIsNull_Succeed()
        {
            // Arrange
            var queryValidator = CreateSearchQueryValidator();
            var searchParameter = A.Fake<ISearchTweetsParameters>();
            searchParameter.CallsTo(x => x.SearchQuery).Returns(null);

            // Act
            queryValidator.ThrowIfSearchParametersIsNotValid(searchParameter);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IsSearchParameterValid_SearchQueryIsEmpty_Throws()
        {
            // Arrange
            var queryValidator = CreateSearchQueryValidator();
            var searchParameter = A.Fake<ISearchTweetsParameters>();
            searchParameter.CallsTo(x => x.SearchQuery).Returns(string.Empty);
            searchParameter.CallsTo(x => x.GeoCode).Returns(null);
            searchParameter.CallsTo(x => x.Filters).Returns(TweetSearchFilters.None);

            // Act
            queryValidator.ThrowIfSearchParametersIsNotValid(searchParameter);
        }

        [TestMethod]
        public void IsSearchParameterValid_SearchQueryIsValid_ReturnsTrue()
        {
            // Arrange
            var queryValidator = CreateSearchQueryValidator();
            var searchParameter = A.Fake<ISearchTweetsParameters>();
            searchParameter.CallsTo(x => x.SearchQuery).Returns(TestHelper.GenerateString());

            // Act
            queryValidator.ThrowIfSearchParametersIsNotValid(searchParameter);
        } 

        #endregion

        #region IsSearchQueryValid

        [TestMethod]
        public void IsSearchQueryValid_SearchQueryIsNull_ReturnsTrue()
        {
            // Arrange
            var queryValidator = CreateSearchQueryValidator();

            // Act
            var result = queryValidator.IsSearchQueryValid(null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsSearchQueryValid_SearchQueryIsEmpty_Throws()
        {
            // Arrange
            var queryValidator = CreateSearchQueryValidator();

            // Act
            var result = queryValidator.IsSearchQueryValid(string.Empty);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsSearchQueryValid_SearchQueryIsValid_ReturnsTrue()
        {
            // Arrange
            var queryValidator = CreateSearchQueryValidator();

            // Act
            var result = queryValidator.IsSearchQueryValid(TestHelper.GenerateString());

            // Assert
            Assert.IsTrue(result);
        } 

        #endregion

        #region IsGeoCodeValid

        [TestMethod]
        public void IsGeoCodeValid_IsNull_False()
        {
            // Arrange
            var queryValidator = CreateSearchQueryValidator();

            // Act
            var result = queryValidator.IsGeoCodeValid(null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsGeoCodeValid_IsNotNull_True()
        {
            // Arrange
            var queryValidator = CreateSearchQueryValidator();
            var geoCode = A.Fake<IGeoCode>();

            // Act
            var result = queryValidator.IsGeoCodeValid(geoCode);

            // Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region IsLocaleParameterValid

        [TestMethod]
        public void IsLocaleParameterValid_SearchQueryIsNull_False()
        {
            // Arrange
            var queryValidator = CreateSearchQueryValidator();

            // Act
            var result = queryValidator.IsLocaleParameterValid(null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsLocaleParameterValid_SearchQueryIsEmpty_False()
        {
            // Arrange
            var queryValidator = CreateSearchQueryValidator();

            // Act
            var result = queryValidator.IsLocaleParameterValid(string.Empty);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsLocaleParameterValid_SearchQueryIsValid_True()
        {
            // Arrange
            var queryValidator = CreateSearchQueryValidator();

            // Act
            var result = queryValidator.IsLocaleParameterValid(TestHelper.GenerateString());

            // Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region IsLanguageDefined

        [TestMethod]
        public void IsLangDefined_LanguageIsUndefined_False()
        {
            // Arrange
            var queryValidator = CreateSearchQueryValidator();

            // Act
            var result = queryValidator.IsLangDefined(Language.Undefined);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsLangDefined_LanguageIsDifferentFromUndefined_True()
        {
            // Arrange
            var queryValidator = CreateSearchQueryValidator();

            // Act
            var result = true;
            for (int i = 1; i < 218 && result; ++i)
            {
                var language = (Language) i;
                result = queryValidator.IsLangDefined(language);
            }

            // Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region IsUntilDefined

        [TestMethod]
        public void IsUntilDefined_UntilIsDefault_False()
        {
            // Arrange
            var queryValidator = CreateSearchQueryValidator();

            // Act
            var result = queryValidator.IsDateTimeDefined(default(DateTime));

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsUntilDefined_UntilIsRandom_True()
        {
            // Arrange
            var queryValidator = CreateSearchQueryValidator();

            // Act
            var result = queryValidator.IsDateTimeDefined(DateTime.Now - new TimeSpan(new Random().Next()));

            // Assert
            Assert.IsTrue(result);
        }

        #endregion

        public SearchQueryValidator CreateSearchQueryValidator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}
