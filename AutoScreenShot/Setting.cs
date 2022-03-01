using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityModManagerNet;

namespace AutoScreenShot
{
    public class Setting : UnityModManager.ModSettings
    {
        public bool onDeath = true;
        public bool onComplete = true;
        public bool saveClipboard = true;
        public bool saveFiles = true;
        public double percent = 0;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            var filepath = GetPath(modEntry);
            try
            {
                using (var writer = new StreamWriter(filepath))
                {
                    var serializer = new XmlSerializer(GetType());
                    serializer.Serialize(writer, this);
                }
            }
            catch
            {
            }
        }

        public override string GetPath(UnityModManager.ModEntry modEntry)
        {
            return Path.Combine(modEntry.Path, GetType().Name + ".xml");
        }

    }
}
