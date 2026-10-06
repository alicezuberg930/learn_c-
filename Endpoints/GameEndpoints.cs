using test.dtos;

namespace test.Endpoints;

public static class GameEndpoints
{
    public static readonly string getGameByIdName = "GetGameById";

    private static List<GameResponseDto> games = [
        new GameResponseDto { Id = 1, Name = "Game 1", Price = 59.99m, Genre = "Action", ReleaseDate = new DateOnly(2023, 5, 15) },
        new GameResponseDto { Id = 2, Name = "Game 2", Price = 49.99m, Genre = "Adventure", ReleaseDate = new DateOnly(2022, 10, 20) },
        new GameResponseDto { Id = 3, Name = "Game 3", Price = 39.99m, Genre = "RPG", ReleaseDate = new DateOnly(2021, 8, 5) },
        new GameResponseDto { Id = 4, Name = "Game 4", Price = 29.99m, Genre = "Strategy", ReleaseDate = new DateOnly(2020, 3, 12) },
        new GameResponseDto { Id = 5, Name = "Game 5", Price = 19.99m, Genre = "Simulation", ReleaseDate = new DateOnly(2019, 11, 30) }
    ];

    public static void MapGameEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games").AddEndpointFilter(async (context, next) =>
        {
            var httpContext = context.HttpContext;
            var request = httpContext.Request;

            // Log the request details
            Console.WriteLine($"Request: {request.Method} {request.Path}");

            // Call the next filter or endpoint
            var result = await next(context);

            // Log the response details
            Console.WriteLine($"Response: {result?.GetType().Name}");

            return result;
        });

        group.MapGet("/", () => Results.Ok(games));

        group.MapGet("/{id}", (int id) => games.Find(game => game.Id == id) is GameResponseDto game ? Results.Ok(game) : Results.NotFound()).WithName(getGameByIdName);

        group.MapPost("/", (CreateGameRequestDto dto) =>
        {
            var newGame = new GameResponseDto
            {
                Id = games.Count + 1,
                Name = dto.Name,
                Genre = dto.Genre,
                Price = dto.Price,
                ReleaseDate = dto.ReleaseDate
            };
            games.Add(newGame);
            return Results.CreatedAtRoute(getGameByIdName, new { id = newGame.Id }, newGame);
        });

        group.MapPut("/{id}", (int id, CreateGameRequestDto dto) =>
        {
            var game = games.Find(g => g.Id == id);
            if (game is null)
                return Results.NotFound();

            game.Name = dto.Name;
            game.Genre = dto.Genre;
            game.Price = dto.Price;
            game.ReleaseDate = dto.ReleaseDate;

            return Results.Ok(game);
        });

        group.MapDelete("/{id}", (int id) =>
        {
            var game = games.Find(g => g.Id == id);
            if (game is null)
                return Results.NotFound();

            games.Remove(game);
            return Results.NoContent();
        });

    }
}