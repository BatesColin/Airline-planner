using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Airline.Models;

namespace Airline.Tests
{
  [TestClass]
  public class FlightTests : IDisposable
  {
    public void Dispose()
    {
      Flight.DeleteAllJoin();
      Flight.DeleteAll();
      Flight.DeleteAll();
    }
    public FlightTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=airline_planner_test;";
    }
    [TestMethod]
    public void Equals_ReturnsTrueForSameName_Flight()
    {
      //Arrange, Act
      Flight firstFlight = new Flight("2018-07-16 16:45:00", "Los Angeles", "New York", "On Time");
      Flight secondFlight = new Flight("2018-07-16 16:45:00", "Los Angeles", "New York", "On Time");

      //Assert
      Assert.AreEqual(firstFlight, secondFlight);
    }
    [TestMethod]
    public void Save_GetAllFlights_Test()
    {
      //Arrange
      Flight newFlight = new Flight("2018-07-16 18:25:00", "Denver", "Portland", "Delayed");
      newFlight.Save();

      //Act
      List<Flight> expectedResult = new List<Flight>{newFlight};
      List<Flight> result = Flight.GetAllFlights();

      //Assert
      CollectionAssert.AreEqual(expectedResult, result);
    }
  }
}
