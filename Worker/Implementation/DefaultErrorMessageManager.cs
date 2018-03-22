using System.Collections.Generic;
using Worker.Interface;

namespace Worker.Implementation
{
    public class DefaultErrorMessageManager : IErrorMessageManager
    {
        private readonly ILogger _logger;

        public DefaultErrorMessageManager(ILogger logger)
        {
            _logger = logger;
        }
        public void AddErrorMessgae(Messgae messgae)
        {
            _logger.Write("SaveErrorMessgae");
        }

        public List<Messgae> GetAll()
        {
            _logger.Write("GetAllErrorMessages");
            return null;
        }

        public void Save()
        {
            _logger.Write("SaveErrorMessgae");
        }
    }
}