using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RestaurantDirectory.Objects
{
  public class Restaurant
  {
    string _name;
    int _cuisine_id;
    string _address;
    string _website;
    string _phone;
    int _id;

    public Restaurant(string name, int cuisine_id, string address = "", string website = "", string phone = "", int id = 0)
    {
      _name = name;
      _cuisine_id = cuisine_id;
      _address = address;
      _website = website;
      _phone = phone;
      _id = id;
    }

    public int GetId()
    {
      return _id;
    }
    public int GetCuisineId()
    {
      return _cuisine_id;
    }
    public string GetCuisineName()
    {
      string cuisineName = Cuisine.Find(_cuisine_id).GetName();
      return cuisineName;
    }
    public string GetName()
    {
      return _name;
    }
    public string GetAddress()
    {
      return _address;
    }
    public string GetWebsite()
    {
      return _website;
    }
    public string GetPhone()
    {
      return _phone;
    }

    public override bool Equals(Object otherRestaurant)
    {
      if (!(otherRestaurant is Restaurant))
      {
        return false;
      }
      else
      {
      Restaurant newRestaurant = (Restaurant) otherRestaurant;
      bool idEquality = (this.GetId() == newRestaurant.GetId());
      bool nameEquality = (this.GetName() == newRestaurant.GetName());
      bool cuisineIdEquality = (this.GetCuisineId() == newRestaurant.GetCuisineId());
      bool addressEquality = (this.GetAddress() == newRestaurant.GetAddress());
      bool websiteEquality = (this.GetWebsite() == newRestaurant.GetWebsite());
      bool phoneEquality = (this.GetPhone() == newRestaurant.GetPhone());
      return (idEquality && nameEquality && cuisineIdEquality);
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM restaurants;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static List<Restaurant> GetAll()
    {
      List<Restaurant> allRestaurants = new List<Restaurant> {};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        int cuisine_id = rdr.GetInt32(2);
        string address = rdr.GetString(3);
        string website = rdr.GetString(4);
        string phone = rdr.GetString(5);
        Restaurant newRestaurant =  new Restaurant(name, cuisine_id, address, website, phone, id);
        allRestaurants.Add(newRestaurant);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return allRestaurants;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO restaurants (name, cuisine_id, address, website, phone) OUTPUT INSERTED.id VALUES (@name, @cuisine_id, @address, @website, @phone);", conn);

      SqlParameter[] insertParameters = new SqlParameter[]
      {
        new SqlParameter("@name", this.GetName()),
        new SqlParameter("@cuisine_id", this.GetCuisineId()),
        new SqlParameter("@address", this.GetAddress()),
        new SqlParameter("@website", this.GetWebsite()),
        new SqlParameter("@phone", this.GetPhone()),
      };

      cmd.Parameters.AddRange(insertParameters);

      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }
    public static Restaurant Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants WHERE id = @RestaurantId;", conn);
      SqlParameter restaurantIdParameter = new SqlParameter();
      restaurantIdParameter.ParameterName = "@RestaurantId";
      restaurantIdParameter.Value = id.ToString();
      cmd.Parameters.Add(restaurantIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundRestaurantId = 0;
      string foundRestaurantName = null;
      int foundRestaurantCuisineId = 0;
      string foundRestaurantAddress = "";
      string foundRestaurantWebsite = "";
      string foundRestaurantPhone = "";

      while(rdr.Read())
      {
        foundRestaurantId = rdr.GetInt32(0);
        foundRestaurantName = rdr.GetString(1);
        foundRestaurantCuisineId = rdr.GetInt32(2);
        foundRestaurantAddress = rdr.GetString(3);
        foundRestaurantWebsite = rdr.GetString(4);
        foundRestaurantPhone = rdr.GetString(5);
      }

      Restaurant foundRestaurant = new Restaurant(foundRestaurantName, foundRestaurantCuisineId, foundRestaurantAddress, foundRestaurantWebsite, foundRestaurantPhone, foundRestaurantId);

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return foundRestaurant;
    }

    public void Edit(string newName, string newAddress, string newWebsite, string newPhone)
    {
      if(newName == "") {newName = this.GetName();}
      if(newAddress == "") {newAddress = this.GetAddress();}
      if(newWebsite == "") {newWebsite = this.GetWebsite();}
      if(newPhone == "") {newPhone = this.GetPhone();}

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE restaurants SET name = @NewName, address = @address, website = @website, phone = @phone OUTPUT INSERTED.name, INSERTED.address, INSERTED.website, INSERTED.phone WHERE id = @RestaurantId;", conn);

      SqlParameter[] insertParameters = new SqlParameter[]
      {
        new SqlParameter("@NewName", newName),
        new SqlParameter("@address", newAddress),
        new SqlParameter("@website", newWebsite),
        new SqlParameter("@phone", newPhone),
        new SqlParameter("@RestaurantId", this.GetId())
      };

      cmd.Parameters.AddRange(insertParameters);
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        this._name = rdr.GetString(0);
        this._address = rdr.GetString(1);
        this._website = rdr.GetString(2);
        this._phone = rdr.GetString(3);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }
    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM restaurants WHERE id = @RestaurantId;", conn);
      SqlParameter restaurantIdParameter = new SqlParameter("@RestaurantId", this.GetId());

      cmd.Parameters.Add(restaurantIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
