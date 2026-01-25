using Npgsql;

namespace _13.databese
{
    public static class Database
    {
        private static readonly NpgsqlDataSource DataSource;

        static Database()
        {
            const string host = "localhost";
            const int port = 5432;
            const string database = "kip";
            const string user = "postgres";
            const string password = "1234";
            
            var connectionString = $"Host={host};Port={port};Database={database};Username={user};Password={password};";
            DataSource = NpgsqlDataSource.Create(connectionString);
        }

        public static NpgsqlConnection GetConnection()
        {
            return DataSource.OpenConnection();
        }
    }
}
