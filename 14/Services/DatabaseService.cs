using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Dapper;
using Cinema.Models;

namespace Cinema.Services;

public class DatabaseService
{
    private readonly string _connectionString = "Host=localhost;Port=5434;Database=cinema;Username=cinema;Password=cinema123";

    public async Task InitializeAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(@"
            CREATE TABLE IF NOT EXISTS users (
                id SERIAL PRIMARY KEY,
                email VARCHAR(255) UNIQUE NOT NULL,
                password VARCHAR(255) NOT NULL,
                name VARCHAR(255) NOT NULL
            );

            CREATE TABLE IF NOT EXISTS movies (
                id SERIAL PRIMARY KEY,
                title VARCHAR(255) NOT NULL,
                description TEXT,
                image_url TEXT,
                rating DECIMAL(3,1),
                release_date DATE,
                age_rating INT
            );

            CREATE TABLE IF NOT EXISTS genres (
                id SERIAL PRIMARY KEY,
                name VARCHAR(100) NOT NULL
            );

            CREATE TABLE IF NOT EXISTS movie_genres (
                movie_id INT REFERENCES movies(id) ON DELETE CASCADE,
                genre_id INT REFERENCES genres(id) ON DELETE CASCADE,
                PRIMARY KEY (movie_id, genre_id)
            );

            CREATE TABLE IF NOT EXISTS halls (
                id SERIAL PRIMARY KEY,
                name VARCHAR(100) NOT NULL,
                classification VARCHAR(100)
            );

            CREATE TABLE IF NOT EXISTS sessions (
                id SERIAL PRIMARY KEY,
                movie_id INT REFERENCES movies(id) ON DELETE CASCADE,
                hall_id INT REFERENCES halls(id) ON DELETE CASCADE,
                datetime TIMESTAMP NOT NULL
            );

            CREATE TABLE IF NOT EXISTS seats (
                id SERIAL PRIMARY KEY,
                hall_id INT REFERENCES halls(id) ON DELETE CASCADE,
                row INT NOT NULL,
                number INT NOT NULL,
                price DECIMAL(10,2) NOT NULL
            );

            CREATE TABLE IF NOT EXISTS tickets (
                id SERIAL PRIMARY KEY,
                user_id INT REFERENCES users(id) ON DELETE CASCADE,
                session_id INT REFERENCES sessions(id) ON DELETE CASCADE,
                seat_id INT REFERENCES seats(id) ON DELETE CASCADE,
                UNIQUE(session_id, seat_id)
            );
        ");
    }

    public async Task SeedDataAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var movieCount = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM movies");
        if (movieCount > 0) return;

        await connection.ExecuteAsync(@"
            INSERT INTO genres (name) VALUES ('Драма'), ('Комедия'), ('Боевик'), ('Фантастика'), ('Ужасы');

            INSERT INTO halls (name, classification) VALUES
                ('Зал 1', 'VIP'),
                ('Зал 2', 'Стандарт'),
                ('Зал 3', 'IMAX');

            INSERT INTO movies (title, description, image_url, rating, release_date, age_rating) VALUES
                ('Побег из Шоушенка', 'Два заключенных подружились за несколько лет...', 'https://via.placeholder.com/300x450', 9.3, '1994-09-23', 16),
                ('Крестный отец', 'Стареющий патриарх криминальной династии...', 'https://via.placeholder.com/300x450', 9.2, '1972-03-24', 18),
                ('Темный рыцарь', 'Бэтмен сражается с Джокером...', 'https://via.placeholder.com/300x450', 9.0, '2008-07-18', 12),
                ('Список Шиндлера', 'История немецкого бизнесмена...', 'https://via.placeholder.com/300x450', 8.9, '1993-12-15', 16),
                ('Начало', 'Вор, крадущий секреты через сны...', 'https://via.placeholder.com/300x450', 8.8, '2010-07-16', 12);

            INSERT INTO movie_genres (movie_id, genre_id) VALUES
                (1, 1), (2, 1), (2, 3), (3, 3), (3, 4), (4, 1), (5, 3), (5, 4);
        ");

        for (int hallId = 1; hallId <= 3; hallId++)
        {
            for (int row = 1; row <= 10; row++)
            {
                for (int number = 1; number <= 15; number++)
                {
                    decimal price = hallId == 1 ? 1000 : hallId == 3 ? 800 : 500;
                    await connection.ExecuteAsync(
                        "INSERT INTO seats (hall_id, row, number, price) VALUES (@HallId, @Row, @Number, @Price)",
                        new { HallId = hallId, Row = row, Number = number, Price = price });
                }
            }
        }

        var movieIds = await connection.QueryAsync<int>("SELECT id FROM movies");
        foreach (var movieId in movieIds)
        {
            for (int hallId = 1; hallId <= 3; hallId++)
            {
                var baseDate = DateTime.Now.AddDays(1);
                for (int i = 0; i < 3; i++)
                {
                    await connection.ExecuteAsync(
                        "INSERT INTO sessions (movie_id, hall_id, datetime) VALUES (@MovieId, @HallId, @DateTime)",
                        new { MovieId = movieId, HallId = hallId, DateTime = baseDate.AddHours(i * 3) });
                }
            }
        }
    }

    public async Task<List<Movie>> GetMoviesAsync(string? search = null, string? sortBy = null)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var sql = "SELECT * FROM movies WHERE 1=1";

        if (!string.IsNullOrEmpty(search))
            sql += " AND LOWER(title) LIKE LOWER(@Search)";

        sql += sortBy switch
        {
            "title" => " ORDER BY title",
            "rating" => " ORDER BY rating DESC",
            _ => " ORDER BY id"
        };

        var result = await connection.QueryAsync<Movie>(sql, new { Search = $"%{search}%" });
        return result.Select(m => new Movie
        {
            Id = m.Id,
            Title = m.Title,
            Description = m.Description,
            ImageUrl = m.ImageUrl,
            Rating = m.Rating,
            ReleaseDate = m.ReleaseDate,
            AgeRating = m.AgeRating
        }).ToList();
    }

    public async Task<Movie?> GetMovieByIdAsync(int id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<Movie>(
            "SELECT * FROM movies WHERE id = @Id", new { Id = id });
    }

    public async Task<List<Genre>> GetMovieGenresAsync(int movieId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return (await connection.QueryAsync<Genre>(@"
            SELECT g.* FROM genres g
            JOIN movie_genres mg ON g.id = mg.genre_id
            WHERE mg.movie_id = @MovieId", new { MovieId = movieId })).ToList();
    }

    public async Task<List<(Session Session, Hall Hall)>> GetMovieSessionsAsync(int movieId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var result = await connection.QueryAsync<Session, Hall, (Session, Hall)>(@"
            SELECT s.*, h.* FROM sessions s
            JOIN halls h ON s.hall_id = h.id
            WHERE s.movie_id = @MovieId AND s.datetime > NOW()
            ORDER BY s.datetime",
            (session, hall) => (session, hall),
            new { MovieId = movieId },
            splitOn: "id");
        return result.ToList();
    }

    public async Task<List<(Seat Seat, bool IsBooked)>> GetSessionSeatsAsync(int sessionId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var result = await connection.QueryAsync<Seat, bool, (Seat, bool)>(@"
            SELECT s.*, CASE WHEN t.id IS NOT NULL THEN true ELSE false END as is_booked
            FROM seats s
            JOIN sessions sess ON s.hall_id = sess.hall_id
            LEFT JOIN tickets t ON t.seat_id = s.id AND t.session_id = @SessionId
            WHERE sess.id = @SessionId
            ORDER BY s.row, s.number",
            (seat, isBooked) => (seat, isBooked),
            new { SessionId = sessionId },
            splitOn: "is_booked");
        return result.ToList();
    }

    public async Task<Session?> GetSessionByIdAsync(int id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<Session>(
            "SELECT * FROM sessions WHERE id = @Id", new { Id = id });
    }

    public async Task<User?> LoginAsync(string email, string password)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM users WHERE email = @Email AND password = @Password",
            new { Email = email, Password = password });
    }

    public async Task<User?> RegisterAsync(string email, string password, string name)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var existingUser = await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM users WHERE email = @Email", new { Email = email });

        if (existingUser != null) return null;

        var id = await connection.ExecuteScalarAsync<int>(
            "INSERT INTO users (email, password, name) VALUES (@Email, @Password, @Name) RETURNING id",
            new { Email = email, Password = password, Name = name });

        return new User { Id = id, Email = email, Password = password, Name = name };
    }

    public async Task<bool> BookTicketAsync(int userId, int sessionId, int seatId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        try
        {
            await connection.ExecuteAsync(
                "INSERT INTO tickets (user_id, session_id, seat_id) VALUES (@UserId, @SessionId, @SeatId)",
                new { UserId = userId, SessionId = sessionId, SeatId = seatId });
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<(Ticket Ticket, Movie Movie, Session Session, Seat Seat, Hall Hall)>> GetUserTicketsAsync(int userId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var result = await connection.QueryAsync<Ticket, Movie, Session, Seat, Hall, (Ticket, Movie, Session, Seat, Hall)>(@"
            SELECT t.*, m.*, s.*, st.*, h.*
            FROM tickets t
            JOIN sessions s ON t.session_id = s.id
            JOIN movies m ON s.movie_id = m.id
            JOIN seats st ON t.seat_id = st.id
            JOIN halls h ON s.hall_id = h.id
            WHERE t.user_id = @UserId
            ORDER BY s.datetime DESC",
            (ticket, movie, session, seat, hall) => (ticket, movie, session, seat, hall),
            new { UserId = userId },
            splitOn: "id,id,id,id");
        return result.ToList();
    }
}
