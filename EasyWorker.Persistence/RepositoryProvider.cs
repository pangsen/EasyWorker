namespace Worker.Persistence
{
    public static class RepositoryProvider
    {
        private static Repository Repository { get; set; }

        public static Repository GetRepository()
        {
            if (Repository == null)
            {
                Repository=new Repository("D:\\temp");
            }
            return Repository;
        }
    }
}