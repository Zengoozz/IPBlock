using API.Models;

namespace API.Repositories
{
    public class ApiResponsesRepository
    {
        private readonly List<ApiResponseModel> _dataModels = new();

        public void Add(ApiResponseModel response)
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
        public List<ApiResponseModel> GetAll()
        {
            return _dataModels;
        }
    }
}
