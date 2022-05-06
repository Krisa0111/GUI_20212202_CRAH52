using Game.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Game.ResourceLoader
{
    internal class ChunkLoader
    {
        const string ENTITIES_NAME_SPACE = "Game.ViewModel.Entities";
        private static Random rnd = new Random();
        private IList<string[]> chunks = new List<string[]>();

        public ChunkLoader(string direcory)
        {
            string[] files = Directory.GetFiles(direcory);

            for (int i = 0; i < files.Length; i++)
            {
                chunks.Add(File.ReadAllLines(files[i]));
            }
        }

        public IReadOnlyList<Entity> GetRandomChunk(Vector3 offset)
        {
            List<Entity> entities = new List<Entity>();

            int index = rnd.Next(0, chunks.Count);
            var chunk = chunks[index];

            for (int i = 0; i < chunk.Length; i++)
            {
                try
                {
                    entities.Add(createEntity(chunk[i], offset));
                } catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            return entities;
        }

        private Entity createEntity(string code, Vector3 offset)
        {
            string[] parts = code.Split(" ");
            Type type = Type.GetType(ENTITIES_NAME_SPACE + "." + parts[0]);

            float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
            float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
            float z = float.Parse(parts[3], CultureInfo.InvariantCulture);

            Vector3 pos = new Vector3(x, y, z) + offset;
            Entity entity = Activator.CreateInstance(type, args: new object[] { pos }) as Entity;

            return entity;
        }
    }
}
