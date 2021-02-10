// <copyright file="CardHelperTest.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.Helpers
{
    using System;
    using System.IO;
    using AdaptiveCards;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Apps.SearchCoach.Helpers;
    using Microsoft.Teams.Apps.SearchCoach.ModelMappers;
    using Microsoft.Teams.Apps.SearchCoach.Models.Configuration;
    using Microsoft.Teams.Apps.SearchCoach.Tests.TestData;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Class that contains test methods for card helper.
    /// </summary>
    [TestClass]
    public class CardHelperTest
    {
        private Mock<ILogger<CardHelper>> logger;
        private IOptions<BotSettings> botOptions;
        private Mock<IMemoryCache> memoryCache;
        private Mock<IWebHostEnvironment> hostingEnvironment;
        private CardHelper cardHelper;
        private Mock<IQuestionAnswersMapper> questionAnswersMapper;

        /// <summary>
        /// Initialize all test variables.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.logger = new Mock<ILogger<CardHelper>>();
            this.memoryCache = new Mock<IMemoryCache>();
            this.hostingEnvironment = new Mock<IWebHostEnvironment>();
            this.botOptions = ConfigurationData.BotOptions;
            this.questionAnswersMapper = new Mock<IQuestionAnswersMapper>();

            this.cardHelper = new CardHelper(
                this.logger.Object,
                this.memoryCache.Object,
                this.hostingEnvironment.Object,
                this.botOptions,
                this.questionAnswersMapper.Object);
        }

        /// <summary>
        ///  Test case to check if questions card is not null and have valid contents.
        /// </summary>
        [TestMethod]
        public void QuestionsCardNotNullValidContent()
        {
            // ARRANGE
            this.hostingEnvironment
                .Setup(m => m.ContentRootPath)
                .Returns(".");

            this.memoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);

            var cardTemplate = File.ReadAllText(CardHelpersData.QuestionListCardFilePath);
            var expectedCardJson = Newtonsoft.Json.JsonConvert.SerializeObject(AdaptiveCard.FromJson(cardTemplate).Card);

            // ACT
            var actualAttachmentCardResult = this.cardHelper.GetQuestionsCard(QuestionAnswersHelperData.QuestionAnswers, isQuestionAlreadySent: false, string.Empty);
            var actualCardJson = Newtonsoft.Json.JsonConvert.SerializeObject(actualAttachmentCardResult.Content);

            // ASSERT
            Assert.IsNotNull(actualAttachmentCardResult);
            Assert.AreEqual(expectedCardJson, actualCardJson);
        }

        /// <summary>
        ///  Test case to check if questions card is not null and contains valid HTML encoded data.
        /// </summary>
        [TestMethod]
        public void QuestionsCardNotNullValidHtmlContent()
        {
            // ARRANGE
            this.hostingEnvironment
                .Setup(m => m.ContentRootPath)
                .Returns(".");

            this.memoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);

            var cardTemplate = File.ReadAllText(CardHelpersData.HtmlEncodedQuestionListCardFilePath);
            var expectedCardJson = Newtonsoft.Json.JsonConvert.SerializeObject(AdaptiveCard.FromJson(cardTemplate).Card);

            // ACT
            var actualAttachmentCardResult = this.cardHelper.GetQuestionsCard(QuestionAnswersHelperData.QuestionAnswersWithHtmlContent, isQuestionAlreadySent: false, string.Empty);
            var actualCardJson = Newtonsoft.Json.JsonConvert.SerializeObject(actualAttachmentCardResult.Content);

            // ASSERT
            Assert.IsNotNull(actualAttachmentCardResult);
            Assert.AreEqual(expectedCardJson, actualCardJson);
        }

        /// <summary>
        ///  Test case to throw exception while passing null data to construct questions card.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetQuestionsCardArgumentNullException()
        {
            // ACT
            this.cardHelper.GetQuestionsCard(null, isQuestionAlreadySent: false, string.Empty);
        }

        /// <summary>
        ///  Test case to check if personal scope welcome card is not null and have valid contents.
        /// </summary>
        [TestMethod]
        public void PersonalScopeWelcomeCardNotNullValidContent()
        {
            // ARRANGE
            this.hostingEnvironment
                .Setup(m => m.ContentRootPath)
                .Returns(".");

            this.memoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);

            var cardTemplate = File.ReadAllText(CardHelpersData.PersonalScopeWelcomeCardFilePath);
            var expectedCardJson = Newtonsoft.Json.JsonConvert.SerializeObject(AdaptiveCard.FromJson(cardTemplate).Card);

            // ACT
            var actualAttachmentCardResult = this.cardHelper.GetPersonalScopeWelcomeCard();
            var actualCardJson = Newtonsoft.Json.JsonConvert.SerializeObject(actualAttachmentCardResult.Content);

            // ASSERT
            Assert.IsNotNull(actualAttachmentCardResult);
            Assert.AreEqual(expectedCardJson, actualCardJson);
        }

        /// <summary>
        /// Test case to check if teams scope welcome card is not null and have valid contents.
        /// </summary>
        [TestMethod]
        public void TeamsScopeWelcomeCardNotNullValidContent()
        {
            // ARRANGE
            this.hostingEnvironment
                .Setup(m => m.ContentRootPath)
                .Returns(".");

            this.memoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);

            var cardTemplate = File.ReadAllText(CardHelpersData.TeamsScopeWelcomeCardFilePath);
            var expectedCardJson = Newtonsoft.Json.JsonConvert.SerializeObject(AdaptiveCard.FromJson(cardTemplate).Card);

            // ACT
            var actualAttachmentCardResult = this.cardHelper.GetTeamsScopeWelcomeCard();
            var actualCardJson = Newtonsoft.Json.JsonConvert.SerializeObject(actualAttachmentCardResult.Content);

            // ASSERT
            Assert.IsNotNull(actualAttachmentCardResult);
            Assert.AreEqual(expectedCardJson, actualCardJson);
        }

        /// <summary>
        ///  Test case to check if error message card is not null and have valid contents.
        /// </summary>
        [TestMethod]
        public void ErrorMessageCardNotNullValidContent()
        {
            // ARRANGE
            this.hostingEnvironment
                .Setup(m => m.ContentRootPath)
                .Returns(".");

            this.memoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);

            var cardTemplate = File.ReadAllText(CardHelpersData.ErrorMessageCardFilePath);
            var expectedCardJson = Newtonsoft.Json.JsonConvert.SerializeObject(AdaptiveCard.FromJson(cardTemplate).Card);

            // ACT
            var actualAttachmentCardResult = this.cardHelper.GetErrorMessageCard(CardHelpersData.ErrorMessageText);
            var actualCardJson = Newtonsoft.Json.JsonConvert.SerializeObject(actualAttachmentCardResult.Content);

            // ASSERT
            Assert.IsNotNull(actualAttachmentCardResult);
            Assert.AreEqual(expectedCardJson, actualCardJson);
        }

        /// <summary>
        ///  Test case to check if error message card is not null and contains valid HTML encoded data.
        /// </summary>
        [TestMethod]
        public void ErrorMessageCardNotNullValidHtmlContent()
        {
            // ARRANGE
            this.hostingEnvironment
                .Setup(m => m.ContentRootPath)
                .Returns(".");

            this.memoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);

            var cardTemplate = File.ReadAllText(CardHelpersData.HtmlEncodedErrorMessageCardFilePath);
            var expectedCardJson = Newtonsoft.Json.JsonConvert.SerializeObject(AdaptiveCard.FromJson(cardTemplate).Card);

            // ACT
            var actualAttachmentCardResult = this.cardHelper.GetErrorMessageCard(CardHelpersData.ErrorMessageTextWithHtml);
            var actualCardJson = Newtonsoft.Json.JsonConvert.SerializeObject(actualAttachmentCardResult.Content);

            // ASSERT
            Assert.IsNotNull(actualAttachmentCardResult);
            Assert.AreEqual(expectedCardJson, actualCardJson);
        }

        /// <summary>
        ///  Test case to throw exception while passing null data to construct error message card.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetErrorMessageCardArgumentNullException()
        {
            // ACT
            this.cardHelper.GetErrorMessageCard(null);
        }

        /// <summary>
        ///  Test case to check if questions answer card is not null and have valid contents.
        /// </summary>
        [TestMethod]
        public void QuestionsAnswerCardNotNullValidContent()
        {
            // ARRANGE
            this.hostingEnvironment
                .Setup(m => m.ContentRootPath)
                .Returns(".");

            this.memoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);

            var cardTemplate = File.ReadAllText(CardHelpersData.QuestionAnswerCardFilePath);
            var expectedCardJson = Newtonsoft.Json.JsonConvert.SerializeObject(AdaptiveCard.FromJson(cardTemplate).Card);

            this.questionAnswersMapper
                .Setup(m => m.MapToDataModel(QuestionsAnswersMapperData.QuestionAnswersViewModel))
                .Returns(QuestionsAnswersMapperData.QuestionAnswerCardData);

            // ACT
            var actualAttachmentCardResult = this.cardHelper.GetQuestionAnswerCard(QuestionsAnswersMapperData.QuestionAnswersViewModel);
            var actualCardJson = Newtonsoft.Json.JsonConvert.SerializeObject(actualAttachmentCardResult.Content);

            // ASSERT
            Assert.IsNotNull(actualAttachmentCardResult);
            Assert.AreEqual(expectedCardJson, actualCardJson);
        }

        /// <summary>
        ///  Test case to check if questions answer card is not null and contains valid HTML encoded data.
        /// </summary>
        [TestMethod]
        public void QuestionAnswerCardNotNullValidHtmlContent()
        {
            // ARRANGE
            this.hostingEnvironment
                .Setup(m => m.ContentRootPath)
                .Returns(".");

            this.memoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);

            this.questionAnswersMapper
                .Setup(m => m.MapToDataModel(QuestionsAnswersMapperData.QuestionAnswersViewModelHtmlContent))
                .Returns(QuestionsAnswersMapperData.QuestionAnswerCardDataHtmlContent);

            var cardTemplate = File.ReadAllText(CardHelpersData.HtmlEncodedQuestionAnswerCardFilePath);
            var expectedCardJson = Newtonsoft.Json.JsonConvert.SerializeObject(AdaptiveCard.FromJson(cardTemplate).Card);

            // ACT
            var actualAttachmentCardResult = this.cardHelper.GetQuestionAnswerCard(QuestionsAnswersMapperData.QuestionAnswersViewModelHtmlContent);
            var actualCardJson = Newtonsoft.Json.JsonConvert.SerializeObject(actualAttachmentCardResult.Content);

            // ASSERT
            Assert.IsNotNull(actualAttachmentCardResult);
            Assert.AreEqual(expectedCardJson, actualCardJson);
        }

        /// <summary>
        ///  Test case to throw exception while passing null data to construct question answer card.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetQuestionAnswerCardArgumentNullException()
        {
            // ACT
            this.cardHelper.GetQuestionAnswerCard(null);
        }
    }
}