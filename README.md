# MovieBE - Movie Booking Backend API

This is the backend API for the Movie Booking System, built with ASP.NET Core and Entity Framework Core.

## Project Structure

- **Controllers**: Contains API controllers for different entities
- **Models2**: Contains Entity Framework models representing the database entities
- **Properties**: Contains launch settings and configuration

## API Endpoints

### Movie Controller (`/api/Movie`)

- `GET /api/Movie` - Get all movies
- `GET /api/Movie/{id}` - Get movie by ID
- `POST /api/Movie` - Create a new movie
- `PUT /api/Movie/{id}` - Update a movie
- `DELETE /api/Movie/{id}` - Delete a movie
- `GET /api/Movie/genres` - Get all unique genres
- `GET /api/Movie/languages` - Get all unique languages

### Theatre Controller (`/api/Theatre`)

- `GET /api/Theatre` - Get all theatres (includes screens)
- `GET /api/Theatre/{id}` - Get theatre by ID
- `POST /api/Theatre` - Create a new theatre
- `PUT /api/Theatre/{id}` - Update a theatre
- `DELETE /api/Theatre/{id}` - Delete a theatre

### Show Controller (`/api/Show`)

- `GET /api/Show` - Get all shows (includes movie, screen, and theatre details)
- `GET /api/Show/{id}` - Get show by ID
- `GET /api/Show/movie/{movieId}` - Get all shows for a specific movie
- `POST /api/Show` - Create a new show
- `PUT /api/Show/{id}` - Update a show
- `DELETE /api/Show/{id}` - Delete a show

### Booking Controller (`/api/Booking`)

- `GET /api/Booking` - Get all bookings (includes user, show, movie, theatre details)
- `GET /api/Booking/{id}` - Get booking by ID
- `GET /api/Booking/user/{userId}` - Get all bookings for a specific user
- `POST /api/Booking` - Create a new booking
- `PUT /api/Booking/{id}` - Update a booking
- `DELETE /api/Booking/{id}` - Delete a booking

### User Controller (`/api/User`)

- `GET /api/User` - Get all users (admin only)
- `GET /api/User/{id}` - Get user by ID
- `POST /api/User/login` - User login
- `POST /api/User/register` - User registration
- `PUT /api/User/{id}` - Update user information
- `DELETE /api/User/{id}` - Delete a user

## Authentication

### Login Request
```json
{
  "email": "user@example.com",
  "password": "password123"
}
```

### Register Request
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "password": "password123",
  "age": 25,
  "phoneNumber": "+1234567890"
}
```

## Sample Data Structures

### Movie
```json
{
  "movieId": "M0001",
  "name": "Sample Movie",
  "language": "English",
  "genre": "Action",
  "targetAudience": "General",
  "duration": 120,
  "releaseDate": "2024-01-01",
  "posterUrl": "https://example.com/poster.jpg",
  "description": "A sample movie description"
}
```

### Theatre
```json
{
  "theatreId": "T0001",
  "nameOfTheatre": "Grand Cinema",
  "noOfScreens": 5,
  "area": "Downtown"
}
```

### Show
```json
{
  "showId": "S000000001",
  "showTime": "14:30:00",
  "showDate": "2024-01-15",
  "seatsRemainingGold": 50,
  "seatsRemainingSilver": 100,
  "classCostGold": 15,
  "classCostSilver": 10,
  "screenId": "SC001",
  "movieId": "M0001"
}
```

### Booking
```json
{
  "bookingId": "B000000001",
  "noOfTickets": 2,
  "totalCost": 30,
  "cardNumber": "1234567890123456",
  "nameOnCard": "John Doe",
  "bookingDate": "2024-01-10T10:30:00",
  "bookingStatus": "Confirmed",
  "userId": "U1001",
  "showId": "S000000001"
}
```

## Configuration

### Database Connection

Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MovieBookingDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

## CORS Configuration

The API is configured to allow all origins, methods, and headers for development purposes. You should restrict this in production.

## Running the Application

1. Make sure you have .NET 9.0 SDK installed
2. Update the database connection string in `appsettings.json`
3. Run the application:
   ```
   dotnet run
   ```
4. The API will be available at `https://localhost:5001` (or check the launch settings)

## Frontend Integration

The API endpoints are designed to work with the frontend Movie booking application. The frontend should make requests to:

- Base URL: `http://localhost:5213/api` (as configured in the frontend `api.js`)
- All endpoints return JSON responses
- Error responses include descriptive messages

## Notes

- User passwords are hashed using SHA256
- Default user role is "USER1" for new registrations
- Booking status defaults to "Confirmed"
- All API responses include proper HTTP status codes and error messages
- The API includes null safety checks to prevent runtime errors

## Error Handling

All endpoints include comprehensive error handling and return appropriate HTTP status codes:

- 200: Success
- 201: Created (for POST requests)
- 400: Bad Request (validation errors)
- 401: Unauthorized (login failures)
- 404: Not Found (resource doesn't exist)
- 409: Conflict (duplicate resources)
- 500: Internal Server Error (server-side errors)
