using MoveDb.Services.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoveDb.Services.IServices {
    public interface IHomeService {

        Task<IEnumerable<Result>> Get();
        Task<List<ResultResponse>> GetMovieSuggestion(string message);

        Task<ResultResponse> GetMovieDetails(int id);
    }
}
