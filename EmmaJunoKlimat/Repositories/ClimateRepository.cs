using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;


namespace EmmaJunoKlimat.Repositories
{
    public class ClimateRepository
    {
        private static readonly string connectionString = "Server=localhost;Port=5432;Database=KlimatobservationerJunoEmma2.0;User ID = postgres; Password= d1zz1e";
        
        #region Read
        /// <summary>
        /// Gets an observer from database
        /// </summary>
        /// <param name="id">Unique primary key</param>
        /// <returns>Observer</returns>
        /// 
        //public static List<Observer> observernames = new List<Observer>();

        Observation observation = new Observation();
        Category category = new Category();
        public Observer GetObserver(int id)
        {
            string stmt = "SELECT * FROM Observer WHERE id = @id";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            command.Parameters.AddWithValue("id", id);

            using var reader = command.ExecuteReader();

            Observer observerName = null;
            while (reader.Read())
            {
                observerName = new Observer
                {
                    Id = (int)reader["id"],
                    Firstname = Convert.IsDBNull(reader["firstname"]) ? null : (string)reader["firstname"],
                    Lastname = Convert.IsDBNull(reader["lastname"]) ? null : (string)reader["lastname"]
                };
            }
            return observerName;
        }

        public List<Observer> GetObservers()
        {
            string stmt = "SELECT * FROM Observer ORDER BY Lastname";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var command = new NpgsqlCommand(stmt, conn);
            using var reader = command.ExecuteReader();
            var observers = new List<Observer>();
            Observer observerName = null;

            while (reader.Read())
            {
                observerName = new Observer
                {
                    Id = (int)reader["id"],
                    Firstname = Convert.IsDBNull(reader["firstname"]) ? null : (string)reader["firstname"],
                    Lastname = Convert.IsDBNull(reader["lastname"]) ? null : (string)reader["lastname"]
                };
                observers.Add(observerName);
            }
            return observers;
        }
        #endregion

        //Lägger till en observer i databasen. Skickar med firstname och lastname eftersom id sätts automatiskt.
        public void AddObserver(Observer observer)
        {
            string stmt = "INSERT INTO observer (firstname, lastname) VALUES (@firstname, @lastname)";
            
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            command.Parameters.AddWithValue("firstname", observer.Firstname);
            command.Parameters.AddWithValue("lastname", observer.Lastname);
            command.ExecuteNonQuery();
        }

        //Tar bort en observer från databsen, där id stämmer överens med id:t på den observer som vi har som inparameter i metoden. Vi har en ExecuteNonQuery eftersom vi vill ändra nåt i databasen, inte hämta något.
        public void DeleteObserver(Observer observer)
        {
            string stmt = "DELETE FROM Observer WHERE id = @id";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(stmt, conn);

            command.Parameters.AddWithValue("id", observer.Id);
            command.ExecuteNonQuery();
        }

        //Hämtar all information från geolocation-tabellen och sätter in den i en lista utav geolocation-klassen. Sätter värdet på de olika propertiesarna i Geolocation-klassen till det som vi hämtar från databasen.
        public List<Geolocation> GetGeolocation()
        {
            string stmt = "SELECT * FROM Geolocation";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var command = new NpgsqlCommand(stmt, conn);
            using var reader = command.ExecuteReader();
            var geolocations = new List<Geolocation>();
            Geolocation geolocation = null;

            while (reader.Read())
            {
                geolocation = new Geolocation
                {
                    Id = (int)reader["id"],
                    Latitude = Convert.IsDBNull(reader["latitude"]) ? null : (double?)reader["latitude"],
                    Longitude = Convert.IsDBNull(reader["longitude"]) ? null : (double?)reader["longitude"],
                    AreaId = Convert.IsDBNull(reader["area_id"]) ? null : (int?)reader["area_id"]
                };
                geolocations.Add(geolocation);
            }
            return geolocations;
        }

        //Hämtar endast de geolocations från geolocation-tabellen där area_id är samma som det areaID som vi skickar in som inparameter i metoden. Sätter värdet på de olika propertiesarna i Geolocation-klassen till det som vi hämtar från databasen.
        public Geolocation GetGeolocationById(int? areaId)
        {
            string stmt = "SELECT * FROM Geolocation WHERE area_id = @area_id";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var command = new NpgsqlCommand(stmt, conn);
            command.Parameters.AddWithValue("@area_id", areaId);
            using var reader = command.ExecuteReader();
            Geolocation geolocation = null;
           
            while (reader.Read())
            {
                geolocation = new Geolocation
                {
                    Id = (int)reader["id"],
                    Latitude = Convert.IsDBNull(reader["latitude"]) ? null : (double?)reader["latitude"],
                    Longitude = Convert.IsDBNull(reader["longitude"]) ? null : (double?)reader["longitude"],
                    AreaId = Convert.IsDBNull(reader["area_id"]) ? null : (int?)reader["area_id"]
                };
            }
            return geolocation;
        }

        public List<Area> GetArea()
        {
            string stmt = "SELECT * FROM Area";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var command = new NpgsqlCommand(stmt, conn);
            using var reader = command.ExecuteReader();
            var areas = new List<Area>();
            Area area = null;

            while (reader.Read())
            {
                area = new Area
                {
                    Id = (int?)reader["id"],
                    Name = Convert.IsDBNull(reader["name"]) ? null : (string)reader["name"],
                    CountryId = Convert.IsDBNull(reader["country_id"]) ? null : (int?)reader["country_id"]
                };
                areas.Add(area);
            }
            return areas;
        }

        public List<Country> GetCountry()
        {
            string stmt = "SELECT * FROM Country";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var command = new NpgsqlCommand(stmt, conn);
            using var reader = command.ExecuteReader();
            var countries = new List<Country>();
            Country country = null;

            while (reader.Read())
            {
                country = new Country
                {
                    Id = (int)reader["id"],
                    CountryName = Convert.IsDBNull(reader["country"]) ? null : (string)reader["country"],

                };
                countries.Add(country);
            }
            return countries;
        }

        // Metod som kollar om en observatör har utfört en observation eller inte
        public bool CheckObservations(Observer observer)
        {
            bool ObservationMade;

            string stmt = "SELECT Observer_id FROM Observation WHERE observer_id = @obs_id";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var command = new NpgsqlCommand(stmt, conn);
            command.Parameters.AddWithValue("obs_id", observer.Id);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                observation = new Observation
                {
                    ObserverId = (int)reader["observer_id"],
                };
            }

            if (observation.ObserverId == observer.Id)
            {
                ObservationMade = true;
            }
            else
            {
                ObservationMade = false;
            }
            return ObservationMade;
        }

        //Metod med transaktion, samtliga villkor måste vara uppfyllda för att transaktionen/observationen ska genomföras
        public void MakeObservationWithTransaction (Observation observation, List<Measurement> measurements)
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            var transaction = conn.BeginTransaction();
            string stmt = "INSERT INTO observation (observer_id, geolocation_id, date) VALUES (@observer_id, @geolocation_id, @date)";
            string stmt1 = "INSERT INTO measurement (observation_id, category_id, value) VALUES (@observation_id, @category_id, @value)";
            string stmt2 = "SELECT * from observation ORDER BY id DESC LIMIT 1";

            try
            {
                 //var command = new NpgsqlCommand(stmt, conn);
                 var command = new NpgsqlCommand(stmt, conn);

                command.Transaction = transaction;

                command.Parameters.AddWithValue("observer_id", observation.ObserverId);
                command.Parameters.AddWithValue("geolocation_id", observation.GeolocationId);
                command.Parameters.AddWithValue("date", observation.CurrentDate);
                command.ExecuteNonQuery();

                //Hämtar ID från ovan tillagda observation
                int obsId = 0;
                  
                command = new NpgsqlCommand(stmt2, conn);
                command.Parameters.AddWithValue("id", observation.Id);
                command.Transaction = transaction;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        obsId = reader.GetInt32(0);
                    }
                }
                    foreach (var measurement in measurements)
                    {
                        command = new NpgsqlCommand(stmt1, conn);
                        command.Transaction = transaction;
                        command.Parameters.AddWithValue("observation_id", obsId);
                        command.Parameters.AddWithValue("category_id", measurement.CategoryID);
                        command.Parameters.AddWithValue("value", measurement.Value);
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }
                transaction.Commit();
                conn.Close();
            }
            catch (PostgresException ex)
            {
                transaction.Rollback();
                conn.Close();
                string errorCide = ex.SqlState;
                throw new Exception("Något är fel i din observation");
            }          
        }

        //Hämtar alla observationer som gjorts
        public List<Observation> GetObservations()
        {
            string stmt = "SELECT * FROM Observation";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var command = new NpgsqlCommand(stmt, conn);
            using var reader = command.ExecuteReader();
            var observations = new List<Observation>();
            Observation observationName = null;

            while (reader.Read())
            {
                observationName = new Observation
                {
                    Id = (int)reader["id"],
                    CurrentDate = Convert.IsDBNull(reader["date"]) ? null : (DateTime?)reader["date"],
                    ObserverId = Convert.IsDBNull(reader["observer_id"]) ? null : (int?)reader["observer_id"],
                    GeolocationId = Convert.IsDBNull(reader["geolocation_id"]) ? null : (int?)reader["geolocation_id"]
                };
                observations.Add(observationName);
            }
            return observations;
        }

        //Hämtar samtliga observationer från en specifik observatör
        public List<Observation> GetObservationsFromSpecificObserver(Observer observer)
        {
            string stmt = "SELECT * FROM Observation WHERE observer_id = @observer_id";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var command = new NpgsqlCommand(stmt, conn);

            command.Parameters.AddWithValue("@observer_id", observer.Id);
            var observations = new List<Observation>();
           
            Observation observationName = null;
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                observationName = new Observation
                {
                    Id = (int)reader["id"],
                    CurrentDate = Convert.IsDBNull(reader["date"]) ? null : (DateTime?)reader["date"],
                    ObserverId = Convert.IsDBNull(reader["observer_id"]) ? null : (int?)reader["observer_id"],
                    GeolocationId = Convert.IsDBNull(reader["geolocation_id"]) ? null : (int?)reader["geolocation_id"]
                };
                observations.Add(observationName);
            }
            return observations;
        }

        //Hämtar alla kategorierna förutom huvudkategorierna, Djur, Träd och Väder
        public List<Category> GetCategory()
        {
            string stmt = "SELECT * FROM category WHERE basecategory_id < 6";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var command = new NpgsqlCommand(stmt, conn);

            using var reader = command.ExecuteReader();
            var categories = new List<Category>();
            Category category = null;

            while (reader.Read())
            {
                category = new Category
                {
                    Id = (int)reader["id"],
                    Name = Convert.IsDBNull(reader["name"]) ? null : (string)reader["name"],
                    BaseCategoryID = Convert.IsDBNull(reader["basecategory_id"]) ? null : (int?)reader["basecategory_id"],
                    UnitId = Convert.IsDBNull(reader["unit_id"]) ? null : (int?)reader["unit_id"]
                };
                categories.Add(category);
            }
            return categories;
        }

        public List<Unit> GetUnits()
        {
            string stmt = "SELECT * FROM unit";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var command = new NpgsqlCommand(stmt, conn);
            using var reader = command.ExecuteReader();
            var units = new List<Unit>();
            Unit unit = null;

            while (reader.Read())
            {
                unit = new Unit
                {
                    Id = (int)reader["id"],
                    Type = Convert.IsDBNull(reader["type"]) ? null : (string)reader["type"],
                    Abbreviation = Convert.IsDBNull(reader["abbreviation"]) ? null : (string)reader["abbreviation"],
                };
                units.Add(unit);
            }
            return units;
        }

        //Metod som hämtar measurements från en observation
        public List<Measurement> GetMeasurementsFromObservation(int observationNr)
        {
            string stmt = "SELECT * FROM measurement WHERE Observation_id = @observationNr";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var command = new NpgsqlCommand(stmt, conn);
            command.Parameters.AddWithValue("@observationNr", observationNr);
            using var reader = command.ExecuteReader();
            var measurements = new List<Measurement>();
            Measurement measurement = null;

            while (reader.Read())
            {
                measurement = new Measurement
                {
                    Id = (int)reader["id"],
                    Value = Convert.IsDBNull(reader["value"]) ? null : (double?)reader["value"],
                    ObservationId = Convert.IsDBNull(reader["observation_id"]) ? null : (int?)reader["observation_id"],
                    CategoryID = Convert.IsDBNull(reader["category_id"]) ? null : (int?)reader["category_id"]
                };
                measurements.Add(measurement);
            }
            return measurements;
        }

        public Category GetSpecificCategory(int? categoryId)
        {
            string stmt = "SELECT * FROM category WHERE id = @categoryId";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var command = new NpgsqlCommand(stmt, conn);
            command.Parameters.AddWithValue("@categoryId", categoryId);
            using var reader = command.ExecuteReader();
           
            Category category = null;

            while (reader.Read())
            {
                category = new Category
                {
                    Id = (int)reader["id"],
                    Name = Convert.IsDBNull(reader["name"]) ? null : (string)reader["name"],
                    BaseCategoryID = Convert.IsDBNull(reader["basecategory_id"]) ? null : (int?)reader["basecategory_id"],
                    UnitId = Convert.IsDBNull(reader["unit_id"]) ? null : (int?)reader["unit_id"]
                };       
            }
            return category;
        }

        public Unit GetSpecificUnit(int? unitId)
        {
            string stmt = "SELECT * FROM unit WHERE id = @unitId";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var command = new NpgsqlCommand(stmt, conn);
            command.Parameters.AddWithValue("@unitId", unitId);
            using var reader = command.ExecuteReader();

            Unit unit = null;

            while (reader.Read())
            {
                unit = new Unit
                {
                    Id = (int)reader["id"],
                    Type = Convert.IsDBNull(reader["type"]) ? null : (string)reader["type"],
                    Abbreviation = Convert.IsDBNull(reader["abbreviation"]) ? null : (string)reader["abbreviation"],
                };
            }
            return unit;
        }

        public void UpdateMeasurements(List<Measurement> measurements)
        {
            string stmt = "UPDATE measurement SET value =@value, category_id=@category_id WHERE id = @id";
            try
            {
                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();
                using var command = new NpgsqlCommand(stmt, conn);
                
                foreach (var measurement in measurements)
                {    
                    command.Parameters.AddWithValue("@id", measurement.Id);
                    command.Parameters.AddWithValue("value", measurement.Value ?? Convert.DBNull);
                    command.Parameters.AddWithValue("category_id", measurement.CategoryID);
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }      
            }
            catch (PostgresException ex)
            {
                string errorCode = ex.SqlState;
                throw new Exception("Du har matat in något fel", ex);
            } 
        }
    }
}
    
