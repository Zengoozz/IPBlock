using API.Models;

namespace API.Repositories
{
    public class ApiFullResponsesRepository
    {
        private readonly List<ApiResponseFullModel> _dataModels = new();

        public void Add(ApiResponseFullModel response)
        {
            try
            {
                _dataModels.Add(response);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<ApiResponseFullModel> GetAll()
        {
            return _dataModels;
        }
    }
}
