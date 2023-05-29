using MemoryGame.Models;
using System.Collections.Generic;
using System.IO;

namespace MemoryGame
{
    public class GameScoresSaver
    {
        private const string FilePath = "scores.bin";

        public void Serialize(List<Score> scores)
        {
            using (Stream stream = File.Open(FilePath, FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bformatter.Serialize(stream, scores);
            }
        }

        public List<Score> Desrialize()
        {
            using (Stream stream = File.Open(FilePath, FileMode.Open))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                return (List<Score>)bformatter.Deserialize(stream);
            }
        }
    }
}
