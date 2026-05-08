using System.Data;

namespace App.Infra.Extensions;
public static class DataReaderExtensions
{
    public static T? GetNullable<T>(this IDataReader reader, string column)
    {
        var ordinal = reader.GetOrdinal(column);
        if (reader.IsDBNull(ordinal))
            return default;

        return (T)reader.GetValue(ordinal);
    }
}
