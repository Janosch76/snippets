namespace Js.Snippets.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Js.Snippets.CSharp.AsyncUtils;
    using Js.Snippets.CSharp.CollectionUtils;
    using Js.Snippets.CSharp.DateUtils;
    using Js.Snippets.CSharp.EnumUtils;
    using Js.Snippets.CSharp.IComparable;
    using Js.Snippets.CSharp.RandomUtils;
    using Js.Snippets.CSharp.StringUtils;
    using Js.Snippets.CSharp.XmlUtils;
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
        public void RemoveFirst()
        {
            Assert.AreEqual("bc", "abc".RemoveFirst());
            Assert.AreEqual("abc", "abc".RemoveFirst(0));
            Assert.AreEqual("bc", "abc".RemoveFirst(1));
            Assert.AreEqual("c", "abc".RemoveFirst(2));
            Assert.AreEqual("", "abc".RemoveFirst(3));
            Assert.AreEqual("", "abc".RemoveFirst(4));
        }

        [TestMethod]
        public void RemoveLast()
        {
            Assert.AreEqual("ab", "abc".RemoveLast());
            Assert.AreEqual("abc", "abc".RemoveLast(0));
            Assert.AreEqual("ab", "abc".RemoveLast(1));
            Assert.AreEqual("a", "abc".RemoveLast(2));
            Assert.AreEqual("", "abc".RemoveLast(3));
            Assert.AreEqual("", "abc".RemoveLast(4));
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
        public void Batch()
        {
            var batches = new[] { 1, 2, 3, 4, 5 }.Batch(2).ToArray();

            Assert.AreEqual(3, batches.Count());
            CollectionAssert.AreEqual(new[] { 1, 2 }, batches[0].ToArray());
            CollectionAssert.AreEqual(new[] { 3, 4 }, batches[1].ToArray());
            CollectionAssert.AreEqual(new[] { 5 }, batches[2].ToArray());
        }

        [TestMethod]
        public void Segment()
        {
            var segments = new[] { 3, 2, 2, 3, 1, 1, 1, 2 }.Segments().ToArray();

            Assert.AreEqual(5, segments.Count());
            CollectionAssert.AreEqual(new[] { 3 }, segments[0].ToArray());
            CollectionAssert.AreEqual(new[] { 2, 2 }, segments[1].ToArray());
            CollectionAssert.AreEqual(new[] { 3 }, segments[2].ToArray());
            CollectionAssert.AreEqual(new[] { 1, 1, 1 }, segments[3].ToArray());
            CollectionAssert.AreEqual(new[] { 2 }, segments[4].ToArray());
        }

        [TestMethod]
        public void Split()
        {
            var buckets = new[] { 1, 2, 3, 4, 5 }.Split(2, 3, 7).ToArray();

            Assert.AreEqual(3, buckets.Count());
            CollectionAssert.AreEqual(new[] { 1, 2 }, buckets[0].ToArray());
            CollectionAssert.AreEqual(new[] { 3 }, buckets[1].ToArray());
            CollectionAssert.AreEqual(new[] { 4, 5 }, buckets[2].ToArray());
        }

        [TestMethod]
        public void SlicesOfMultiDimensionalArray()
        {
            var array = new int[2, 2] { { 0, 1 }, { 2, 3} };

            CollectionAssert.AreEqual(new[] { 0, 1 }, array.Row(0).ToArray());
            CollectionAssert.AreEqual(new[] { 2, 3 }, array.Row(1).ToArray());

            CollectionAssert.AreEqual(new[] { 0, 2 }, array.Column(0).ToArray());
            CollectionAssert.AreEqual(new[] { 1, 3 }, array.Column(1).ToArray());
        }

        [TestMethod]
        public void IsBetween()
        {
            Assert.AreEqual(true, 1.IsBetween(0, 2));
            Assert.AreEqual(true, 1.IsBetween(1, 1));
            Assert.AreEqual(false, 1.IsBetween(2, 5));
        }

        [TestMethod]
        public void Dates()
        {
            Assert.AreEqual(new DateTime(2018, 03, 09), new DateTime(2018, 02, 18) + 2.Weeks() + 5.Days());
            Assert.AreEqual("2018-02-18", new DateTime(2018, 02, 18).ToDateIso());
            Assert.AreEqual("2018-02-18T20:30:11.9990000+01:00", new DateTime(2018, 02, 18, 19, 30, 11, 999).ToLocalTime().ToDateTimeIso());
        }

        [TestMethod]
        public void OneOf()
        {
            var rng = new Random();
            var c = rng.OneOf(1, 2, 3);
            CollectionAssert.Contains(new[] { 1, 2, 3 }, c);
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
            Assert.AreEqual("1 + {y}", "{x} + {y}".ReplaceTokens(false, new { x = 1 }));

            var tokenReplacer = new TokenReplacer(new[] { new { Date = new DateTime(2018, 01, 31) } }, true, '%', '%');
            Assert.AreEqual("20180131", tokenReplacer.ReplaceTokens("%Date:yyyyMMdd%"));
        }

        [TestMethod]
        public void ConvertXmlRepresentations()
        {
            var xml = new XElement("items", new XElement("item", new XAttribute("value", "2")));

            Assert.AreEqual(@"<items><item value=""2"" /></items>", xml.ToXmlDocument().InnerXml);
        }

        [TestMethod]
        public async Task RepeatedTask_Run()
        {
            int counter = 0;
            await PeriodicTask.Repeat(3, () => counter++, TimeSpan.FromMilliseconds(10));
            Assert.AreEqual(3, counter);
        }

        [TestMethod]
        public async Task OrderByCompletion()
        {
            var tasks = new Task<int>[]
            {
                DelayAndReturn(200),
                DelayAndReturn(300),
                DelayAndReturn(100)
            };

            var results = new List<int>();
            foreach (var task in tasks.OrderByCompletion())
            {
                results.Add(await task);
            }

            CollectionAssert.AreEqual(new[] { 100, 200, 300 }, results);
        }

        [TestMethod]
        public async Task SnapshotRefresh()
        {
            var initialValue = 0;
            var snapshot = new Snapshot<int>(() => DelayAndReturn(100), initialValue);

            Assert.AreEqual(initialValue, snapshot.Value);
            Assert.AreEqual(DateTime.MinValue, snapshot.LastRefreshed);

            await snapshot.Refresh();

            Assert.AreEqual(100, snapshot.Value);
            Assert.AreNotEqual(DateTime.MinValue, snapshot.LastRefreshed);
        }

        private static async Task<int> DelayAndReturn(int value)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(value));
            return value;
        }
    }
}