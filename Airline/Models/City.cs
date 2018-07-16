using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Airline.Models
{
  public class City
  {
    private int _cityId;
    private string _cityName;

    public City(string CityName, int CityId = 0)
    {
      _cityId = CityId;
      _cityName = CityName;
    }
    public int GetCityId()
    {
      return _cityId;
    }
    public string GetCityName()
    {
      return _cityName;
    }
    public override bool Equals(System.Object otherCity)
    {
      if(!(otherCity is City))
      {
        return false;
      }
      else
      {
        City newCity = (City) otherCity;
        bool idEquality = this.GetCityId().Equals(newCity.GetCityId());
        bool nameEquality = this.GetCityName().Equals(newCity.GetCityName());
        return (idEquality && nameEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetCityId().GetHashCode();
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO cities (name) VALUES (@name);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._cityName;
      cmd.Parameters.Add(name);

      cmd.ExecuteNonQuery();
      _cityId = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public static List<City> GetAllCities()
    {
      List<City> allCities = new List<City> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cities;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
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
    public void AddFlight(Flight newFlight)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO cities_flights (cities_id, flights_id) VALUES (@CityId, @FlightId);";

      MySqlParameter city_id = new MySqlParameter();
      city_id.ParameterName = "@CityId";
      city_id.Value =  _cityId;
      cmd.Parameters.Add(city_id);

      MySqlParameter flight_id = new MySqlParameter();
      flight_id.ParameterName = "@FlightId";
      flight_id.Value = newFlight.GetFlightId();
      cmd.Parameters.Add(flight_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }
    public List<Flight> GetFlights()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT flights.* FROM cities
                JOIN cities_flights ON (cities.id = cities_flights.cities_id)
                JOIN flights ON (cities_flights.flights_id = flights.id)
                WHERE cities.id = @CityId;";

            MySqlParameter citiesIdParameter = new MySqlParameter();
            citiesIdParameter.ParameterName = "@CityId";
            citiesIdParameter.Value = _cityId;
            cmd.Parameters.Add(citiesIdParameter);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Flight> allFlights = new List<Flight>{};

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
    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM cities;";
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
  }
}
