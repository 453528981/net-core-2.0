using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Dapper
{
    public static class SqlBuilder
    {
        public class SelectBuilder
        {
            private readonly IDictionary<string, Clauses> data = new Dictionary<string, Clauses>();

            private class Clause
            {
                public string Sql { get; set; }
                public dynamic Parameters { get; set; }
            }

            private class Clauses : List<Clause>
            {
                private readonly string joiner;
                private readonly string prefix;
                private readonly string postfix;

                public Clauses(string joiner, string prefix = "", string postfix = "")
                {
                    this.joiner = joiner;
                    this.prefix = prefix;
                    this.postfix = postfix;
                }

                public override string ToString()
                {
                    return prefix + string.Join(joiner, this.Select(c => c.Sql)) + postfix;
                }
            }

            //private readonly static Regex regex = new Regex(@"\/\*\*.+\*\*\/", RegexOptions.Compiled | RegexOptions.Multiline);

            private void AddClause(string name, string sql, dynamic parameters, string joiner, string prefix = "", string postfix = "")
            {
                Clauses clauses;
                if (!data.TryGetValue(name, out clauses))
                {
                    clauses = new Clauses(joiner, prefix, postfix);
                    data[name] = clauses;
                }
                clauses.Add(new Clause { Sql = sql, Parameters = parameters });
            }

            internal SelectBuilder(string field)
            {
                AddClause("select", field, null, " , ", prefix: "", postfix: "\n");
            }

            public SelectBuilder From(string table)
            {
                AddClause("from", table, null, " , ", "", "\n");
                return this;
            }

            public SelectBuilder Where(string sql, dynamic parameters = null)
            {
                AddClause("where", sql, parameters, " AND ", prefix: "WHERE ", postfix: "\n");
                return this;
            }

            public SelectBuilder Where(bool condition, string sql, dynamic parameters = null)
            {
                if (condition)
                {
                    AddClause("where", sql, parameters, " AND ", prefix: "WHERE ", postfix: "\n");
                }
                return this;
            }

            public SelectBuilder OrderBy(string sql, dynamic parameters = null)
            {
                AddClause("orderby", sql, parameters, " , ", prefix: "ORDER BY ", postfix: " ");
                return this;
            }

            public SelectBuilder GroupBy(string sql, dynamic parameters = null)
            {
                AddClause("groupby", sql, parameters, joiner: " , ", prefix: "GROUP BY ", postfix: "\n");
                return this;
            }

            public SelectBuilder Having(string sql, dynamic parameters = null)
            {
                AddClause("having", sql, parameters, joiner: "\nAND ", prefix: "HAVING ", postfix: "\n");
                return this;
            }

            public SelectBuilder Parameters(dynamic parameters)
            {
                AddClause("--parameters", "", parameters, "");
                return this;
            }

            public CommandDefinition ToCommand()
            {
                return ToCommand(0);
            }

            public CommandDefinition ToCommand(int take)
            {
                var sb = new StringBuilder();

                var param = new DynamicParameters();

                foreach (var p in data.SelectMany(o => o.Value.Where(p => p != null)))
                {
                    param.AddDynamicParams(p.Parameters);
                }

                sb.Append("SELECT ");

                sb.Append(data["select"]);
                sb.AppendFormat("FROM {0}", data["from"]);
                if (data.ContainsKey("where"))
                    sb.Append(data["where"]);
                if (data.ContainsKey("groupby"))
                    sb.Append(data["groupby"]);
                if (data.ContainsKey("having"))
                    sb.Append(data["having"]);
                if (data.ContainsKey("orderby"))
                    sb.Append(data["orderby"]);
                if (take > 0)
                {
                    sb.Append(" limit " + take);
                }
                return new CommandDefinition(sb.ToString(), param);
            }

            public CommandDefinition ToCommand(int page, int size)
            {
                if (page < 1)
                {
                    page = 1;
                }
                if (size < 1)
                {
                    size = 20;
                }

                var sb = new StringBuilder();

                var param = new DynamicParameters();

                foreach (var p in data.SelectMany(o => o.Value.Where(p => p != null)))
                {
                    param.AddDynamicParams(p.Parameters);
                }

                if (data.ContainsKey("having") && !data.ContainsKey("groupby"))
                {
                    throw new ArgumentException("group by 参数未提供");
                }

                var offset = (page - 1) * size;


                sb.AppendFormat("SELECT COUNT(0) as Count FROM {0}", data["from"]);
                if (data.ContainsKey("where"))
                    sb.Append(data["where"]);
                if (data.ContainsKey("groupby"))
                    sb.Append(data["groupby"]);
                if (data.ContainsKey("having"))
                    sb.Append(data["having"]);
                sb.Append(";\n");

                sb.Append("SELECT ");
                sb.Append(data["select"]);
                sb.AppendFormat("FROM {0}", data["from"]);
                if (data.ContainsKey("where"))
                    sb.Append(data["where"]);
                if (data.ContainsKey("groupby"))
                    sb.Append(data["groupby"]);
                if (data.ContainsKey("having"))
                    sb.Append(data["having"]);
                if (data.ContainsKey("orderby"))
                    sb.Append(data["orderby"]);
                sb.Append(" limit " + size + " offset " + offset);
                sb.Append(";\n");
                return new CommandDefinition(sb.ToString(), param);
            }
        }

        public class InsertBuilder
        {
            private readonly string table;

            private readonly IDictionary<string, object> columns = new Dictionary<string, object>();

            internal InsertBuilder(string table)
            {
                this.table = table;
            }

            public InsertBuilder Column(string name, object value)
            {
                columns.Add(name, value);
                return this;
            }

            public CommandDefinition ToCommand(bool identity = false, IDbTransaction transaction = null, int? timeout = null)
            {
                var names = string.Join(",", columns.Keys);
                var panmes = string.Join(",", columns.Keys.Select(o => "@" + o));
                var sb = new StringBuilder("INSERT INTO ")
                    .AppendFormat("{0} ({1})VALUES({2})", table, names, panmes);
                if (identity)
                {
                    sb.AppendLine(";SELECT @@IDENTITY Id;");
                }

                var param = new DynamicParameters(columns.ToDictionary(x => x.Key, x => x.Value));

                return new CommandDefinition(sb.ToString(), param, transaction, timeout, CommandType.Text, CommandFlags.None);
            }
        }

        public class UpdateBuilder
        {
            private readonly string table;
            private readonly HashSet<string> statics = new HashSet<string>();

            private readonly IDictionary<string, object> columns = new Dictionary<string, object>();

            private readonly IDictionary<string, dynamic> wheres = new Dictionary<string, dynamic>();

            internal UpdateBuilder(string table)
            {
                this.table = table;
            }

            public UpdateBuilder Column(string sql)
            {
                statics.Add(sql);
                return this;
            }

            public UpdateBuilder Column(string name, object value)
            {
                columns.Add(name, value);
                return this;
            }

            public UpdateBuilder Column(bool condition, string sql)
            {
                if (condition)
                {
                    statics.Add(sql);
                }
                return this;
            }
            public UpdateBuilder Column(bool condition,string name, object value)
            {
                if (condition)
                {
                    columns.Add(name, value);
                }
                return this;
            }

            public UpdateBuilder Where(string sql, dynamic param = null)
            {
                wheres.Add(sql, param);
                return this;
            }

            public UpdateBuilder Where(bool condition, string sql, dynamic param = null)
            {
                if (condition)
                {
                    wheres.Add(sql, param);
                }
                return this;
            }

            public CommandDefinition ToCommand(IDbTransaction transaction = null, int? timeout = null)
            {
                if (statics.Count + columns.Count < 1)
                {
                    throw new ArgumentException("更新参数未提供");
                }
                if (wheres.Count == 0)
                {
                    throw new ArgumentException("更新条件未提供");
                }
                var sb = new StringBuilder("UPDATE ")
                    .AppendLine(table)
                    .Append("SET ");

                var i = 0;
                foreach (var o in statics)
                {
                    sb.AppendFormat(i == 0 ? "{0}\n" : "    ,{0}\n", o);
                    i++;
                }
                i = 0;
                foreach (var o in columns)
                {
                    sb.AppendFormat(i == 0 && statics.Count == 0 ? "{0}=@{1}\n" : "    ,{0}=@{1}\n", o.Key, o.Key);
                    i++;
                }
                var param = new DynamicParameters(columns);

                sb.Append("WHERE ");

                i = 0;
                foreach (var o in wheres)
                {
                    sb.AppendFormat(i == 0 ? "{0}\n" : "    AND {1}\n", o.Key, o.Key);
                    param.AddDynamicParams(o.Value);
                    i++;
                }

                return new CommandDefinition(sb.ToString(), param, transaction, timeout, CommandType.Text, CommandFlags.None);
            }
        }

        public class DeleteBuilder
        {
            private readonly string table;

            private readonly IDictionary<string, dynamic> wheres = new Dictionary<string, dynamic>();

            internal DeleteBuilder(string table)
            {
                this.table = table;
            }

            public DeleteBuilder Where(string sql, dynamic param = null)
            {
                wheres.Add(sql, param);
                return this;
            }

            public DeleteBuilder Where(bool condition, string sql, dynamic param = null)
            {
                if (condition)
                {
                    wheres.Add(sql, param);
                }
                return this;
            }

            public CommandDefinition ToCommand(IDbTransaction transaction = null, int? timeout = null)
            {
                if (wheres.Count == 0)
                {
                    throw new ArgumentException("删除条件未提供");
                }
                var sb = new StringBuilder("DELETE FROM ")
                    .AppendLine(table);

                var param = new DynamicParameters();

                sb.Append("WHERE ");
                var i = 0;
                foreach (var o in wheres)
                {
                    sb.AppendFormat(i == 0 ? "{0} " : "AND {0} ", o.Key);

                    param.AddDynamicParams(o.Value);
                    i++;
                }


                return new CommandDefinition(sb.ToString(), param, transaction, timeout, CommandType.Text, CommandFlags.None);
            }
        }

        public class RawBuilder
        {
            private readonly HashSet<string> set = new HashSet<string>();
            private readonly DynamicParameters parameters = new DynamicParameters();

            internal RawBuilder(string sql, dynamic param = null)
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    set.Add(sql);
                }
                if (param != null)
                {
                    parameters.AddDynamicParams(param);
                }
            }

            public RawBuilder Append(dynamic param)
            {
                return Append(null, param);
            }

            public RawBuilder Append(string sql, dynamic param = null)
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    set.Add(sql);
                }

                if (param != null)
                {
                    parameters.AddDynamicParams(param);
                }
                return this;
            }

            public RawBuilder Append(bool condition, string sql, dynamic param = null)
            {
                return condition ? Append(sql, param) : this;
            }

            public CommandDefinition ToCommand(IDbTransaction transaction = null, int? timeout = null)
            {
                var sb = new StringBuilder();

                foreach (var o in set)
                {
                    sb.AppendLine(o);
                }

                return new CommandDefinition(sb.ToString(), parameters, transaction, timeout, CommandType.Text, CommandFlags.None);
            }

        }

        public static SelectBuilder Select(string field)
        {
            return new SelectBuilder(field);
        }

        public static InsertBuilder Insert(string table)
        {
            return new InsertBuilder(table);
        }

        public static UpdateBuilder Update(string table)
        {
            return new UpdateBuilder(table);
        }

        public static DeleteBuilder Delete(string table)
        {
            return new DeleteBuilder(table);
        }

        public static RawBuilder Raw(string sql, dynamic param = null)
        {
            return new RawBuilder(sql, param);
        }
        public static RawBuilder Raw(dynamic param)
        {
            return new RawBuilder(null, param);
        }
    }
}