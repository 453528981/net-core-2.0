using System;
using System.Linq;
using System.Collections.Generic;

namespace Ayatta
{
    /// <summary>
    /// 按权重随机
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RandomEntity<T> : BaseEntity<T>
    {
        /// <summary>
        /// 权重 >0
        /// </summary>
        public int Impression { get; set; }
    }

    public static class RandomEntityExtension
    {
        //private static Random random;
        public static RandomEntity<T> Random<T>(this IList<RandomEntity<T>> sources)
        {
            if ((sources == null) || (sources.Count == 0))
            {
                return null;
            }

            var maxValue = sources.Sum(o => o.Impression);

            if (maxValue == 0)
            {
                return null;
            }
            var randomNumber = GetRandomNumber(maxValue);

            var temp = 0;
            var index = -1;
            for (var j = 0; j < sources.Count; j++)
            {
                temp += sources[j].Impression;
                if (randomNumber > temp) continue;
                index = j;
                break;
            }
            return sources[index];
        }

        /// <summary>
        /// 随机种子值
        /// </summary>
        /// <returns></returns>
        private static int GetRandomNumber(int maxValue)
        {
            //var bytes = new byte[4];
            //var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            //rng.GetBytes(bytes);
            //var seed = BitConverter.ToInt32(bytes, 0);

            //if (random == null)
            //{
            //    random = new Random(seed);
            //}
            //return (random.Next(maxValue) + 1);
            return 1;
        }
    }

}