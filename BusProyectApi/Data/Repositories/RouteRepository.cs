using BusProyectApi.Data.Interfaces;
using BusProyectApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusProyectApi.Data.Repositories
{
    public class RouteRepository : IRouteRepository
    {
        private readonly ApplicationDBContext _context;
        public RouteRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RouteInfo>> GetAllRoutesAsyc()
        {
            var routes = await _context.routes.ToArrayAsync();
            return routes;
        }

        public async Task<RouteInfo> GetRouteByIdAsyc(int id)
        {
            return await _context.routes.FindAsync(id);
        }

        public async Task<RouteInfo> CreateRouteAsync(RouteInfo route)
        {
            _context.routes.Add(route);
            await _context.SaveChangesAsync();
            return route;
        }

        public async Task UpdateRouteAsync(RouteInfo route)
        {
            _context.routes.Update(route);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRouteAsync(RouteInfo route)
        {
            _context.routes.Remove(route);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsRouteDuplicateAsync(string origin, string destination)
        {
            return await _context.routes.AnyAsync(r => r.DeparturePlace == origin && r.ArrivingPlace == destination);
        }

        public async Task<bool> IsRouteDuplicateForOtherRouteAsync(string origin, string destination, int routeId)
        {
            return await _context.routes.AnyAsync(r => r.DeparturePlace == origin && r.ArrivingPlace == destination && r.Id != routeId);
        }

        public async Task<(double Distance, List<string> Path)> GetFastestRouteAsync(string start, string end)
        {
            var routes = await _context.routes.ToListAsync();

            if (!routes.Any())
                return (double.MaxValue, new List<string>());

            var nodes = routes.SelectMany(r => new[] { r.DeparturePlace, r.ArrivingPlace }).Distinct().ToHashSet();
            var distances = nodes.ToDictionary(node => node, node => double.MaxValue);
            var previousNodes = nodes.ToDictionary(node => node, node => (string)null);
            var priorityQueue = new SortedSet<(double Distance, string Node)>();
            var visited = new HashSet<string>();

            if (!nodes.Contains(start) || !nodes.Contains(end))
            {
                return (double.MaxValue, new List<string>());
            }

            distances[start] = 0;
            priorityQueue.Add((0, start));

            while (priorityQueue.Count > 0)
            {
                var (currentDistance, currentNode) = priorityQueue.Min;
                priorityQueue.Remove(priorityQueue.Min);

                if (currentNode == end)
                    break;

                if (visited.Contains(currentNode))
                    continue;

                visited.Add(currentNode);

                foreach (var route in routes.Where(r => r.DeparturePlace == currentNode || r.ArrivingPlace == currentNode))
                {
                    var neighbor = route.DeparturePlace == currentNode ? route.ArrivingPlace : route.DeparturePlace;
                    var totalDistance = currentDistance + route.Distance;

                    if (totalDistance < distances[neighbor])
                    {
                        priorityQueue.Remove((distances[neighbor], neighbor));
                        distances[neighbor] = totalDistance;
                        previousNodes[neighbor] = currentNode;
                        priorityQueue.Add((totalDistance, neighbor));
                    }
                }
            }

            var path = new List<string>();
            for (var at = end; at != null; at = previousNodes[at])
            {
                path.Add(at);
            }

            path.Reverse();

            return distances[end] == double.MaxValue ? (double.MaxValue, new List<string>()) : (distances[end], path);
        }
    }
}
