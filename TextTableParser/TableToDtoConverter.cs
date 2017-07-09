using System;
using System.Collections.Generic;
using System.Linq;

namespace TextTableParser
{
    internal class TableToDtoConverter<T> : ITableToDtoConverter<T> where T : class, new()
    {
        private IDictionary<string, Action<T, string>> _propertySetterFunctions;
        private IList<string> _columnsList;

        internal TableToDtoConverter(IDictionary<string, Action<T, string>> propertySetterFunctions, IList<string> columnsList)
        {
            _propertySetterFunctions = propertySetterFunctions;
            _columnsList = columnsList;
        }

        public IEnumerable<T> Convert(Table table)
        {
            return table.Rows.Select(row =>
            {
                return ConvertRowToDto(table, row);
            });
        }

        private T ConvertRowToDto(Table table, string[] row)
        {
            var dto = new T();
            var columns = table.Columns ?? _columnsList;
            for (int i = 0; i < columns.Count; i++)
            {
                var setter = _propertySetterFunctions[columns[i]];
                setter(dto, row[i]);
            }
            return dto;
        }
    }
}
