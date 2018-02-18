namespace Js.Snippets.CSharp.RandomUtils
{
    using System;
    using System.Collections.Generic;
    using Js.Snippets.CSharp.IComparable;

    /// <summary>
    /// <see cref="Random"/> extension methods
    /// </summary>
    public static class RandomExtensions
    {
        public static bool CoinToss(this Random rng, double successProbability = .5)
        {
            if (!successProbability.IsBetween(0, 1))
            {
                throw new ArgumentOutOfRangeException(nameof(successProbability));
            }

            return rng.NextDouble() < successProbability;
        }

        public static T OneOf<T>(this Random rng, IList<T> values)
        {
            return values[rng.Next(values.Count)];
        }

        public static T OneOf<T>(this Random rng, params T[] values)
        {
            return values[rng.Next(values.Length)];
        }
    }
}
