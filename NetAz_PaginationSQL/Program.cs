using System.Data.SqlClient;
using Tools;

namespace NetAz_PaginationSQL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            {
                //Connection connection = new Connection(SqlClientFactory.Instance, @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestPagination;Integrated Security=True;");

                //Command command = new Command("SP_GetHistory", true);
                //command.AddParameter("PageCount", -1, Direction.Output);

                //IEnumerable<DataDto> datas = connection.ExecuteReader(command, (dr) => dr.ToData(), true);
                //Console.WriteLine($"Pages Count : {command["PageCount"]}");
                //DisplayData(datas);

                //Command commandPage9 = new Command("SP_GetHistory", true);
                //commandPage9.AddParameter("Page", 9);
                //commandPage9.AddParameter("PageCount", -1, Direction.Output);

                //datas = connection.ExecuteReader(commandPage9, (dr) => dr.ToData(), true);
                //DisplayData(datas);

                //Command commandPage2Line50 = new Command("SP_GetHistory", true);
                //commandPage2Line50.AddParameter("LineCount", 50);
                //commandPage2Line50.AddParameter("Page", 2);
                //commandPage2Line50.AddParameter("PageCount", -1, Direction.Output);

                //datas = connection.ExecuteReader(commandPage2Line50, (dr) => dr.ToData(), true);
                //DisplayData(datas);
            }

            {
                Connection connection = new Connection(SqlClientFactory.Instance, @"Data Source=DESKTOP-BRIAREO\SQL2019DEV;Initial Catalog=DbSlide;Integrated Security=True;");
                Command command = new Command("SELECT DISTINCT CONVERT(NCHAR, UPPER(LEFT(Last_Name, 1))) AS Initial FROM Student");
                IEnumerable<char> chars = connection.ExecuteReader(command, dr => ((string)dr["Initial"])[0]);
                string values = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                ConsoleColor defaultColor = Console.ForegroundColor;

                foreach (char c in values)
                {
                    if(chars.Contains(c))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($" {c}");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($" {c}");
                    }                    
                }

                Console.ForegroundColor = defaultColor;
            }
        }

        private static void DisplayData(IEnumerable<DataDto> datas)
        {
            Console.WriteLine(new string('-', 16) + " Datas " + new string('-', 16));
            foreach(DataDto data in datas)
            {
                Console.WriteLine($"{data.Id:D3} : {data.Value} ({data.CreatedDate:yyyy/MM/dd})");
            }
            Console.WriteLine(new string('-', 39));
            Console.WriteLine();
        }
    }
}