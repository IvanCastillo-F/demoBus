using BusProyectApi.Models.Entities;

namespace BusProyectApi.Data.Interfaces
{
    public interface IRouteRepository
    {
        Task<RouteInfo> CreateRouteAsync(RouteInfo route);
        Task DeleteRouteAsync(RouteInfo route);
        Task<IEnumerable<RouteInfo>> GetAllRoutesAsyc();
        Task<RouteInfo> GetRouteByIdAsyc(int id);
        Task UpdateRouteAsync(RouteInfo route);
        Task<bool> IsRouteDuplicateAsync(string origin, string destination);
        Task<bool> IsRouteDuplicateForOtherRouteAsync(string origin, string destination, int routeId);
        Task<(double Distance, List<string> Path)> GetFastestRouteAsync(string start, string end);
    }
}