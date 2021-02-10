// <copyright file="ListExtensionsTest.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.Helpers
{
    using System.Linq;
    using Microsoft.Teams.Apps.SearchCoach.Helpers;
    using Microsoft.Teams.Apps.SearchCoach.Tests.TestData;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Class to test list extension methods.
    /// </summary>
    [TestClass]
    public class ListExtensionsTest
    {
        /// <summary>
        /// Test case to check if a list of items is splitting properly based on BatchSplitCount = 20.
        /// </summary>
        [TestMethod]
        public void SplitListNotNullValidCount()
        {
            // ACT
            var actualResult = ListExtensions.SplitList(ListExtensionsData.UserObjectIds).ToList();

            // ASSERT
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(ListExtensionsData.BatchSplitCount, actualResult[0].Count);
        }

        /// <summary>
        /// Test case to check if a list is splitting with zero count, if passing empty list.
        /// </summary>
        [TestMethod]
        public void SplitListEmptyListZeroCount()
        {
            // ACT
            var actualResult = ListExtensions.SplitList(ListExtensionsData.UserObjectIdsEmptyList).ToList();

            // ASSERT
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(0, actualResult.Count);
        }
    }
}