namespace Ayatta
{
    /// <summary>
    /// 魔法类
    /// </summary>
    /// <typeparam name="TFirst"></typeparam>
    public class Magic<TFirst>
    {

        public TFirst First { get; set; }

        public Magic()
            : this(default(TFirst))
        {

        }

        public Magic(TFirst first)
        {
            First = first;
        }
    }

    /// <summary>
    /// 魔法类
    /// </summary>
    /// <typeparam name="TFirst"></typeparam>
    /// <typeparam name="TSecond"></typeparam>
    public class Magic<TFirst, TSecond> : Magic<TFirst>
    {
        public TSecond Second { get; set; }

        public Magic()
            : this(default(TFirst))
        {

        }

        public Magic(TFirst first)
            : this(first, default(TSecond))
        {

        }

        public Magic(TFirst first, TSecond second)
        {
            First = first;
            Second = second;
        }
    }

    /// <summary>
    /// 魔法类
    /// </summary>
    /// <typeparam name="TFirst"></typeparam>
    /// <typeparam name="TSecond"></typeparam>
    /// <typeparam name="TThird"></typeparam>
    public class Magic<TFirst, TSecond, TThird> : Magic<TFirst, TSecond>
    {
        public TThird Third { get; set; }

        public Magic()
            : this(default(TFirst))
        {

        }

        public Magic(TFirst first)
            : this(first, default(TSecond))
        {

        }

        public Magic(TFirst first, TSecond second)
            : this(first, second, default(TThird))
        {

        }

        public Magic(TFirst first, TSecond second, TThird third)
        {
            First = first;
            Second = second;
            Third = third;
        }
    }

    /// <summary>
    /// 魔法类
    /// </summary>
    /// <typeparam name="TFirst"></typeparam>
    /// <typeparam name="TSecond"></typeparam>
    /// <typeparam name="TThird"></typeparam>
    /// <typeparam name="TFourth"></typeparam>
    public class Magic<TFirst, TSecond, TThird, TFourth> : Magic<TFirst, TSecond, TThird>
    {
        public TFourth Fourth { get; set; }
        public Magic()
            : this(default(TFirst))
        {

        }

        public Magic(TFirst first)
            : this(first, default(TSecond))
        {

        }

        public Magic(TFirst first, TSecond second)
            : this(first, second, default(TThird))
        {

        }

        public Magic(TFirst first, TSecond second, TThird third)
            : this(first, second, third, default(TFourth))
        {

        }
        public Magic(TFirst first, TSecond second, TThird third, TFourth fourth)
        {
            First = first;
            Second = second;
            Third = third;
            Fourth = fourth;
        }
    }

    /// <summary>
    /// 魔法类
    /// </summary>
    /// <typeparam name="TFirst"></typeparam>
    /// <typeparam name="TSecond"></typeparam>
    /// <typeparam name="TThird"></typeparam>
    /// <typeparam name="TFourth"></typeparam>
    /// <typeparam name="TFifth"></typeparam>
    public class Magic<TFirst, TSecond, TThird, TFourth, TFifth> : Magic<TFirst, TSecond, TThird, TFourth>
    {
        public TFifth Fifth { get; set; }
        public Magic()
            : this(default(TFirst))
        {

        }

        public Magic(TFirst first)
            : this(first, default(TSecond))
        {

        }

        public Magic(TFirst first, TSecond second)
            : this(first, second, default(TThird))
        {

        }

        public Magic(TFirst first, TSecond second, TThird third)
            : this(first, second, third, default(TFourth))
        {

        }
        public Magic(TFirst first, TSecond second, TThird third, TFourth fourth)
            : this(first, second, third, fourth, default(TFifth))
        {

        }
        public Magic(TFirst first, TSecond second, TThird third, TFourth fourth, TFifth fifth)
        {
            First = first;
            Second = second;
            Third = third;
            Fourth = fourth;
            Fifth = fifth;
        }
    }
}