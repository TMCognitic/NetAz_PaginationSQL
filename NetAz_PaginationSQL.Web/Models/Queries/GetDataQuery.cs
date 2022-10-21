using NetAz_PaginationSQL.Web.Models.Mappers;
using System.Data.SqlClient;
using Tools;

namespace NetAz_PaginationSQL.Web.Models.Queries
{
    public class GetDataQuery
    {
        public int Page { get; set; }
        public int LineCount { get; set; }
        public int PageCount { get; set; }

        public GetDataQuery(int page)
        {
            if (page < 0)
                throw new InvalidOperationException("The minimum page is 0");

            Page = page;
        }

        public GetDataQuery(int lineCount, int page)
            : this(page)
        {
            if (lineCount < 10)
                throw new InvalidOperationException("The minimum line count is 10");

            LineCount = lineCount;
        }
    }

    public class GetDataQueryHandler
    {
        private readonly Connection _connection;

        public GetDataQueryHandler(Connection connection)
        {
            _connection = connection;
        }

        public IEnumerable<DataDto> Execute(GetDataQuery query)
        {
            Connection connection = new Connection(SqlClientFactory.Instance, @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestPagination;Integrated Security=True;");

            Command command = new Command("SP_GetHistory", true);
            if(query.Page != 0)
                command.AddParameter("Page", query.Page);
            if(query.LineCount != 0)
                command.AddParameter("LineCount", query.LineCount);

            command.AddParameter("PageCount", -1, Direction.Output);

            IEnumerable<DataDto> datas = connection.ExecuteReader(command, (dr) => dr.ToData(), true);
            query.PageCount = (int)command["PageCount"]!;

            return datas;
        }
    }
}
