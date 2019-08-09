using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace TextTableParser
{
    public class TableToDtoConverterFactory
    {
        private static readonly Expression InvariantCulture =
            Expression.MakeMemberAccess(null, typeof(CultureInfo).GetMember("InvariantCulture").First());

        private static ConcurrentDictionary<Type, object> _parsersCache = new ConcurrentDictionary<Type, object>();

        private static Dictionary<Type, Func<ParameterExpression, Expression>> _converterFactoryMethods = new Dictionary<Type, Func<ParameterExpression, Expression>>();

        private static Expression CreateDirectConvertMethodCall(string method, ParameterExpression param) =>
            Expression.Call(typeof(Convert).GetMethod(method, new[] { typeof(string) }), param);

        private static Expression CreateDirectConvertMethodCallInvariantCulture(string method, ParameterExpression param) =>
            Expression.Call(typeof(Convert).GetMethod(method, new[] { typeof(string), typeof(IFormatProvider) }), param, InvariantCulture);

        private static Expression CreateIfNotEmtyConvertMethodCall<T>(string method, ParameterExpression param) where T: struct =>
            Expression.Condition(Expression.Call(typeof(string).GetMethod("IsNullOrEmpty"), param),
                                                     Expression.TypeAs(Expression.Constant((int?)null),
                                                                       typeof(T?)),
                                                     Expression.TypeAs(CreateDirectConvertMethodCall(method, param),
                                                                       typeof(T?)));

        private static Expression CreateIfNotEmtyConvertMethodCallInvariantCulture<T>(string method, ParameterExpression param) where T : struct =>
            Expression.Condition(Expression.Call(typeof(string).GetMethod("IsNullOrEmpty"), param),
                                             Expression.TypeAs(Expression.Constant((int?)null),
                                                               typeof(T?)),
                                             Expression.TypeAs(CreateDirectConvertMethodCallInvariantCulture(method, param),
                                                               typeof(T?)));


        static TableToDtoConverterFactory()
        {
            _converterFactoryMethods[typeof(string)] = param => param;

            _converterFactoryMethods[typeof(bool)] = param => CreateDirectConvertMethodCall("ToBoolean", param);
            _converterFactoryMethods[typeof(bool?)] = param => CreateIfNotEmtyConvertMethodCall<bool>("ToBoolean", param);
            _converterFactoryMethods[typeof(byte)] = param => CreateDirectConvertMethodCall("ToByte", param);
            _converterFactoryMethods[typeof(byte?)] = param => CreateIfNotEmtyConvertMethodCall<byte>("ToByte", param);
            _converterFactoryMethods[typeof(char)] = param => CreateDirectConvertMethodCall("ToChar", param);
            _converterFactoryMethods[typeof(char?)] = param => CreateIfNotEmtyConvertMethodCall<char>("ToChar", param);
            _converterFactoryMethods[typeof(DateTime)] = param => CreateDirectConvertMethodCall("ToDateTime", param);
            _converterFactoryMethods[typeof(DateTime?)] = param => CreateIfNotEmtyConvertMethodCall<DateTime>("ToDateTime", param);
            _converterFactoryMethods[typeof(decimal)] = param => CreateDirectConvertMethodCall("ToDecimal", param);
            _converterFactoryMethods[typeof(decimal?)] = param => CreateIfNotEmtyConvertMethodCall<decimal>("ToDecimal", param);
            _converterFactoryMethods[typeof(double)] = param => CreateDirectConvertMethodCallInvariantCulture("ToDouble", param);
            _converterFactoryMethods[typeof(double?)] = param => CreateIfNotEmtyConvertMethodCallInvariantCulture<double>("ToDouble", param);
            _converterFactoryMethods[typeof(short)] = param => CreateDirectConvertMethodCall("ToInt16", param);
            _converterFactoryMethods[typeof(short?)] = param => CreateIfNotEmtyConvertMethodCall<short>("ToInt16", param);
            _converterFactoryMethods[typeof(int)] = param => CreateDirectConvertMethodCall("ToInt32", param);
            _converterFactoryMethods[typeof(int?)] = param => CreateIfNotEmtyConvertMethodCall<int>("ToInt32", param);
            _converterFactoryMethods[typeof(long)] = param => CreateDirectConvertMethodCall("ToInt64", param);
            _converterFactoryMethods[typeof(long?)] = param => CreateIfNotEmtyConvertMethodCall<long>("ToInt64", param);
            _converterFactoryMethods[typeof(sbyte)] = param => CreateDirectConvertMethodCall("ToSByte", param);
            _converterFactoryMethods[typeof(sbyte?)] = param => CreateIfNotEmtyConvertMethodCall<sbyte>("ToSByte", param);
            _converterFactoryMethods[typeof(float)] = param => CreateDirectConvertMethodCallInvariantCulture("ToSingle", param);
            _converterFactoryMethods[typeof(float?)] = param => CreateIfNotEmtyConvertMethodCallInvariantCulture<float>("ToSingle", param);
            _converterFactoryMethods[typeof(ushort)] = param => CreateDirectConvertMethodCall("ToUInt16", param);
            _converterFactoryMethods[typeof(ushort?)] = param => CreateIfNotEmtyConvertMethodCall<ushort>("ToUInt16", param);
            _converterFactoryMethods[typeof(uint)] = param => CreateDirectConvertMethodCall("ToUInt32", param);
            _converterFactoryMethods[typeof(uint?)] = param => CreateIfNotEmtyConvertMethodCall<uint>("ToUInt32", param);
            _converterFactoryMethods[typeof(ulong)] = param => CreateDirectConvertMethodCall("ToUInt64", param);
            _converterFactoryMethods[typeof(ulong?)] = param => CreateIfNotEmtyConvertMethodCall<ulong>("ToUInt64", param);
        }

        public ITableToDtoConverter<T> GetParser<T>() where T: class, new()
        {
            return (ITableToDtoConverter<T>)_parsersCache.GetOrAdd(typeof(T), t => CreateTableToDtoConverter<T>());
        }

        private ITableToDtoConverter<T> CreateTableToDtoConverter<T>() where T : class, new()
        {
            var propertySetterFunctions = new Dictionary<string, Action<T, string>>();
            var columnsList = new List<string>();

            foreach (var property in typeof(T).GetProperties())
            {
                var dtoParam = Expression.Parameter(typeof(T), "dto");
                var valueParam = Expression.Parameter(typeof(string), "value");

                if (!_converterFactoryMethods.ContainsKey(property.PropertyType))
                {
                    throw new ArgumentOutOfRangeException($"Type { typeof(T).FullName } contains property { property.Name } of unsupported type { property.PropertyType }.");
                }

                var valueExpr = _converterFactoryMethods[property.PropertyType](valueParam);

                var assignExpr = Expression.Assign(Expression.MakeMemberAccess(dtoParam, property),
                                                   valueExpr);
                propertySetterFunctions[property.Name] = Expression.Lambda<Action<T, string>>(assignExpr, dtoParam, valueParam).Compile();
                columnsList.Add(property.Name);
            }

            return new TableToDtoConverter<T>(propertySetterFunctions, columnsList);
        }
    }
}
