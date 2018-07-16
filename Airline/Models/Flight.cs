using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Airline.Models
{
  public class Flight
  {
    private int _flightId;
    private string _flightTime;
    private string _departureCity;
    private string _arrivalCity;
    private string _flightStatus;


    public Flight(string FlightTime, string DepartureCity, string ArrivalCity, string FlightStatus, int FlightId = 0)
    {
      _flightId = FlightId;
      _flightTime = FlightTime;
      _departureCity = DepartureCity;
      _arrivalCity = ArrivalCity;
      _flightStatus = FlightStatus;
    }
    public int GetFlightId()
    {
      return _flightId;
    }
    public string GetFlightTime()
    {
      return _flightTime;
    }
    public string GetDepartureCity()
    {
      return _departureCity;
    }
    public string GetArrivalCity()
    {
      return _arrivalCity;
    }
    public string GetFlightStatus()
    {
      return _flightStatus;
    }
    public override bool Equals(System.Object otherFlight)
    {
      if(!(otherFlight is Flight))
      {
        return false;
      }
      else
      {
        Flight newFlight = (Flight) otherFlight;
        bool idEquality = this.GetFlightId().Equals(newFlight.GetFlightId());
        bool timeEquality = this.GetFlightTime().Equals(newFlight.GetFlightTime());
        bool departureEquality = this.GetDepartureCity().Equals(newFlight.GetDepartureCity());
        bool arrivalEquality = this.GetArrivalCity().Equals(newFlight.GetArrivalCity());
        bool statusEquality = this.GetFlightStatus().Equals(newFlight.GetFlightStatus());
        return (idEquality && timeEquality && departureEquality && arrivalEquality && statusEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetFlightId().GetHashCode();
    }
    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM flights;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public static void DeleteAllJoin()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM cities_flights;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO flights (departure_time, departure_city, arrival_destination, flight_status) VALUES (@time, @departureCity, @arrivalCity, @flightStatus);";

      cmd.Parameters.Add(new MySqlParameter("@time", _flightTime));
      cmd.Parameters.Add(new MySqlParameter("@departureCity", _departureCity));
      cmd.Parameters.Add(new MySqlParameter("@arrivalCity", _arrivalCity));
      cmd.Parameters.Add(new MySqlParameter("@flightStatus", _flightStatus));

      cmd.ExecuteNonQuery();
      _flightId = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public static List<Flight> GetAllFlights()
    {
      List<Flight> allFlights = new List<Flight> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM flights;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int FlightId = rdr.GetInt32(0);
        string FlightTime = rdr.GetString(1);
        string DepartureCity = rdr.GetString(2);
        string ArrivalCity = rdr.GetString(3);
        string FlightStatus = rdr.GetString(4);
        Flight newFlight = new Flight(FlightTime, DepartureCity, ArrivalCity, FlightStatus, FlightId);
        allFlights.Add(newFlight);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allFlights;
    }
    public void AddCity(City newCity)
       {
           MySqlConnection conn = DB.Connection();
           conn.Open();
           var cmd = conn.CreateCommand() as MySqlCommand;
           cmd.CommandText = @"INSERT INTO cities_flights (cities_id, flights_id) VALUES (@CityId, @FlightId);";

           MySqlParameter city_id = new MySqlParameter();
           city_id.ParameterName = "@CityId";
           city_id.Value = newCity.GetCityId();
           cmd.Parameters.Add(city_id);

           MySqlParameter flight_id = new MySqlParameter();
           flight_id.ParameterName = "@FlightId";
           flight_id.Value = _flightId;
           cmd.Parameters.Add(flight_id);

           cmd.ExecuteNonQuery();
           conn.Close();
           if (conn != null)
           {
               conn.Dispose();
           }
       }
       public List<City> GetCities()
           {
               MySqlConnection conn = DB.Connection();
               conn.Open();
               MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
               cmd.CommandText = @"SELECT * FROM flights
                   JOIN cities_flights ON (flights.id = citiess_flights.flights_id)
                   JOIN flights ON (cities_flights.flight_id = flights.id)
                   WHERE flights.id = @FlightId;";

               MySqlParameter flightsIdParameter = new MySqlParameter();
               flightsIdParameter.ParameterName = "@FlightId";
               flightsIdParameter.Value = _flightId;
               cmd.Parameters.Add(flightsIdParameter);

               MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
               List<City> allCities = new List<City>{};

               while(rdr.Read())
               {
                 int CityId = rdr.GetInt32(0);
                 string CityName = rdr.GetString(1);
                 City newCity = new City(CityName, CityId);
                 allCities.Add(newCity);
               }
               conn.Close();
               if (conn != null)
               {
                   conn.Dispose();
               }
               return allCities;
           }

  }
}
