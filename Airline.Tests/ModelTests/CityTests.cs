using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Airline.Models;

namespace Airline.Tests
{
  [TestClass]
  public class CityTests : IDisposable
  {
    public void Dispose()
    {
      Flight.DeleteAllJoin();
      Flight.DeleteAll();
      City.DeleteAll();
    }
    public CityTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=airline_planner_test;";
    }
    [TestMethod]
    public void Equals_ReturnsTrueForSameName_City()
    {
      //Arrange, Act
      City firstCity = new City("Seattle");
      City secondCity = new City("Seattle");

      //Assert
      Assert.AreEqual(firstCity, secondCity);
    }
    [TestMethod]
    public void Save_GetAllCities_Test()
    {
      //Arrange
      City newCity = new City("Tacoma");
      newCity.Save();

      //Act
      List<City> expectedResult = new List<City>{newCity};
      List<City> result = City.GetAllCities();

      //Assert
      CollectionAssert.AreEqual(expectedResult, result);
    }
    [TestMethod]
    public void Test_AddCity_AddsCityToCity()
    {
      //Arrange
      City testCity = new City("Tacoma");
      testCity.Save();

      Flight testFlight = new Flight("2018-07-16 16:45:00", "Tacoma", "New York", "On Time");
      testFlight.Save();

      Flight testFlight2 = new Flight("2018-07-16 18:25:00", "Tacoma", "Portland", "Delayed");
      testFlight2.Save();

      //Act
      testCity.AddFlight(testFlight);
      testCity.AddFlight(testFlight2);

      List<Flight> result = testCity.GetFlights();
      List<Flight> testList = new List<Flight>{testFlight, testFlight2};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
  }
}
