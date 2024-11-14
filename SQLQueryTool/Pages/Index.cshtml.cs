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
        private readonly string _getQuery;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(_connectionString))
            {
                Log.Error("ConnectionString is empty or incorrect!");
            }
            _getQuery = configuration["Queries:DefaultQuery"];
            if (string.IsNullOrEmpty(_getQuery))
            {
                Log.Error("DefaultQuery is empty or incorrect!");
            }
        }

        // Holds each row as a dictionary, where key = column name, value = column value
        public List<Dictionary<string, string>> Records { get; private set; } = new();

        // Holds the column names for creating dynamic headers
        public List<string> ColumnNames { get; private set; } = new();

        public void OnGet()
        {
            try
            {
                using (SqlConnection connection = new(_connectionString))
                {
                    connection.Open();
                    Log.Info($"Executing SQL query: {_getQuery}");

                    using (SqlCommand command = new SqlCommand(_getQuery, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            // Capture column names for header generation
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                ColumnNames.Add(reader.GetName(i));
                            }

                            // Read each row and add it as a dictionary to the Records list
                            while (reader.Read())
                            {
                                var row = new Dictionary<string, string>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetName(i);
                                    string? value = reader.IsDBNull(i) ? string.Empty : reader[i].ToString();
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
                throw new Exception($"Error executing SQL query: {ex}");
            }
        }
    }
}
