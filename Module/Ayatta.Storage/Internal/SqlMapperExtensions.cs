using System;
using Ayatta;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Dapper
{
    public static class SqlMapperExtensions
    {
        public static PagedList<TFirst> PagedList<TFirst, TSecond, TKey>(this IDbConnection connection, int index, int size,
            CommandDefinition command, Func<TFirst, TKey> firstKey, Func<TSecond, TKey> secondKey,
            Action<TFirst, IEnumerable<TSecond>> addChildren)
        {

            long count;
            var data = new List<TFirst>();
            var second = new List<TSecond>();
            using (var conn = connection)
            {
                conn.Open();
                using (var reader = connection.QueryMultiple(command))
                {
                    count = reader.ReadFirst<long>();
                    if (count > 0)
                    {
                        data = reader.Read<TFirst>().ToList();
                        second = reader.Read<TSecond>().ToList();
                    }
                }
                var children = second.GroupBy(secondKey).ToDictionary(g => g.Key, g => g.AsEnumerable());

                foreach (var item in data)
                {
                    IEnumerable<TSecond> temp;
                    if (children.TryGetValue(firstKey(item), out temp))
                    {
                        addChildren(item, temp);
                    }
                }
            }
            return new PagedList<TFirst>(data, index, size, (int)count);
        }

        public static IPagedList<T> PagedList<T>(this IDbConnection connection, int index, int size, CommandDefinition command) where T : IEntity
        {
            long count;
            var data = new List<T>();
            using (var conn = connection)
            {
                conn.Open();
                using (var reader = conn.QueryMultiple(command))
                {
                    count = reader.ReadFirst<long>();
                    if (count > 0)
                    {
                        data = reader.Read<T>().ToList();
                    }
                }
            }
            return new PagedList<T>(data, index, size, (int)count);
        }

        public static IEnumerable<TFirst> Query<TFirst, TSecond, TKey>(this IDbConnection connection, CommandDefinition command, Func<TFirst, TKey> firstKey, Func<TSecond, TKey> secondKey, Action<TFirst, IEnumerable<TSecond>> addChildren)
        {
            List<TFirst> parent;
            List<TSecond> children;
            using (var reader = connection.QueryMultiple(command))
            {
                parent = reader.Read<TFirst>().ToList();
                children = reader.Read<TSecond>().ToList();
            }
            var temp = children.GroupBy(secondKey).ToDictionary(g => g.Key, g => g.AsEnumerable());

            foreach (var item in parent)
            {
                IEnumerable<TSecond> second;
                if (temp.TryGetValue(firstKey(item), out second))
                {
                    addChildren(item, children);
                }
            }
            return parent;

        }

        public static IEnumerable<TFirst> Query<TFirst, TSecond>(this IDbConnection connection, CommandDefinition command, string splitOn = "Id")
        {
            return connection.Query(command.CommandText, MappingCache<TFirst, TSecond>.Map, command.Parameters, command.Transaction, command.Buffered, splitOn, command.CommandTimeout, command.CommandType);
        }

        public static IEnumerable<TFirst> Query<TFirst, TSecond>(this IDbConnection connection, string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {

            return SqlMapper.Query<TFirst, TSecond, TFirst>(connection, sql, MappingCache<TFirst, TSecond>.Map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static IEnumerable<TFirst> Query<TFirst, TSecond, TThird>(this IDbConnection connection, string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return SqlMapper.Query<TFirst, TSecond, TThird, TFirst>(connection, sql, MappingCache<TFirst, TSecond, TThird>.Map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static IEnumerable<TFirst> Query<TFirst, TSecond, TThird, TFourth>(this IDbConnection connection, string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFirst>(connection, sql, MappingCache<TFirst, TSecond, TThird, TFourth>.Map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public static IEnumerable<TFirst> Query<TFirst, TSecond, TThird, TFourth, TFifth>(this IDbConnection connection, string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFifth, TFirst>(connection, sql, MappingCache<TFirst, TSecond, TThird, TFourth, TFifth>.Map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        internal static class MappingCache
        {
            internal static Expression GetSetExpression(ParameterExpression sourceExpression, params ParameterExpression[] destinationExpressions)
            {
                var destination = destinationExpressions
                    .Select(parameter => new
                    {
                        Parameter = parameter,
                        Property = parameter.Type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(property => property.CanWrite && !property.GetIndexParameters().Any()).FirstOrDefault(property => property.PropertyType == sourceExpression.Type /*|| sourceExpression.Type.IsSubclassOf(property.PropertyType)*/)
                    }).FirstOrDefault(parameter => parameter.Property != null);

                if (destination == null)
                {
                    throw new InvalidOperationException(string.Format("No writable property of type {0} found in types {1}.", sourceExpression.Type.FullName, string.Join(", ", destinationExpressions.Select(parameter => parameter.Type.FullName))));
                }

                return Expression.IfThen(Expression.Not(Expression.Equal(destination.Parameter, Expression.Constant(null))), Expression.Call(destination.Parameter, destination.Property.GetSetMethod(), sourceExpression));
            }
        }

        internal static class MappingCache<TFirst, TSecond>
        {
            static MappingCache()
            {
                var first = Expression.Parameter(typeof(TFirst), "first");
                var second = Expression.Parameter(typeof(TSecond), "second");

                var secondSetExpression = MappingCache.GetSetExpression(second, first);

                var blockExpression = Expression.Block(first, second, secondSetExpression, first);

                Map = Expression.Lambda<Func<TFirst, TSecond, TFirst>>(blockExpression, first, second).Compile();
            }

            internal static Func<TFirst, TSecond, TFirst> Map { get; private set; }
        }

        internal static class MappingCache<TFirst, TSecond, TThird>
        {
            static MappingCache()
            {
                var first = Expression.Parameter(typeof(TFirst), "first");
                var second = Expression.Parameter(typeof(TSecond), "second");
                var third = Expression.Parameter(typeof(TThird), "third");

                var secondSetExpression = MappingCache.GetSetExpression(second, first);
                var thirdSetExpression = MappingCache.GetSetExpression(third, first, second);

                var blockExpression = Expression.Block(first, second, third, secondSetExpression, thirdSetExpression, first);

                Map = Expression.Lambda<Func<TFirst, TSecond, TThird, TFirst>>(blockExpression, first, second, third).Compile();
            }

            internal static Func<TFirst, TSecond, TThird, TFirst> Map { get; private set; }
        }

        internal static class MappingCache<TFirst, TSecond, TThird, TFourth>
        {
            static MappingCache()
            {
                var first = Expression.Parameter(typeof(TFirst), "first");
                var second = Expression.Parameter(typeof(TSecond), "second");
                var third = Expression.Parameter(typeof(TThird), "third");
                var fourth = Expression.Parameter(typeof(TFourth), "fourth");

                var secondSetExpression = MappingCache.GetSetExpression(second, first);
                var thirdSetExpression = MappingCache.GetSetExpression(third, first, second);
                var fourthSetExpression = MappingCache.GetSetExpression(fourth, first, second, third);

                var blockExpression = Expression.Block(first, second, third, fourth, secondSetExpression, thirdSetExpression, fourthSetExpression, first);

                Map = Expression.Lambda<Func<TFirst, TSecond, TThird, TFourth, TFirst>>(blockExpression, first, second, third, fourth).Compile();
            }

            internal static Func<TFirst, TSecond, TThird, TFourth, TFirst> Map { get; private set; }
        }

        internal static class MappingCache<TFirst, TSecond, TThird, TFourth, TFifth>
        {
            static MappingCache()
            {
                var first = Expression.Parameter(typeof(TFirst), "first");
                var second = Expression.Parameter(typeof(TSecond), "second");
                var third = Expression.Parameter(typeof(TThird), "third");
                var fourth = Expression.Parameter(typeof(TFourth), "fourth");
                var fifth = Expression.Parameter(typeof(TFifth), "fifth");

                var secondSetExpression = MappingCache.GetSetExpression(second, first);
                var thirdSetExpression = MappingCache.GetSetExpression(third, first, second);
                var fourthSetExpression = MappingCache.GetSetExpression(fourth, first, second, third);
                var fifthSetExpression = MappingCache.GetSetExpression(fifth, first, second, third, fourth);

                var blockExpression = Expression.Block(first, second, third, fourth, fifth, secondSetExpression, thirdSetExpression, fourthSetExpression, fifthSetExpression, first);

                Map = Expression.Lambda<Func<TFirst, TSecond, TThird, TFourth, TFifth, TFirst>>(blockExpression, first, second, third, fourth, fifth).Compile();
            }

            internal static Func<TFirst, TSecond, TThird, TFourth, TFifth, TFirst> Map { get; private set; }
        }
    }
}