namespace Airplane.Best.Routes.Domain.Messages
{
    public static class ExceptionMessages
    {
        public const string ExceptionGetAllRoutes = "RouteRepository: Exceção ao tentar chamar todas as rotas.";
        public const string ExceptionGetRouteById = "RouteRepository: Exceção ao tentar buscar rota por ID.";
        public const string ExceptionGetRoutesByOriginAndDestination = "RouteRepository: Exceção ao tentar buscar rotas por origem e destino.";
        public const string ExceptionGetBestRouteByOriginAndDestination = "RouteRepository: Exceção ao tentar buscar a melhor rota por origem e destino.";
        public const string ExceptionCreateRoute = "RouteRepository: Exceção ao tentar criar uma nova rota.";
        public const string ExceptionUpdateRoute = "RouteRepository: Exceção ao tentar atualizar uma rota.";
        public const string ExceptionDeleteRoute = "RouteRepository: Exceção ao tentar deletar uma rota.";
    }
}
