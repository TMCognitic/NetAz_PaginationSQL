using System.Data;

namespace NetAz_PaginationSQL
{
    static class DataRecordExtensions
    {
        internal static DataDto ToData(this IDataRecord record)
        {
            return new DataDto() { Id = (int)record["Id"], Value = (string)record["Value"], CreatedDate = (DateTime)record["CreatedDate"] };
        }
    }
}