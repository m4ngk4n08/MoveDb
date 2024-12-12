using MoveDb.Services.Data.Entities;

namespace MoveDb.Services.Services.IServices {
    public interface IApiService {
        Task<string> APIRequests(string url);
        Task<string?> GeminiApiRequest(GemeniRequest gemeniRequest);
    }
}
