using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System;
using Mono.Data.Sqlite;
using System.IO;

public class DatabaseManager : MonoBehaviour {

	private string path;
	public GameObject plant01;
	public GameObject plant02;

	// Use this for initialization
	void Start () {
		checkPath ();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void addPlant(float x, float z, int[] dna)
    {
        using (IDbConnection dbConnection = new SqliteConnection("URI=file:" + path))
        {
            dbConnection.Open();
            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                string query = String.Format("INSERT INTO Plant(stalk, cup, leaves, creepers, spikes, teeth, fruits) values (\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\")", dna[0], dna[1], dna[2], dna[3], dna[4], dna[5], dna[6]);
                dbCommand.CommandText = query;
                dbCommand.ExecuteScalar();

                query = "SELECT last_insert_rowid()";
                dbCommand.CommandText = query;
                dbCommand.CommandType = CommandType.Text;
                int plant_id = Convert.ToInt32(dbCommand.ExecuteScalar());
                Debug.Log("Pobrane plant_id: " + plant_id);

                query = String.Format("INSERT INTO PlantsOnField(posX, posY, posZ, plant_id) values (\"{0}\",\"{1}\",\"{2}\",\"{3}\")", x, 0, z, plant_id);
                dbCommand.CommandText = query;
                dbCommand.ExecuteScalar();

                dbConnection.Close();
            }
        }
    }

    public Plant[] getPlants()
    {
        int count = 0;
        Plant[] plants;
        using (IDbConnection dbConnection = new SqliteConnection("URI=file:" + path))
        {
            dbConnection.Open();
            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                string query = "SELECT COUNT(id) from PlantsOnField";
                dbCommand.CommandText = query;
                dbCommand.CommandType = CommandType.Text;
                count = Convert.ToInt32(dbCommand.ExecuteScalar());
                plants = new Plant[count];
                
                query = "SELECT posX, posY, posZ, stalk, cup, leaves, creepers, spikes, teeth, fruits FROM Plant, PlantsOnField WHERE Plant.id = PlantsOnField.plant_id";
                dbCommand.CommandText = query;
                using (IDataReader reader = dbCommand.ExecuteReader())
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        Plant plant = new Plant(new Vector3(reader.GetFloat(0), reader.GetFloat(1), reader.GetFloat(2)), 
                            new int[] { reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetInt32(6), reader.GetInt32(7), reader.GetInt32(8), reader.GetInt32(9)});
                        plants[i] = plant;
                        i++;
                    }
                    reader.Close();
                }
                dbConnection.Close();
            }
        }
        Debug.Log("Count of plants in DB: " + count);

        return plants;
    }

    public PlantInBag[] getBagPlants()
    {
        int count = 0;
        PlantInBag[] plants;
        using (IDbConnection dbConnection = new SqliteConnection("URI=file:" + path))
        {
            dbConnection.Open();
            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                string query = "SELECT COUNT(id) from PlantsInBag";
                dbCommand.CommandText = query;
                dbCommand.CommandType = CommandType.Text;
                count = Convert.ToInt32(dbCommand.ExecuteScalar());
                plants = new PlantInBag[count];

                query = "SELECT stalk, cup, leaves, creepers, spikes, teeth, fruits, count FROM Plant, PlantsInBag WHERE Plant.id = PlantsInBag.plant_id";
                dbCommand.CommandText = query;
                using (IDataReader reader = dbCommand.ExecuteReader())
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        PlantInBag plant = new PlantInBag(new int[] { reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetInt32(6) }, reader.GetInt32(7));
                        
                        plants[i] = plant;
                        i++;
                    }
                    reader.Close();
                }
                dbConnection.Close();
            }
        }
        Debug.Log("Count of plants in backpack: " + count);

        return plants;
    }
    

    public void removePlant(float x, float z)
    {
        using (IDbConnection dbConnection = new SqliteConnection("URI=file:" + path))
        {
            dbConnection.Open();
            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                string query = String.Format("SELECT plant_id FROM PlantsOnField WHERE posX=\"{0}\" and posZ=\"{1}\"", x, z);
                dbCommand.CommandText = query;
                dbCommand.CommandType = CommandType.Text;
                int plant_id = Convert.ToInt32(dbCommand.ExecuteScalar());

                query = String.Format("DELETE FROM PlantsOnField WHERE posX=\"{0}\" and posZ=\"{1}\"", x, z);
                Debug.Log(query);
                dbCommand.CommandText = query;
                dbCommand.ExecuteScalar();

                query = "DELETE FROM Plant WHERE id =" + plant_id;
                Debug.Log(query);
                dbCommand.CommandText = query;
                dbCommand.ExecuteScalar();

                dbConnection.Close();
            }
        }
    }

    public int[] getDnaOfPlant(float x, float z)
    {
        int[] dna;

        using (IDbConnection dbConnection = new SqliteConnection("URI=file:" + path))
        {
            dbConnection.Open();
            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                string query = String.Format("SELECT stalk, cup, leaves, creepers, spikes, teeth, fruits FROM Plant, PlantsOnField WHERE Plant.id = PlantsOnField.plant_id AND PlantsOnField.posX =\"{0}\" AND PlantsOnField.posZ =\"{1}\"", x, z);
                dbCommand.CommandText = query;
                using (IDataReader reader = dbCommand.ExecuteReader())
                {
                    reader.Read();
                    dna = new int[] { reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetInt32(6)};
                    reader.Close();
                }
                dbConnection.Close();
            }
        }
        return dna;
    }

    public void addPlantToBag(int[] dna)
    {
        using (IDbConnection dbConnection = new SqliteConnection("URI=file:" + path))
        {
            dbConnection.Open();
            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                string query =String.Format("SELECT COUNT(PlantsInBag.id) from Plant, PlantsInBag WHERE Plant.id = plant_id AND stalk =\"{0}\" AND cup =\"{1}\" AND leaves = \"{2}\" AND creepers =\"{3}\" AND spikes =\"{4}\" AND teeth =\"{5}\" AND fruits=\"{6}\"", dna[0], dna[1], dna[2], dna[3], dna[4], dna[5], dna[6]);
                dbCommand.CommandText = query;
                dbCommand.CommandType = CommandType.Text;
                int count = Convert.ToInt32(dbCommand.ExecuteScalar());

                Debug.Log("count: " + count);

                if (count != 0)
                {
                    //query = "UPDATE PlantsInBag SET count=count-1 WHERE plant_id=(SELECT id from Plant where Plant.id = plant_id AND stalk =1 AND cup =1 AND leaves = 0 AND creepers =0 AND spikes =0 AND teeth =0 AND fruits=0)";
                    query = String.Format("UPDATE PlantsInBag SET count=count+1 WHERE plant_id=(SELECT id from Plant where Plant.id = plant_id AND stalk =\"{0}\" AND cup =\"{1}\" AND leaves = \"{2}\" AND creepers =\"{3}\" AND spikes =\"{4}\" AND teeth =\"{5}\" AND fruits=\"{6}\")", dna[0], dna[1], dna[2], dna[3], dna[4], dna[5], dna[6]);
                    dbCommand.CommandText = query;
                    dbCommand.ExecuteNonQuery();
                }
                else {
                    query = String.Format("INSERT INTO Plant(stalk, cup, leaves, creepers, spikes, teeth, fruits) values (\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\")", dna[0], dna[1], dna[2], dna[3], dna[4], dna[5], dna[6]);
                    dbCommand.CommandText = query;
                    dbCommand.ExecuteScalar();

                    query = "SELECT last_insert_rowid()";
                    dbCommand.CommandText = query;
                    dbCommand.CommandType = CommandType.Text;
                    int plant_id = Convert.ToInt32(dbCommand.ExecuteScalar());

                    query = String.Format("INSERT INTO PlantsInBag(count, plant_id) values (\"{0}\",\"{1}\")", 1, plant_id);
                    dbCommand.CommandText = query;
                    dbCommand.ExecuteScalar();
                }

                dbConnection.Close();
            }
        }
    }

    private void checkPath() {
		if (Application.platform != RuntimePlatform.Android) {
			path = Application.dataPath + "/PolyGainDB.sqlite";
		} else {
			path = Application.persistentDataPath + "/PolyGainDB.sqlite";
			if(!File.Exists(path)){
				WWW load = new WWW ("jar:file://" + Application.dataPath + "!/assets/" + "PolyGainDB.sqlite"); 
				while (!load.isDone){}
				File.WriteAllBytes (path, load.bytes);
			}    
		}
	}

}
