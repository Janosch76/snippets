namespace Js.Snippets.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Caching;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Js.Snippets.CSharp.AsyncUtils;
    using Js.Snippets.CSharp.CacheUtils;
    using Js.Snippets.CSharp.Collections.Concurrent;
    using Js.Snippets.CSharp.CollectionUtils;
    using Js.Snippets.CSharp.ComparableUtils;
    using Js.Snippets.CSharp.DateUtils;
    using Js.Snippets.CSharp.EnumUtils;
    using Js.Snippets.CSharp.IComparable;
    using Js.Snippets.CSharp.RandomUtils;
    using Js.Snippets.CSharp.StreamUtils;
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
        public void ToQueryString()
        {
            var url = "http://example.com/path/to/page";
            var queryParams = new Dictionary<string, string>() { { "name", "ferret" }, { "color", "purple" } };

            Assert.AreEqual(
                "http://example.com/path/to/page?name=ferret&color=purple",
                url + "?" + queryParams.ToQueryString());
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
        public void DistinctBy()
        {
            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var result = items.DistinctBy(v => v % 3);

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, result.ToArray());
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
        public void StdDev()
        {
            Assert.AreEqual(0, new double[] { }.StdDev());
            Assert.AreEqual(0, new double[] { 1 }.StdDev());
            Assert.AreEqual(0, new double[] { 1, 1, 1, 1, 1, 1 }.StdDev());
            Assert.AreEqual(50, new[] { 100, 0, 100, 0 }.StdDev(v => v));
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
            var array = new int[2, 2] { { 0, 1 }, { 2, 3 } };

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

            var date = new DateTime(2018, 02, 18);
            Assert.AreEqual(true, date.IsBetween(new DateTime(2018, 01, 01), new DateTime(2018, 02, 28)));
        }

        [TestMethod]
        public void ComparerFromFactory()
        {
            var values = new int[] { 3, 5, 2, 4, 2, 1 };
            var comparer = ComparerFactory.Create<int>((x, y) => y <= x);

            CollectionAssert.AreEqual(new[] { 5, 4, 3, 2, 2, 1 }, values.OrderBy(x => x, comparer).ToArray());
        }

        [TestMethod]
        public void Dates()
        {
            Assert.AreEqual(new DateTime(2018, 03, 09), new DateTime(2018, 02, 18) + 2.Weeks() + 5.Days());
            Assert.AreEqual("2018-02-18", new DateTime(2018, 02, 18).ToDateIso());
            Assert.AreEqual("2018-02-18T20:30:11.9990000+01:00", new DateTime(2018, 02, 18, 19, 30, 11, 999).ToLocalTime().ToDateTimeIso());
        }

        [TestMethod]
        public void IsWeekend()
        {
            Assert.AreEqual(true, new DateTime(2018, 05, 19).IsWeekend(), "Sat");
            Assert.AreEqual(true, new DateTime(2018, 05, 20).IsWeekend(), "Sun");
            Assert.AreEqual(false, new DateTime(2018, 05, 21).IsWeekend(), "Mon");
            Assert.AreEqual(false, new DateTime(2018, 05, 22).IsWeekend(), "Tue");
            Assert.AreEqual(false, new DateTime(2018, 05, 23).IsWeekend(), "Wed");
            Assert.AreEqual(false, new DateTime(2018, 05, 24).IsWeekend(), "Thu");
            Assert.AreEqual(false, new DateTime(2018, 05, 25).IsWeekend(), "Fri");
        }

        [TestMethod]
        public void IsWorkingDay()
        {
            Assert.AreEqual(false, new DateTime(2018, 05, 19).IsWorkingDay(), "Sat");
            Assert.AreEqual(false, new DateTime(2018, 05, 20).IsWorkingDay(), "Sun");
            Assert.AreEqual(true, new DateTime(2018, 05, 21).IsWorkingDay(), "Mon");
            Assert.AreEqual(true, new DateTime(2018, 05, 22).IsWorkingDay(), "Tue");
            Assert.AreEqual(true, new DateTime(2018, 05, 23).IsWorkingDay(), "Wed");
            Assert.AreEqual(true, new DateTime(2018, 05, 24).IsWorkingDay(), "Thu");
            Assert.AreEqual(true, new DateTime(2018, 05, 25).IsWorkingDay(), "Fri");
        }

        [TestMethod]
        public void NextWorkDay()
        {
            Assert.AreEqual(new DateTime(2018, 05, 21), new DateTime(2018, 05, 19).NextWorkday(), "Sat");
            Assert.AreEqual(new DateTime(2018, 05, 21), new DateTime(2018, 05, 20).NextWorkday(), "Sun");
            Assert.AreEqual(new DateTime(2018, 05, 21), new DateTime(2018, 05, 21).NextWorkday(), "Mon");
        }

        [TestMethod]
        public void NextDayOfWeek()
        {
            Assert.AreEqual(new DateTime(2018, 05, 21), new DateTime(2018, 05, 19).Next(DayOfWeek.Monday), "Sat");
            Assert.AreEqual(new DateTime(2018, 05, 21), new DateTime(2018, 05, 20).Next(DayOfWeek.Monday), "Sun");
            Assert.AreEqual(new DateTime(2018, 05, 21), new DateTime(2018, 05, 21).Next(DayOfWeek.Monday), "Mon");
        }

        [TestMethod]
        public void OneOf()
        {
            var rng = new Random();
            var c = rng.OneOf(1, 2, 3);
            CollectionAssert.Contains(new[] { 1, 2, 3 }, c);
        }

        public enum State
        {
            [System.ComponentModel.Description("Operationn waiting")]
            Waiting,

            [System.ComponentModel.Description("Operation in progress")]
            InProgress,

            [System.ComponentModel.Description("Operation cancelled")]
            Cancelled,

            [System.ComponentModel.Description("Operation finished")]
            Finished,

            [System.ComponentModel.Description("Operation failed")]
            Failed,
        }

        [TestMethod]
        public void EnumParse()
        {
            Assert.AreEqual(State.Cancelled, Enums.Parse<State>("Cancelled"));
            AssertThrows<ArgumentException>(() => Enums.Parse<State>("CANCELLED"));
            Assert.AreEqual(State.Cancelled, Enums.Parse<State>("CANCELLED", ignoreCase: true));
        }

        [TestMethod]
        public void EnumToList()
        {
            var states = Enums.ToList<State>();
            CollectionAssert.AreEqual(new[] { State.Waiting, State.InProgress, State.Cancelled, State.Finished, State.Failed }, states.ToArray());
        }

        [TestMethod]
        public void EnumToDescriptionsList()
        {
            var states = Enums.ToDictionary<State>().Select(kv => kv.Value);
            CollectionAssert.AreEqual(new[] { "Operationn waiting", "Operation in progress", "Operation cancelled", "Operation finished", "Operation failed" }, states.ToArray());
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
        public void JoinStrings()
        {
            var values = new[] { 1, 2, 3 };
            Assert.AreEqual("1,2,3", values.Join(","));
            Assert.AreEqual("'1','2','3'", values.Join(",", v => $"'{v}'"));
        }

        [TestMethod]
        public void ConvertXmlRepresentations()
        {
            var xml = new XElement("items", new XElement("item", new XAttribute("value", "2")));

            Assert.AreEqual(@"<items><item value=""2"" /></items>", xml.ToXmlDocument().InnerXml);
        }

        [TestMethod]
        public void AddOrInsertCachedItem()
        {
            ObjectCache cache = MemoryCache.Default;
            var policy = new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromMinutes(1) };
            var value = cache.AddOrGetExisting<int>("foo", () => { return 1; }, policy);
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public async Task AddOrInsertCachedItemAsync()
        {
            ObjectCache cache = MemoryCache.Default;
            var policy = new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromMinutes(1) };
            var value = await cache.AddOrGetExistingAsync<int>("foo", async () => { await Task.Delay(100); return 1; }, policy);
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public async Task StreamCopyTo()
        {
            string result;
            var progress = new List<long>();
            var progressReporter = new Progress<long>(bytesRead => progress.Add(bytesRead));
            using (var source = new MemoryStream())
            {
                using (var destination = new MemoryStream())
                {
                    using (var writer = new StreamWriter(source, Encoding.ASCII, 1024, leaveOpen: true))
                    {
                        await writer.WriteAsync(new string('a', 1000));
                        await writer.FlushAsync();
                        source.Position = 0;
                    }

                    await source.CopyToAsync(destination, 100, CancellationToken.None, progressReporter);
                    await Task.Delay(50);

                    using (var reader = new StreamReader(destination))
                    {
                        destination.Position = 0;
                        result = await reader.ReadToEndAsync();
                    }
                }
            }

            Assert.AreEqual(new string('a', 1000), result);
            Assert.AreEqual(10, progress.Count());
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

        [TestMethod]
        public async Task ConcurrentHashSetAddRemove()
        {
            var set = new ConcurrentHashSet<int>();

            var tasks = Enumerable.Range(0, 10000)
                .Select(i => AddRemove(i % 3));
            await Task.WhenAll(tasks.ToArray());

            Assert.AreEqual(0, set.Count);

            async Task AddRemove(int i)
            {
                set.Add(i);
                await Task.Delay(10);
                set.Remove(i);
            }
        }

        private static async Task<int> DelayAndReturn(int value)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(value));
            return value;
        }

        private TException AssertThrows<TException>(Action code) where TException : Exception
        {
            try
            {
                code();
            }
            catch (Exception e)
            {
                var expectedException = e as TException;
                if (expectedException == null)
                {
                    Assert.Fail($"Expected exception of type {typeof(TException)}, but {e.GetType()} was raised.");
                }

                return expectedException;
            }

            Assert.Fail($"Expected exception of type {typeof(TException)}, but no exception was raised.");
            return null;
        }
    }
}