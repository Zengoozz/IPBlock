using System.Collections.Concurrent;
using System.Transactions;

namespace API.Repositories
{
    public class BlockedCountriesRepository
    {
        private readonly ConcurrentDictionary<string, int?> _blockedItems = new ConcurrentDictionary<string, int?>();
        private readonly ConcurrentDictionary<string, string> _countriesInfo = new ConcurrentDictionary<string, string>();
        
        //  1 for success // 2 for failed in temporal block // 3 for failed for non-temporal block
        public int Block(string code, string countryName = "", int? duration = null)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var blockedItem = _blockedItems.FirstOrDefault(i => i.Key == code); // Key is the countryCode

                    //DURATION: Null = is blocked with no duration // > 0 = blocked with specific duration // 0 is not blocked
                    if (blockedItem.Key != null) 
                    {
                        if(blockedItem.Value == null && duration == null) // Value is the duration
                        {
                            scope.Complete();
                            return 3;
                        }
                        else if (blockedItem.Value != null && duration != null)
                        {
                            scope.Complete();
                            return 2;
                        }
                        else
                        {
                            _blockedItems.AddOrUpdate(code, duration, (key, val) => val);

                            scope.Complete();
                            return 1;
                        }
                    }
                    else
                    {
                        _blockedItems.AddOrUpdate(code, duration, (key, val) => val);
                        _countriesInfo.AddOrUpdate(code, countryName, (key, val) => val);

                        scope.Complete();
                        return 1;
                    }
                }
                catch(Exception) {
                    scope.Dispose();
                    throw;
                }
            }
            

        }
        public bool UnBlock(string code)
        {
            var isBlocked = _blockedItems.Any(i => i.Key == code);

            var response = false;

            if (isBlocked)
            {
                response = _blockedItems.TryRemove(code, out _);
            }

            return response;
        }
        public List<string> GetPaginatedBlockedCountries(int pageNumber, int pageSize, string countryCode, string countryName)
        {
            var code = countryCode;
            if(countryName != "" && countryCode == "")
            {
                var countryInfo = _countriesInfo.FirstOrDefault(i => i.Value == countryName);
                code = countryInfo.Key;
            }

            return _blockedItems.Where(x => code != "" && x.Key == code)
                .Select(x => x.Key)
                .Skip((pageNumber) - 1 * pageSize)
                .Take(pageSize)
                .ToList();
        }
        public List<string> GetAllCountryCodesBlocked()
        {
            return _blockedItems.Select(i => i.Key).ToList();
        }
    }
}
