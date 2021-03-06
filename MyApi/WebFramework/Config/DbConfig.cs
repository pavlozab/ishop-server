namespace Config
{
    public class DbConfig
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConnectionString => $"Server={Server};Port={Port};Database={Database};" +
                                          $"Username={Username};Password={Password};SSL Mode=Require;Trust Server Certificate=true";
    }
}