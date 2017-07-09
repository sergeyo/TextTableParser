using System.Collections.Generic;

namespace TextTableParser
{
    public interface ITableToDtoConverter<T> where T: class, new()
    {
        IEnumerable<T> Convert(Table table);
    }
}
