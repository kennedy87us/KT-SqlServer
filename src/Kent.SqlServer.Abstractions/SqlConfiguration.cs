namespace Kent.SqlServer.Abstractions
{
    /// <summary>
    ///     Configuration to connect SQL Server instance.
    /// </summary>
    public class SqlConfiguration
    {
        /// <summary>
        ///     Gets or sets database connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        private int commandTimeout;
        /// <summary>
        ///     Gets or sets the wait time (in seconds) before terminating the attempt to execute a command.
        /// </summary>
        public int CommandTimeout
        {
            get
            {
                if (commandTimeout <= 0) return 30;
                return commandTimeout;
            }
            set { commandTimeout = value; }
        }
    }
}