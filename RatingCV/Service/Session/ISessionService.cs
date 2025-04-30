using Microsoft.AspNetCore.Mvc;
using RatingCV.Model.session;

namespace RatingCV.Service.Session;

public interface ISessionService
{
    Task<session> AddSession([FromBody] sessionDto sessionDto);
    Task<session> DeleteSession(int sessionId);
    Task<List<session>> GetSession();
}