using log4net;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace connect.Pages
{
    public class IndexModel : PageModel
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(_connectionString))
            {
                Log.Error("ConnectionString is empty or incorrect!");
            }

            _configuration = configuration;
        }

        // Holds each row as a dictionary, where key = column name, value = column value
        public List<Dictionary<string, string>> Records { get; private set; } = new();

        // Holds the column names for creating dynamic headers
        public List<string> ColumnNames { get; private set; } = new();

        public void OnGet(string layoutid)
        {
            if (string.IsNullOrEmpty(layoutid))
            {
                Log.Error("No layoutid provided in the URL.");
                return;
            }

            string query = _configuration[$"Queries:{layoutid}"];

            if (string.IsNullOrEmpty(query))
            {
                Log.Error($"Query for layoutid '{layoutid}' not found in appsettings.json.");
                return;
            }

            ExecuteQuery(query);
        }

        private void ExecuteQuery(string query)
        {
            try
            {
                using (SqlConnection connection = new(_connectionString))
                {
                    connection.Open();
                    Log.Info($"Executing SQL query: {query}");

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                ColumnNames.Add(reader.GetName(i));
                            }

                            while (reader.Read())
                            {
                                var row = new Dictionary<string, string>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetName(i);
                                    string value = reader.IsDBNull(i) ? string.Empty : reader[i].ToString();
                                    row[columnName] = value;
                                }
                                Records.Add(row);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error executing SQL query: {ex.Message}");
                throw new ApplicationException($"Error executing SQL query: {ex.Message}");
            }
        }
    }
}
