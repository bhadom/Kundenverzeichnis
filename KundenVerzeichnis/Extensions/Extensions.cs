using System;
using System.Collections.Generic;
using System.Linq;

namespace KundenVerzeichnis.Extensions
{
    /// <summary>
    /// Contains helpful Extension Methods 
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// A dictionary to summarize the price per month
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="opDate"></param>
        /// <param name="opPrice"></param>
        /// <returns>
        /// The sum of all months
        /// </returns>
        public static Dictionary<Byte, Decimal> sumPerMonth<T>(this IEnumerable<T> source, Func<T, DateTime> opDate, Func<T, Decimal> opPrice)
        {
            Dictionary<Byte, Decimal> pricesPerMonth = new Dictionary<byte, decimal>();
            for (byte i = 1; i <= 12; i++) pricesPerMonth.Add(i, source.Where(t => opDate(t).Month == i).Sum(t => opPrice(t)));
            return pricesPerMonth;
        }
    }
}
