using System.Collections.Generic;
using Worker.Interface;

namespace Worker.Implementation
{
    public class DefaultHistoryMessageManager : IHistoryMessageManager
    {
        private readonly ILogger _logger;

        public DefaultHistoryMessageManager(ILogger logger)
        {
            _logger = logger;
        }
        public List<Messgae> GetAll()
        {
            _logger.Write("GetAllHistoryMessage");
            return null;
        }

        public void AddHistoryMessgae(Messgae messgae)
        {
            _logger.Write("AddHistoryMessgae");
        }

        public void Save()
        {
            _logger.Write("SaveHistoryMessgae");
        }
    }
}