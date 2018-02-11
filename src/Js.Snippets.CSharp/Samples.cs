namespace Js.Snippets.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Js.Snippets.CSharp.EnumUtils;
    using Js.Snippets.CSharp.StringUtils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Samples
    {
        [TestMethod]
        public void Left()
        {
            Assert.AreEqual("ab", "abc".Left(2));
            Assert.AreEqual("", "abc".Left(0));
            Assert.AreEqual("abc", "abc".Left(5));
            Assert.AreEqual("", "".Left(5));
        }

        [TestMethod]
        public void Right()
        {
            Assert.AreEqual("bc", "abc".Right(2));
            Assert.AreEqual("", "abc".Right(0));
            Assert.AreEqual("abc", "abc".Right(5));
            Assert.AreEqual("", "".Right(5));
        }

        [TestMethod]
        public void In()
        {
            Assert.AreEqual(true, "b".In("a", "b", "c"));
            Assert.AreEqual(false, "B".In("a", "b", "c"));
            Assert.AreEqual(false, "d".In("a", "b", "c"));

            Assert.AreEqual(true, TaskStatus.Canceled.In(TaskStatus.Canceled, TaskStatus.RanToCompletion));
        }

        [TestMethod]
        public void ToEnum()
        {
            Assert.AreEqual(TaskStatus.Canceled, "Canceled".ToEnum<TaskStatus>());
            Assert.AreEqual(TaskStatus.Canceled, "CANCELED".ToEnum<TaskStatus>(ignoreCase: true));
        }

        [TestMethod]
        public void ReplaceTokens()
        {
            Assert.AreEqual("C:\\Temp\\a.out", "{RootFolder}\\a.out".ReplaceTokens(new { RootFolder = "C:\\Temp" }));
            Assert.AreEqual("1 + 2", "{x} + {y}".ReplaceTokens(new { x = 1, y = 2 }));
            Assert.AreEqual("20180131", "{Date: yyyyMMdd}".ReplaceTokens(new { Date = new DateTime(2018, 01, 31) }));
            Assert.AreEqual("1", "{Params[1]}".ReplaceTokens(new { Params = new int[] { 0, 1, 2 } }));
            Assert.AreEqual("C:\\Temp\\a.out", "{Params[Folder2]}\\a.out".ReplaceTokens(new { Params = new Dictionary<string, string>() { { "Folder1", "C:\\User" }, { "Folder2", "C:\\Temp" } } }));

            var tokenReplacer = new TokenReplacer(new[] { new { Date = new DateTime(2018, 01, 31) } }, '%', '%');
            Assert.AreEqual("20180131", tokenReplacer.ReplaceTokens("%Date:yyyyMMdd%"));
        }
    }
}
