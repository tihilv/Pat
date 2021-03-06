﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Pat.Api.Modules;
using Pat.Api.Services;

namespace Pat.BusinessLogic.Services
{
    class OptionsService : IOptionsService
    {
        private readonly string _filename;
        private readonly Dictionary<string, IOptions> _savedOptions;

        public OptionsService(string filename)
        {
            _filename = filename;
            _savedOptions = ReadOptions(filename);
        }

        private Dictionary<string, IOptions> ReadOptions(string filename)
        {
            try
            {
                if (File.Exists(filename))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (FileStream stream = new FileStream(filename, FileMode.Open))
                    {
                        return (Dictionary<string, IOptions>) formatter.Deserialize(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception at options deserialization: {ex.ToString()}");
            }

            return new Dictionary<string, IOptions>();
        }

        public IOptions GetSavedOptions(IOptions emptyOptions)
        {
            if (emptyOptions == null)
                return null;

            var typeName = emptyOptions.GetType().Name;
            if (!_savedOptions.TryGetValue(typeName, out var saved))
            {
                saved = emptyOptions;
                _savedOptions[typeName] = saved;
            }

            return saved;
        }

        public void SaveOptions()
        {
            SaveOptions(_filename, _savedOptions);
        }

        private void SaveOptions(string filename, Dictionary<string, IOptions> options)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(filename, FileMode.Create))
            {
                formatter.Serialize(stream, options);
                stream.Close();
            }
        }
    }
}
