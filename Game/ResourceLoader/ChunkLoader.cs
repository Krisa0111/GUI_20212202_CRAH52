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
        private struct EntityDef
        {
            public Type entityType;
            public Vector3 position;

            public EntityDef(Type entityType, Vector3 position)
            {
                this.entityType = entityType;
                this.position = position;
            }

            public Entity GetEntity(Vector3 offset)
            {
                Entity entity = Activator.CreateInstance(entityType, args: new object[] { position + offset }) as Entity;
                return entity;
            }
        }

        const string ENTITIES_NAME_SPACE = "Game.ViewModel.Entities";
        private static Random rnd = new Random();
        private IList<IList<EntityDef>> chunks = new List<IList<EntityDef>>();

        public ChunkLoader(string direcory)
        {
            string[] files = Directory.GetFiles(direcory);

            foreach (string file in files)
            {
                var lines = File.ReadAllLines(file);
                List<EntityDef> entityDefs = new List<EntityDef>();

                foreach (string line in lines)
                {
                    try
                    {
                        var def = ParseEntityDef(line);
                        if (typeof(Entity).IsAssignableFrom(def.entityType))
                            entityDefs.Add(def);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }

                chunks.Add(entityDefs);
            }
        }

        public IReadOnlyList<Entity> GetRandomChunk(Vector3 offset)
        {
            List<Entity> entities = new List<Entity>();

            int index = rnd.Next(0, chunks.Count);
            var chunk = chunks[index];

            foreach (var item in chunk)
            {
                entities.Add(item.GetEntity(offset));
            }

            return entities;
        }

        private EntityDef ParseEntityDef(string code)
        {
            string[] parts = code.Split(" ");
            Type type = Type.GetType(ENTITIES_NAME_SPACE + "." + parts[0]);

            float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
            float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
            float z = float.Parse(parts[3], CultureInfo.InvariantCulture);

            EntityDef def = new EntityDef(type, new Vector3(x, y, z));

            return def;
        }
    }
}
