using System.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace Flashcards
{
    class StudyController
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        internal int ReadRandomFlashcards(int id)
        {
            TableVisualisation tableVisualisation = new();
            List<FlashcardModel> flashcards = new();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT * FROM Flashcards WHERE StackId = {id}";

                flashcards = connection.Query<FlashcardModel>(query).ToList();

                connection.Close();
            }
            int flashcardCount = flashcards.Count;
            if (flashcardCount > 0) tableVisualisation.ShowRandomFlashcards(flashcards);

            return flashcardCount;
        }
        internal List<StudyModel> View()
        {
            TableVisualisation tableVisualisation = new();
            List<StudyModel> study = new();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = $"SELECT * FROM Study";

                study = conn.Query<StudyModel>(query).ToList();

                conn.Close();
            }
            if (study.Count > 0)
                tableVisualisation.ShowStudyHistory(study);
            else
            {
                Console.WriteLine("No history of studying. Press enter to go back to main menu.");
                Console.ReadLine();
            }
            return study;
        }

        internal void Insert(int score, string date)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var tableCmd = conn.CreateCommand();

                tableCmd.CommandText = $"INSERT INTO Study(date, score) VALUES ('{date}', '{score}')";

                tableCmd.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}