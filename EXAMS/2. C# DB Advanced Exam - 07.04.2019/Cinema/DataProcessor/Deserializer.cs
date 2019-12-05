namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Cinema.Data.Models;
    using Cinema.Data.Models.Enums;
    using Cinema.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie 
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat 
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection 
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket 
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var moviesList = JsonConvert.DeserializeObject<List<MovieImportDto>>(jsonString);

            StringBuilder sb = new StringBuilder();

            foreach (var movieDto in moviesList)
            {
                if (!IsValid(movieDto) || !Enum.IsDefined(typeof(Genre), movieDto.Genre)
                    || context.Movies.Any(m => m.Title == movieDto.Title))
                {
                    sb.AppendLine(ErrorMessage);

                    continue;
                }


                Movie movie = new Movie
                {
                    Title = movieDto.Title,
                    Genre = (Genre)Enum.Parse(typeof(Genre), movieDto.Genre),
                    Duration = TimeSpan.ParseExact(movieDto.Duration, "c", null),
                    Rating = movieDto.Rating,
                    Director = movieDto.Director
                };


                context.Movies.Add(movie);

                sb.AppendLine($"Successfully imported {movie.Title} with genre {movie.Genre} and rating {movie.Rating:F2}!");

                context.SaveChanges();

            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var hallsList = JsonConvert.DeserializeObject<List<HallImportDto>>(jsonString);

            StringBuilder sb = new StringBuilder();

            List<Hall> halls = new List<Hall>();

            foreach (var hallDto in hallsList)
            {
                if (!IsValid(hallDto) || hallDto.Seats <= 0)
                {
                    sb.AppendLine(ErrorMessage);

                    continue;
                }

                Hall hall = new Hall
                {
                    Name = hallDto.Name,
                    Is3D = hallDto.Is3D,
                    Is4Dx = hallDto.Is4Dx,
                };

                List<Seat> hallSeats = new List<Seat>();

                for (int i = 0; i < hallDto.Seats; i++)
                {
                    Seat seat = new Seat
                    {
                        HallId = hall.Id,
                        Hall = hall
                    };

                    hallSeats.Add(seat);
                }

                context.Seats.AddRange(hallSeats);

                hall.Seats = hallSeats;

                halls.Add(hall);

                string projectionType = "";

                if (hall.Is3D)
                {
                    if (hall.Is4Dx)
                    {
                        projectionType = "4Dx/3D";
                    }
                    else
                    {
                        projectionType = "3D";
                    }
                }
                else
                {
                    if (hall.Is4Dx)
                    {
                        projectionType = "4Dx";
                    }
                    else
                    {
                        projectionType = "Normal";
                    }
                }

                sb.AppendLine(string.Format(SuccessfulImportHallSeat, hall.Name, projectionType, hall.Seats.Count));

            }

            context.Halls.AddRange(halls);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            XmlSerializer ser = new XmlSerializer(typeof(ProjectionImportDto[]), new XmlRootAttribute("Projections"));

            var projectionsDto = (ProjectionImportDto[])ser.Deserialize(new StringReader(xmlString));

            List<Projection> projections = new List<Projection>();

            StringBuilder sb = new StringBuilder();

            foreach (var projectionDto in projectionsDto)
            {
                var movie = context.Movies.FirstOrDefault(m => m.Id == projectionDto.MovieId);

                var hall = context.Halls.FirstOrDefault(h => h.Id == projectionDto.HallId);

                if (!IsValid(projectionDto) || hall == null || movie == null)
                {
                    sb.AppendLine(ErrorMessage);

                    continue;
                }

                Projection projection = new Projection
                {
                    HallId = projectionDto.HallId,
                    Hall = hall,
                    MovieId = projectionDto.MovieId,
                    Movie = movie,
                    DateTime = DateTime.ParseExact(projectionDto.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                };

                projections.Add(projection);

                sb.AppendLine(string.Format(SuccessfulImportProjection, projection.Movie.Title, projection.DateTime.ToString("MM/dd/yyyy")));
            }

            context.Projections.AddRange(projections);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            XmlSerializer ser = new XmlSerializer(typeof(CustomerImportDto[]), new XmlRootAttribute("Customers"));

            var customersDto = (CustomerImportDto[])ser.Deserialize(new StringReader(xmlString));

            List<Customer> customers = new List<Customer>();

            StringBuilder sb = new StringBuilder();

            foreach (var customerDto in customersDto)
            {
                if (!IsValid(customerDto))
                {
                    sb.AppendLine(ErrorMessage);

                    continue;
                }

                List<Ticket> validTickets = new List<Ticket>();

                bool areAllTicketsValid = true;

                foreach (var ticketDto in customerDto.Tickets)
                {
                    if (!IsValid(ticketDto) || !context.Projections.Any(p => p.Id == ticketDto.ProjectionId))
                    {
                        sb.AppendLine(ErrorMessage);

                        areAllTicketsValid = false;

                        break;
                    }
                }

                if (areAllTicketsValid)
                {
                    Customer customer = new Customer
                    {
                        FirstName = customerDto.FirstName,
                        LastName = customerDto.LastName,
                        Age = customerDto.Age,
                        Balance = customerDto.Balance
                    };

                    foreach (var ticketDto in customerDto.Tickets)
                    {
                        var projection = context.Projections.FirstOrDefault(p => p.Id == ticketDto.ProjectionId);

                        Ticket ticket = new Ticket
                        {
                            ProjectionId = ticketDto.ProjectionId,
                            Projection = projection,
                            Customer = customer,
                            CustomerId = customer.Id,
                            Price = ticketDto.Price
                        };

                        validTickets.Add(ticket);
                    }

                    customer.Tickets = validTickets;

                    context.Tickets.AddRange(validTickets);

                    customers.Add(customer);

                    sb.AppendLine(string.Format(SuccessfulImportCustomerTicket,customer.FirstName,
                        customer.LastName, customer.Tickets.Count ));
                }
                else
                {
                    continue;
                }
               
            }

            context.Customers.AddRange(customers);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(entity);

            var validationResult = new List<ValidationResult>();

            var result = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return result;
        }
    }
}